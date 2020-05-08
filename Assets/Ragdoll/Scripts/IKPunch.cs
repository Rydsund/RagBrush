#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
#endif
using UnityEngine;

public class IKPunch : MonoBehaviour
{

    new Rigidbody rigidbody;

    [SerializeField]
    private KeyCode punchInput1;
    [SerializeField]
    private KeyCode punchInput2;

    [SerializeField]
    private GameObject myGrabdObj;
    //public GameObject parentObject, grandparentObject;
    [SerializeField]
    List<string> grabbableObjects;

    [SerializeField]
    private float punchForce = 1;
    //public Transform target, parentTarget, grandparentTarget;
    [SerializeField]
    private bool isGrab = false;
    //public GameObject punch;

    /// <summary>
    /// Chain length of bones
    /// </summary>
    public int chainLength = 2;

    /// <summary>
    /// Target the chain should bent to
    /// </summary>
    [SerializeField]
    private Transform target;
    [SerializeField]
    private Transform aim;
    [SerializeField]
    private Transform pole;

    /// <summary>
    /// Solver iterations per update
    /// </summary>

    [Header("Solver Parameters")]
    [SerializeField]
    private int iterations = 10;

    /// <summary>
    /// Distance when the solver stops
    /// </summary>
    [SerializeField]
    private float delta = 0.001f;

    /// <summary>
    /// Strength of going back to the start position.
    /// </summary>
    [Range(0, 1)]
    [SerializeField]
    private float snapBackStrength = 1f;


    protected float[] bonesLength; //Target to Origin
    protected float completeLength;
    protected Transform[] bones;
    protected Vector3[] positions;
    protected Vector3[] startDirectionSucc;
    protected Quaternion[] startRotationBone;
    protected Quaternion startRotationTarget;
    //protected Rigidbody RBTarget;
    //protected Transform StartPositionTarget;
    protected Transform root;
    //protected bool startPunch;
    protected bool isPunching;
    private bool isHolding;
    protected float startDistance;

    [SerializeField]
    List<string> grabbableObjects;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        //StartPositionTarget = Target;
    }

    void Awake()
    {
        Init();
    }

    void Init()//FastIK
    {
        //initial array
        bones = new Transform[chainLength + 1];
        positions = new Vector3[chainLength + 1];
        bonesLength = new float[chainLength];
        startDirectionSucc = new Vector3[chainLength + 1];
        startRotationBone = new Quaternion[chainLength + 1];

        //find root
        root = transform;
        for (var i = 0; i <= chainLength; i++)
        {
            if (root == null)
                throw new UnityException("The chain value is longer than the ancestor chain!");
            root = root.parent;
        }

        //init target
        if (target == null)
        {
            target = new GameObject(gameObject.name + " Target").transform;
            SetPositionRootSpace(target, GetPositionRootSpace(transform));
        }
        startRotationTarget = GetRotationRootSpace(target);


        //init data
        var current = transform;
        completeLength = 0;
        for (var i = bones.Length - 1; i >= 0; i--)
        {
            bones[i] = current;
            startRotationBone[i] = GetRotationRootSpace(current);

            if (i == bones.Length - 1)
            {
                //leaf
                startDirectionSucc[i] = GetPositionRootSpace(target) - GetPositionRootSpace(current);
            }
            else
            {
                //mid bone
                startDirectionSucc[i] = GetPositionRootSpace(bones[i + 1]) - GetPositionRootSpace(current);
                bonesLength[i] = startDirectionSucc[i].magnitude;
                completeLength += bonesLength[i];
            }

            current = current.parent;
        }
    }


    void LateUpdate() //Johan
    {
        if (Input.GetKey(punchInput1) || Input.GetKey(punchInput2))
        {
            if (myGrabdObj != null && !isGrab)
            {
                FixedJoint fixedJoint = myGrabdObj.AddComponent<FixedJoint>();
                fixedJoint.connectedBody = rigidbody;
                fixedJoint.breakForce = 8000;
                isGrab = true;
            }

            if (!isHolding)
                MoveHandTarget();

            ResolveIK();
        }
        else if(!Input.GetKey(punchInput1) && !Input.GetKey(punchInput2))
        {
            if (myGrabdObj != null/* && myGrabdObj.CompareTag("Item")*/)
            {
                Destroy(myGrabdObj.GetComponent<Joint>());
            }
            myGrabdObj = null;
            isGrab = false;

            target.position = aim.position;
            target.rotation = aim.rotation;
            isPunching = false;
            isHolding = false;
        }
    }

    void MoveHandTarget()//Johan
    {
        if (!isPunching)
        {
            target.position = transform.position;
            target.rotation = transform.rotation;
            startDistance = Vector3.Distance(target.position, aim.position);
            isPunching = true;
        }

        var delta = 1 - Mathf.Pow(Vector3.Distance(target.position, aim.position) / startDistance, 5.0f / 9.0f);

        target.rotation = Quaternion.Slerp(target.rotation, aim.rotation, (delta / startDistance) * punchForce);

        if (Mathf.Approximately(delta, 1))
        {
            target.position = aim.position;
            target.rotation = aim.rotation;
            isHolding = true;
            isPunching = false;
        }
        target.position = Vector3.MoveTowards(target.position, aim.position, punchForce * Time.deltaTime);
    }

    private void ResolveIK() //FastIK
    {
        if (target == null)
            return;

        if (bonesLength.Length != chainLength)
            Init();

        //Fabric

        //  root
        //  (bone0) (bonelen 0) (bone1) (bonelen 1) (bone2)...
        //   x--------------------x--------------------x---...

        //get position
        for (int i = 0; i < bones.Length; i++)
            positions[i] = GetPositionRootSpace(bones[i]);

        var targetPosition = GetPositionRootSpace(target);
        var targetRotation = GetRotationRootSpace(target);

        //1st is possible to reach?
        if ((targetPosition - GetPositionRootSpace(bones[0])).sqrMagnitude >= completeLength * completeLength)
        {
            //just strech it
            var direction = (targetPosition - positions[0]).normalized;
            //set everything after root
            for (int i = 1; i < positions.Length; i++)
                positions[i] = positions[i - 1] + direction * bonesLength[i - 1];
        }
        else
        {
            for (int i = 0; i < positions.Length - 1; i++)
                positions[i + 1] = Vector3.Lerp(positions[i + 1], positions[i] + startDirectionSucc[i], snapBackStrength);

            for (int iteration = 0; iteration < iterations; iteration++)
            {
                //https://www.youtube.com/watch?v=UNoX65PRehA
                //back
                for (int i = positions.Length - 1; i > 0; i--)
                {
                    if (i == positions.Length - 1)
                        positions[i] = targetPosition; //set it to target
                    else
                        positions[i] = positions[i + 1] + (positions[i] - positions[i + 1]).normalized * bonesLength[i]; //set in line on distance
                }

                //forward
                for (int i = 1; i < positions.Length; i++)
                    positions[i] = positions[i - 1] + (positions[i] - positions[i - 1]).normalized * bonesLength[i - 1];

                //close enough?
                if ((positions[positions.Length - 1] - targetPosition).sqrMagnitude < delta * delta)
                    break;
            }
        }

        //move towards pole
        if (pole != null)
        {
            var polePosition = GetPositionRootSpace(pole);
            for (int i = 1; i < positions.Length - 1; i++)
            {
                var plane = new Plane(positions[i + 1] - positions[i - 1], positions[i - 1]);
                var projectedPole = plane.ClosestPointOnPlane(polePosition);
                var projectedBone = plane.ClosestPointOnPlane(positions[i]);
                var angle = Vector3.SignedAngle(projectedBone - positions[i - 1], projectedPole - positions[i - 1], plane.normal);
                positions[i] = Quaternion.AngleAxis(angle, plane.normal) * (positions[i] - positions[i - 1]) + positions[i - 1];
            }
        }

        //set position & rotation
        for (int i = 0; i < positions.Length; i++) //Johan
        {
            if (i != positions.Length - 1)
            {
                SetRotationRootSpace(bones[i], Quaternion.FromToRotation(startDirectionSucc[i], positions[i + 1] - positions[i]) * Quaternion.Inverse(startRotationBone[i]));
                SetPositionRootSpace(bones[i], positions[i]);
            }



            //if (i == Positions.Length - 1)
            //    SetRotationRootSpace(Bones[i], Quaternion.Inverse(targetRotation) * StartRotationTarget * Quaternion.Inverse(StartRotationBone[i]));
            //else
            //    SetRotationRootSpace(Bones[i], Quaternion.FromToRotation(StartDirectionSucc[i], Positions[i + 1] - Positions[i]) * Quaternion.Inverse(StartRotationBone[i]));
            //SetPositionRootSpace(Bones[i], Positions[i]);
        }
    }

    private Vector3 GetPositionRootSpace(Transform current)//FastIK
    {
        if (root == null)
            return current.position;
        else
            return Quaternion.Inverse(root.rotation) * (current.position - root.position);
    }

    private void SetPositionRootSpace(Transform current, Vector3 position)//FastIK
    {
        if (root == null)
            current.position = position;
        else
            current.position = root.rotation * position + root.position;
    }

    private Quaternion GetRotationRootSpace(Transform current)//FastIK
    {
        //inverse(after) * before => rot: before -> after
        if (root == null)
            return current.rotation;
        else
            return Quaternion.Inverse(current.rotation) * root.rotation;
    }

    private void SetRotationRootSpace(Transform current, Quaternion rotation)//FastIK
    {
        if (root == null)
            current.rotation = rotation;
        else
            current.rotation = root.rotation * rotation;
    }



    /// <summary>
    ///
    /// Johan & Mattias
    /// </summary>
    /// <param name="other"></param>
    public void OnTriggerEnter(Collider other)//Johan
    {
        foreach (string s in grabbableObjects)
        {
            if (other.gameObject.CompareTag(s))
            {
                myGrabdObj = other.gameObject;
                return;
            }
        }
    }

    //public void OnTriggerExit(Collider other)//Johan
    //{
    //    if (other.gameObject.CompareTag("Item"))
    //    {
    //        myGrabdObj = null;
    //    }
    //}



    void OnDrawGizmos()
    {
#if UNITY_EDITOR
        var current = this.transform;
        for (int i = 0; i < chainLength && current != null && current.parent != null; i++)
        {
            var scale = Vector3.Distance(current.position, current.parent.position) * 0.1f;
            Handles.matrix = Matrix4x4.TRS(current.position, Quaternion.FromToRotation(Vector3.up, current.parent.position - current.position), new Vector3(scale, Vector3.Distance(current.parent.position, current.position), scale));
            Handles.color = Color.green;
            Handles.DrawWireCube(Vector3.up * 0.5f, Vector3.one);
            current = current.parent;
        }
    }
#endif
}
