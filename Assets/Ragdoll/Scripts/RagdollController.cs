using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngineInternal;

public class RagdollController : MonoBehaviour //Johan
{
	public Animator animator = null;

	new Rigidbody rigidbody;
	CapsuleCollider capsuleCollider;

	private Transform mainCameraTransform = null;
    [SerializeField]
    Transform spineTransform;


	Vector3 targetRotation;

    [SerializeField]
	private float rotationSpeed = 10f;
    [SerializeField]
	private float speed = 3f;
    [SerializeField]
	private float jumpForce = 3;
    [SerializeField]
	private float outValue = 10;
	private float vertical;
	private float horizontal;
    private float bendVertical;



    [SerializeField]
    private Transform aimController, Chest, chestTarget;

    float mouseX, mouseY;



    bool isGround = true;
	bool alive = true;

	void Start()
    {
		rigidbody = GetComponent<Rigidbody>(); //Johan
		capsuleCollider = GetComponent<CapsuleCollider>(); //Johan
		mainCameraTransform = Camera.main.transform; //Johan
	}

	void Update()
	{
		
		if (Input.GetButtonDown("Jump") && isGround)//Johan
		{
			rigidbody.AddForce(new Vector3(0,jumpForce * 100,0), ForceMode.Impulse);
			isGround = false;
		}
	}

    void FixedUpdate()
    {
        if (alive)//Johan
        {
            Move();
            //Bend();
            //Aim();
        }



        else
        {
            animator.enabled = false;
        }


    }

    /// <summary>
    /// Böjer radollens överkropp med input från musen i y-led.
    /// /Jonathan
    /// </summary>
    private void Bend()
    {
        //horizontal = Input.GetAxis("Mouse X");
        bendVertical += Input.GetAxis("Mouse Y")/* *5*/;
        bendVertical = Mathf.Clamp(bendVertical, -45f, 18f);



        //if (Input.GetKey(KeyCode.LeftShift))
        //{
        SoftJointLimit rootLimit = spineTransform.GetComponent<CharacterJoint>().lowTwistLimit;
        //SoftJointLimit targetLimit = new SoftJointLimit() { limit = 180 };
        //spineTransform.GetComponent<CharacterJoint>().lowTwistLimit = limit;
        rootLimit.limit = -80;
        //
        spineTransform.Rotate(Vector3.forward, bendVertical, Space.Self);
        /*Debug.Log(limit.)*/
        //Vector3 moveDirection = (vertical + moveDirection.forward);
        //moveDirection.Normalize();

        //targetRotation = Quaternion.LookRotation(Vector3.forward).eulerAngles;
        //spineTransform.GetComponent<Rigidbody>().rotation = Quaternion.Slerp(spineTransform.GetComponent<Rigidbody>().rotation,
        //    Quaternion.Euler(targetRotation), Time.fixedDeltaTime);
        //spineTransform.GetComponent<CharacterJoint>().lowTwistLimit = rootLimit;
        //}

    }

    private void Aim()
    {
        mouseY += Input.GetAxisRaw("Mouse Y");
        mouseX -= Input.GetAxisRaw("Mouse X");
        mouseY = Mathf.Clamp(mouseY, -35, 18);
        //mouseX = Mathf.Clamp(mouseX, 0, 0);

        aimController.rotation = Quaternion.Euler(-mouseY, -mouseX, 0);
        
    }

    /// <summary>
    /// 
    /// /Johan (& Jonathan)
    /// </summary>
    private void Move()
    {
        Vector3 forward = mainCameraTransform.forward;
        forward.y = 0;
        forward = forward.normalized;
        Vector3 right = new Vector3(forward.z, 0, -forward.x);

        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        //horizontal -= Input.GetAxis("Mouse X");
        ////vertical += Input.GetAxis("Mouse Y");
        //vertical = Mathf.Clamp(vertical, -90, 5);
      
        Vector3 moveDirection = (horizontal * right + vertical * forward);
        moveDirection.Normalize();
        if(moveDirection != Vector3.zero)
        {
            targetRotation = Quaternion.LookRotation(moveDirection).eulerAngles;
            rigidbody.rotation = Quaternion.Slerp(rigidbody.rotation, Quaternion.Euler(targetRotation.x, targetRotation.y, targetRotation.z), Time.fixedDeltaTime * rotationSpeed);

        }

        //targetRotation = Quaternion.LookRotation(moveDirection).eulerAngles;
        //rigidbody.rotation = Quaternion.Slerp(rigidbody.rotation, Quaternion.Euler(targetRotation.x, targetRotation.y, targetRotation.z), Time.fixedDeltaTime * rotationSpeed);


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


    void OnCollisionEnter(Collision other) //Johan
	{
		if (other.relativeVelocity.magnitude > outValue)
		{
			StartCoroutine(Out());
		}
		if (other.gameObject.CompareTag("Terrain"))
		{
			isGround = true;
		}
	}

	IEnumerator Out() //Johan
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
