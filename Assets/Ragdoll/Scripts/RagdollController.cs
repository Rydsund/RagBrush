using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollController : MonoBehaviour
{
	public Animator animator = null;
	private Transform mainCameraTransform = null;

	Rigidbody rb;
	CapsuleCollider cap;

	float verticalRaw, v;
	float horizontalRaw, h;

	Vector3 targetRotation;

	public float rotationSpeed = 10f;
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
	}

	void FixedUpdate()
	{
		horizontalRaw = Input.GetAxisRaw("Horizontal");
		verticalRaw = Input.GetAxisRaw("Vertical");
		Vector3 inputRaw = new Vector3(horizontalRaw, 0, verticalRaw);

		Vector3 forward = mainCameraTransform.forward;
		forward.y = 0;
		forward = forward.normalized;
		Vector3 right = new Vector3(forward.z, 0, -forward.x);

		h = Input.GetAxis("Horizontal");
		v = Input.GetAxis("Vertical");
		Vector3 moveDirection = (h * right + v * forward);

		moveDirection.Normalize();

		if (inputRaw != Vector3.zero)
		{
			targetRotation = Quaternion.LookRotation(moveDirection).eulerAngles;

			rb.rotation = Quaternion.Slerp(rb.rotation, Quaternion.Euler(targetRotation.x, targetRotation.y - 90/*Mathf.Round(targetRotation.y / 45) * 45 - 90*/, targetRotation.z - 90),
			Time.fixedDeltaTime * rotationSpeed);

			animator.enabled = true;
		}
		else if (inputRaw.sqrMagnitude == 0)
		{
			animator.enabled = false;
		}
	}


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
