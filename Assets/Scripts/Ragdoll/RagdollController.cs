using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollController : MonoBehaviour
{


	public Animator animator = null;

	new Rigidbody rigidbody;
	CapsuleCollider capsuleCollider;

	private Transform mainCameraTransform = null;
    [SerializeField]
    Transform spine, aim;

	Vector3 targetRotation;

    [SerializeField]
	private float rotationSpeed = 1f; // 10
    [SerializeField]
	private float speed = 3f;
    [SerializeField]
	private float jumpForce = 3;
    [SerializeField]
	private float outValue = 10;
	private float vertical;
	private float horizontal;
    private float mouseY;
    [SerializeField]
    private float clampMin = -35;
    [SerializeField]
    private float clampMax = 30;

	bool isGround = true;
	public bool alive = true;

    public IKReach ikReach;



    /// <summary>
    /// 
    /// /Johan
    /// </summary>
	void Start()
    {
		rigidbody = GetComponent<Rigidbody>();
		capsuleCollider = GetComponent<CapsuleCollider>();
		mainCameraTransform = Camera.main.transform;
        
	}

    /// <summary>
    /// 
    /// /Johan
    /// </summary>
	void Update()
    {
        Aim();

        if (Input.GetButtonDown("Jump") && isGround)
        {
            rigidbody.AddForce(new Vector3(0, jumpForce * 100, 0), ForceMode.Impulse);
            isGround = false;
        }
    }


    /// <summary>
    /// 
    /// /Johan
    /// </summary>
    void FixedUpdate()
    {
        if (alive)
        {
            Move();
            Bend();
        }
        else
        {
            animator.enabled = false;
        }


    }

    /// <summary>
    /// Böjer radollens överkropp till siktets rotation i y-led.
    /// /Jonathan & Johan
    private void Aim()
    {
        mouseY += Input.GetAxisRaw("Mouse Y");  //om vi ändra lookSensitivity i cameraController, se över den här.
        mouseY = Mathf.Clamp(mouseY, clampMin, clampMax);
        aim.eulerAngles = new Vector3(-mouseY * 2, transform.eulerAngles.y, transform.eulerAngles.z);
    }
    /// </summary>
    private void Bend()
    {
        spine.eulerAngles = new Vector3(spine.eulerAngles.x, spine.eulerAngles.y, -aim.eulerAngles.x - 90);
    }

    /// <summary>
    /// 
    /// /Johan (& Jonathan)
    /// </summary>
    private void Move()
    {
        //if (ikReach.GetIsGrabbingHeavy())
        //{
        //    MoveBackwards();
        //}


        Vector3 forward = mainCameraTransform.forward;
        forward.y = 0;
        forward = forward.normalized;
        Vector3 right = new Vector3(forward.z, 0, -forward.x);

        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
      
        Vector3 moveDirection = (horizontal * right + vertical * forward);
        moveDirection.Normalize();
        if(moveDirection != Vector3.zero)
        {
            targetRotation = Quaternion.LookRotation(moveDirection).eulerAngles;
            rigidbody.rotation = Quaternion.Slerp(rigidbody.rotation, Quaternion.Euler(targetRotation.x, targetRotation.y, targetRotation.z), Time.fixedDeltaTime * rotationSpeed);
        }


        Vector3 input = new Vector3(horizontal, 0, vertical);

        if (input != Vector3.zero)
        {
            animator.enabled = true;
        }
        else if (input.sqrMagnitude == 0)
        {
            animator.enabled = false;
        }


        Vector3 velocity = moveDirection * speed;
        rigidbody.MovePosition(rigidbody.position + velocity * Time.fixedDeltaTime);
    }

    //private void MoveBackwards()
    //{
    //    if (Input.GetKey(KeyCode.S))
    //    {
    //        Vector3 backwards = new Vector3(0, 0, -1);
    //        // Addforce, clamp dollydoll
    //    }

    //}

    /// <summary>
    /// 
    /// /Johan
    /// </summary>
    void OnCollisionEnter(Collision other)
	{
		if (other.relativeVelocity.magnitude > outValue)
		{
			StartCoroutine(Out());
		}
        isGround = true;


        
	}

    private void OnCollisionStay(Collision other)
    {
        HandleColourCollision(other);
    }

    /// <summary>
    /// Detects if the character is in collision with a colour based on distance from vertices of the colliding gameobject 
    /// Mattias Smedman
    /// </summary>
    /// <param name="other"></param>
    private void HandleColourCollision(Collision other)
    {
        if (other.gameObject.GetComponent<Paintable>() != null)
        {
            Mesh collisionObjectMesh = other.gameObject.GetComponent<MeshFilter>().mesh;
            Vector3[] collisionObjectVertices = collisionObjectMesh.vertices;
            Vector3 contact = other.GetContact(0).point;

            Vector3 center = transform.InverseTransformPoint(contact);
            Debug.Log("Contact: " + contact);
            for (int i = 0; i < collisionObjectVertices.Length; i++)
            {
                Vector3 posOfVert = transform.InverseTransformPoint(collisionObjectVertices[i]);

                float outerR = 20;

                float x = Mathf.Pow(posOfVert.x - center.x, 2);
                float y = Mathf.Pow(posOfVert.y - center.y, 2);

                float distance = Mathf.Sqrt(x - y);

                if (outerR > distance)
                {
                    if (other.gameObject.GetComponent<MeshFilter>().mesh.colors32[i].r > 0)
                    {
                        jumpForce = 6;
                    }
                }
                else
                {
                    jumpForce = 4;
                }

            }
        }
        else
        {
            jumpForce = 4;
        }
    }

    /// <summary>
    /// 
    /// /Johan
    /// </summary>
	IEnumerator Out()
	{
		rigidbody.constraints = RigidbodyConstraints.None;
		capsuleCollider.enabled = false;
		alive = false;
		yield return new WaitForSeconds(4);
		rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
		capsuleCollider.enabled = true;
		alive = true;
	}
}
