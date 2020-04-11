using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollController : MonoBehaviour //Johan
{
	public Animator animator = null;
	private Transform mainCameraTransform = null;

	Rigidbody rb;
	CapsuleCollider cap;
	

	float vertical;
	float horizontal;

	Vector3 targetRotation;

	public float rotationSpeed = 10f;
	public float speed = 3f;
	public float jumpForce = 3;
	bool isGround = true;
	public float outValue = 10;

	bool alive = true;

	void Start()
    {
		rb = GetComponent<Rigidbody>(); //Johan
		cap = GetComponent<CapsuleCollider>(); //Johan
		mainCameraTransform = Camera.main.transform; //Johan
	}

	void Update()
	{
		
		if (Input.GetButtonDown("Jump") && isGround)//Johan
		{
			rb.AddForce(new Vector3(0,jumpForce * 100,0), ForceMode.Impulse);
			isGround = false;
		}
	}

	void FixedUpdate()
	{
		if (alive)//Johan
		{
			horizontal = Input.GetAxis("Horizontal");
			vertical = Input.GetAxis("Vertical");
			Vector3 input = new Vector3(horizontal, 0, vertical);

			if (input != Vector3.zero)
			{
				Vector3 forward = mainCameraTransform.forward;
				forward.y = 0;
				forward = forward.normalized;
				Vector3 right = new Vector3(forward.z, 0, -forward.x);

				Vector3 moveDirection = (horizontal * right + vertical * forward);
				moveDirection.Normalize();

				targetRotation = Quaternion.LookRotation(moveDirection).eulerAngles;
				rb.rotation = Quaternion.Slerp(rb.rotation, Quaternion.Euler(targetRotation.x, targetRotation.y, targetRotation.z), Time.fixedDeltaTime * rotationSpeed);

				animator.enabled = true;

				Vector3 velocity = moveDirection * speed;
				rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
			}
			else if (input.sqrMagnitude == 0)
			{
				animator.enabled = false;
			}
		}
		else
		{
			animator.enabled = false;
		}
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
		rb.constraints = RigidbodyConstraints.None;
		cap.enabled = false;
		alive = false;
		yield return new WaitForSeconds(4);
		rb.constraints = RigidbodyConstraints.FreezeRotation;
		cap.enabled = true;
		alive = true;
	}
}
