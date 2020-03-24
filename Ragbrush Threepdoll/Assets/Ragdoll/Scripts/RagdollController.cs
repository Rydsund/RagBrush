using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollController : MonoBehaviour
{
	public Animator animator = null;

	private Transform mainCameraTransform = null;


	Rigidbody rb;
	CapsuleCollider cap;

	float vertical;
	float horizontal;

	float verticalRaw;
	float horizontalRaw;

	Vector3 targetRotation;


	[SerializeField]
	private float lookSensitivity = 3f;

	[SerializeField]
	Camera cam;

	private Vector3 velocity = Vector3.zero;
	private Vector3 rotation = Vector3.zero;
	private Vector3 currentRotation = Vector3.zero;
	
	private float cameraRotationX = 0f;
	private float currentCameraRotationX = 0f;
	private Vector3 thrusterForce = Vector3.zero;
	[SerializeField]
	private float cameraRotationLimit = 85f;

	public float rotationSpeed = 0.1f;
	public float speed = 10;

	public float jumpForce = 3;

	bool isGround = true;
	public float outValue = 10;

	void Start()
    {
		rb = GetComponent<Rigidbody>();
		cap = GetComponent<CapsuleCollider>();
		

		mainCameraTransform = Camera.main.transform;

	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space) && isGround)
		{
			rb.AddForce(new Vector3(0,jumpForce * 100,0), ForceMode.Impulse);
			isGround = false;
		}

		//float xRot = Input.GetAxisRaw("Mouse X");
		//rotation = new Vector3(-xRot, 0f, 0f) * lookSensitivity;
		//float xRot = Input.GetAxisRaw("Mouse X");
		//rotation = new Vector3(-xRot, 0f, 0f) * lookSensitivity;
		//Apply rotation
		//Rotation(rotation);
	}

	void FixedUpdate()
	{

		float xRot = Input.GetAxisRaw("Mouse X");
		rotation = new Vector3(-xRot, 0f, 0f) * lookSensitivity;
		currentRotation -= rotation;

		rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));
		rb.rotation = Quaternion.Slerp(rb.rotation, Quaternion.Euler(currentRotation.y, Mathf.Round(currentRotation.x / 45) * 45, currentRotation.z - 90),
		Time.fixedDeltaTime * rotationSpeed);


		//horizontal = Input.GetAxis("Horizontal");
		//vertical = Input.GetAxis("Vertical");

		horizontalRaw = Input.GetAxisRaw("Horizontal");
		verticalRaw = Input.GetAxisRaw("Vertical");

		//Vector3 forward = mainCameraTransform.forward;
		//Vector3 right = mainCameraTransform.right;

		//forward.Normalize();
		//right.Normalize();



		//Vector3 input = new Vector3(horizontal, 0, vertical);
		Vector3 inputRaw = new Vector3(horizontalRaw, 0, verticalRaw);

		//Vector3 moveDirection = (forward * vertical + right * horizontal).normalized;


		//if (input.sqrMagnitude > 1f)
		//{
		//	input.Normalize();
		//}

		//if (inputRaw != Vector3.zero)
		//{
		//	targetRotation = Quaternion.LookRotation(moveDirection).eulerAngles;
		//}

		////rb.rotation = Quaternion.Slerp(rb.rotation, Quaternion.Euler(targetRotation.x, Mathf.Round(targetRotation.y / 45) * 45, targetRotation.z - 90),
		////Time.fixedDeltaTime * rotationSpeed);

		////rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));

		//rb.rotation = Quaternion.Slerp(rb.rotation, Quaternion.Euler(currentRotation.y, Mathf.Round(currentRotation.x / 45) * 45, currentRotation.z - 90),
		//Time.fixedDeltaTime * rotationSpeed);


		if (inputRaw.sqrMagnitude != 0)
		{
			animator.enabled = true;

		}
		else if (inputRaw.sqrMagnitude == 0)
		{
			animator.enabled = false;
		}
		//Move();

	}


	//private void Move()
	//{
	//	Vector2 movementInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

	//	Vector3 forward = mainCameraTransform.forward;
	//	Vector3 right = mainCameraTransform.right;

	//	forward.Normalize();
	//	right.Normalize();

	//	Vector3 moveDirection = (forward * movementInput.y + right * movementInput.x).normalized;

	//	if (moveDirection != Vector3.zero)
	//	{
	//		rb.rotation = Quaternion.Slerp(rb.rotation, Quaternion/*.Euler*/.LookRotation(moveDirection), Time.deltaTime * rotationSpeed);
			
	//		animator.enabled = true;
	//	}
	//	else
	//	{
	//		animator.enabled = false;
	//	}

	//	//if (movementInput.sqrMagnitude != 0)
	//	//{
	//	//	animator.enabled = true;

	//	//}
	//	//else if (movementInput.sqrMagnitude == 0)
	//	//{
	//	//	animator.enabled = false;
	//	//}

	//}

	void OnCollisionEnter(Collision other)
	{
		if(other.relativeVelocity.magnitude > outValue)
		{
			StartCoroutine(Out());
		}
		if (other.gameObject.CompareTag("Terrain"))
		{
			isGround = true;
		}
	}

	IEnumerator Out()
	{
		rb.constraints = RigidbodyConstraints.None;
		cap.enabled = false;
		yield return new WaitForSeconds(4);
		rb.constraints = RigidbodyConstraints.FreezeRotation;
		cap.enabled = true;
	}

	
}
