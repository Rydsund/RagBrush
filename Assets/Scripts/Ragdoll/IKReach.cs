using System;
using System.Collections.Generic;
using UnityEngine;

public class IKReach : MonoBehaviour
{
    [SerializeField]
    private RagdollController player;

    Player playerInventory;

    new Rigidbody rigidbody;

    [SerializeField]
    private KeyCode punchInput1;
    [SerializeField]
    private KeyCode punchInput2;

    [SerializeField]
    private GameObject myGrabdObj;

    [SerializeField]
    public List<string> grabbableObjects;

    protected float[] bonesLength; 
    [SerializeField]
    private float punchForce = 1;

    [SerializeField]
    float breakForceHeavy;

    [SerializeField]
    float breakForceLight;

    [SerializeField]
    float breakTorqueHeavy;

    [SerializeField]
    float breakTorqueLight;

   


    [SerializeField]
    float maxCarryWeight = 8;

    protected float startDistance;
    protected float completeLength;

    /// <summary>
    /// Styrka av att gå tillbaka till start positionen
    /// </summary>
    [Range(0, 1)]
    [SerializeField]
    private float snapBackStrength = 1f;

    /// <summary>
    /// Distans när solver stoppar.
    /// </summary>
    [SerializeField]
    private float delta = 0.001f;

    [SerializeField]
    private bool isGrabbing = false;
    protected bool isStartingReach;
    private bool isExtended;
    bool isGrabbingHeavy;

    /// <summary>
    /// chainLength av ben
    /// </summary>
    public int chainLength = 2;

    /// <summary>
    /// Solver iterations per update
    /// </summary>
    [Header("Solver Parameters")]
    [SerializeField]
    private int iterations = 10;

    /// <summary>
    /// Mål chain ska böja till
    /// </summary>
    [SerializeField]
    private Transform target;
    [SerializeField]
    private Transform aim;
    [SerializeField]
    private Transform pole;
    protected Transform root;
    protected Transform[] bones;

    protected Vector3[] positions;
    protected Vector3[] startDirectionSucc;

    protected Quaternion[] startRotationBone;
    protected Quaternion startRotationTarget;
    

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        playerInventory = GetComponentInParent<Player>();
    }

    void Awake()
    {
        Init();
    }

    void Init()//FastIK
    {
        bones = new Transform[chainLength + 1];
        positions = new Vector3[chainLength + 1];
        bonesLength = new float[chainLength];
        startDirectionSucc = new Vector3[chainLength + 1];
        startRotationBone = new Quaternion[chainLength + 1];

        root = transform;
        for (var i = 0; i <= chainLength; i++)
        {
            if (root == null)
                throw new UnityException("The chain value is longer than the ancestor chain!");
            root = root.parent;
        }

        if (target == null)
        {
            target = new GameObject(gameObject.name + " Target").transform;
            SetPositionRootSpace(target, GetPositionRootSpace(transform));
        }
        startRotationTarget = GetRotationRootSpace(target);


        var current = transform;
        completeLength = 0;
        for (var i = bones.Length - 1; i >= 0; i--)
        {
            bones[i] = current;
            startRotationBone[i] = GetRotationRootSpace(current);

            if (i == bones.Length - 1)
            {
                startDirectionSucc[i] = GetPositionRootSpace(target) - GetPositionRootSpace(current);
            }
            else
            {
                startDirectionSucc[i] = GetPositionRootSpace(bones[i + 1]) - GetPositionRootSpace(current);
                bonesLength[i] = startDirectionSucc[i].magnitude;
                completeLength += bonesLength[i];
            }

            current = current.parent;
        }
    }


    /// <summary>
    /// Hanterar loopen, kollar efter input och bestämmer vad ska hända och när det ska hända.
    /// Johan
    /// (& lite Mattias)
    /// </summary>
    void Update() 
    {
        if (player.alive)
        {
            if (Input.GetKey(punchInput1) || Input.GetKey(punchInput2))
            {
                if (myGrabdObj != null && !isGrabbing)
                {
                    GrabObject();
                }

                if (isGrabbing)
                {
                    if (Input.GetKeyDown(KeyCode.G))
                    {
                        if (myGrabdObj != null)
                        {
                            playerInventory.AddItemToInventory(myGrabdObj.GetComponent<Collider>());
                        }           
                    }
                }

                if (!isExtended)
                    MoveHandTarget();
               
                if (!isGrabbing ||                     
                     myGrabdObj != null && myGrabdObj.GetComponent<Rigidbody>().mass <= maxCarryWeight)
                {
                    ResolveIK();
                }
            }
            else if (!Input.GetKey(punchInput1) && !Input.GetKey(punchInput2))
            {
                DropObject();
            }
        }
        else
        {
            DropObject();
        }
    }

    /// <summary>
    /// Släpper det objekt som karaktären håller i. / Johan
    /// Loopar igenom all joints för att förstöra endast de skapade av denna hand. / Mattias
    /// </summary>
    private void DropObject()
    {
        if (myGrabdObj != null)
        {
            Joint[] joints = myGrabdObj.GetComponents<Joint>();
            for(int i = 0; i < joints.Length; i++)
            {
                if(joints[i].connectedBody == rigidbody)
                {
                    Destroy(joints[i]);
                }
            }
        }
        myGrabdObj = null;
        isGrabbing = false;

        target.position = aim.position;
        target.rotation = aim.rotation;
        isStartingReach = false;
        isExtended = false;
    }

    /// <summary>
    /// Hanterar att ta tag i ett objekt
    /// Johan & Mattias
    /// </summary>
    private void GrabObject()
    {
        FixedJoint fixedJoint = myGrabdObj.AddComponent<FixedJoint>();
        fixedJoint.connectedBody = rigidbody;

        FixedJoint fixedJoint2 = myGrabdObj.AddComponent<FixedJoint>();
        fixedJoint2.connectedBody = rigidbody;

        if (myGrabdObj.GetComponent<Rigidbody>().mass >= maxCarryWeight)
        {
            fixedJoint.breakForce = breakForceHeavy;
            fixedJoint2.breakForce = breakForceHeavy;

            fixedJoint.breakTorque = breakTorqueHeavy;
            fixedJoint2.breakTorque = breakTorqueHeavy;
            isGrabbingHeavy = true;
        }
        else
        {
            fixedJoint.breakForce = breakForceLight;
            fixedJoint2.breakForce = breakForceLight;

            fixedJoint.breakTorque = breakTorqueLight;
            fixedJoint2.breakTorque = breakTorqueLight;
            isGrabbingHeavy = false;
        }
        isGrabbing = true;
    }

    /// <summary>
    /// Flyttar target som armarna och händerna roterar mot.
    /// Johan
    /// </summary>
    void MoveHandTarget()//Johan
    {
        if (!isStartingReach)
        {
            target.position = transform.position;
            target.rotation = transform.rotation;
            startDistance = Vector3.Distance(target.position, aim.position);
            isStartingReach = true;
        }

        var delta = 1 - Mathf.Pow(Vector3.Distance(target.position, aim.position) / startDistance, 5.0f / 9.0f);

        target.rotation = Quaternion.Slerp(target.rotation, aim.rotation, (delta / startDistance) * punchForce);

        if (Mathf.Approximately(delta, 1))
        {
            target.position = aim.position;
            target.rotation = aim.rotation;
            isExtended = true;
            isStartingReach = false;
        }
        target.position = Vector3.MoveTowards(target.position, aim.position, punchForce * Time.deltaTime);
    }

    /// <summary>
    /// FastIK
    /// </summary>
    private void ResolveIK() 
    {
        if (target == null)
            return;

        if (bonesLength.Length != chainLength)
            Init();

        for (int i = 0; i < bones.Length; i++)
            positions[i] = GetPositionRootSpace(bones[i]);

        var targetPosition = GetPositionRootSpace(target);

        if ((targetPosition - GetPositionRootSpace(bones[0])).sqrMagnitude >= completeLength * completeLength)
        {
            ReachFirstPosition(targetPosition);
        }
        else
        {
            CheckOtherPositions(targetPosition);
        }

        if (pole != null)
        {
            MoveTowardsPole();
        }

        SetPositionAndRotation();
    }

    /// <summary>
    /// FastIK
    /// </summary>
    /// <param name="targetPosition"></param>
    private void CheckOtherPositions(Vector3 targetPosition)
    {
        for (int i = 0; i < positions.Length - 1; i++)
            positions[i + 1] = Vector3.Lerp(positions[i + 1], positions[i] + startDirectionSucc[i], snapBackStrength);

        for (int iteration = 0; iteration < iterations; iteration++)
        {
            for (int i = positions.Length - 1; i > 0; i--)
            {
                if (i == positions.Length - 1)
                    positions[i] = targetPosition; 
                else
                    positions[i] = positions[i + 1] + (positions[i] - positions[i + 1]).normalized * bonesLength[i]; 
            }

            for (int i = 1; i < positions.Length; i++)
                positions[i] = positions[i - 1] + (positions[i] - positions[i - 1]).normalized * bonesLength[i - 1];

            if ((positions[positions.Length - 1] - targetPosition).sqrMagnitude < delta * delta)
                break;
        }
    }

    /// <summary>
    /// Sträcker mot targetr
    /// Johan
    /// </summary>
    /// <param name="targetPosition"></param>
    private void ReachFirstPosition(Vector3 targetPosition)
    {
        var direction = (targetPosition - positions[0]).normalized;
        for (int i = 1; i < positions.Length; i++)
            positions[i] = positions[i - 1] + direction * bonesLength[i - 1];
    }

    /// <summary>
    /// Sätter position och rotation
    /// Johan & Jonathan
    /// </summary>
    private void SetPositionAndRotation()
    {
        for (int i = 0; i < positions.Length; i++) 
        {
            if (i != positions.Length - 1)
            {
                SetRotationRootSpace(bones[i], Quaternion.FromToRotation(startDirectionSucc[i], positions[i + 1] - positions[i]) * Quaternion.Inverse(startRotationBone[i]));
                SetPositionRootSpace(bones[i], positions[i]);
            }
        }
    }

    /// <summary>
    /// Flyttar händerna mot pole
    /// </summary>
    private void MoveTowardsPole()
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
    /// Tar tag i ett objekt om det har en tag som kan greppas
    /// Johan & Mattias
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)//Johan
    {
        if(myGrabdObj == null)
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
    }
}
