﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RC : MonoBehaviour
{
	public Animator animator = null;
	private Transform mainCameraTransform = null;

	new Rigidbody rigidbody;
	CapsuleCollider capsuleCollider;

	public Transform LookTarget;
	public Transform[] FootTarget;
	public Transform[] HandTarget;
	//public Transform[] HandPole;


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
		rigidbody = GetComponent<Rigidbody>(); //Johan
		capsuleCollider = GetComponent<CapsuleCollider>(); //Johan
		mainCameraTransform = Camera.main.transform; //Johan
	}

	void Update()
	{

		if (Input.GetButtonDown("Jump") && isGround)//Johan
		{
			rigidbody.AddForce(new Vector3(0, jumpForce * 100, 0), ForceMode.Impulse);
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
				//footsteps
				//for (int i = 0; i < FootTarget.Length; i++)
				//{
				//	var foot = FootTarget[i];
				//	var ray = new Ray(foot.transform.position + Vector3.up * 0.5f, Vector3.down);
				//	var hitInfo = new RaycastHit();
				//	if (Physics.SphereCast(ray, 0.05f, out hitInfo, 0.50f) && hitInfo.transform.tag != "Ragdoll")
				//		foot.position = hitInfo.point + Vector3.up * 0.05f;
				//}
				//for (int i = 0; i < HandTarget.Length; i++)
				//{
				//	//HandTarget[i].rotation = Quaternion.Lerp(Quaternion.Euler(90, 0, 0), HandTarget[i].rotation, , normDist);
				//}


				Vector3 forward = mainCameraTransform.forward;
				forward.y = 0;
				forward = forward.normalized;
				Vector3 right = new Vector3(forward.z, 0, -forward.x);

				Vector3 moveDirection = (horizontal * right + vertical * forward);
				moveDirection.Normalize();

				targetRotation = Quaternion.LookRotation(moveDirection).eulerAngles;
				rigidbody.rotation = Quaternion.Slerp(rigidbody.rotation, Quaternion.Euler(targetRotation.x, targetRotation.y, targetRotation.z), Time.fixedDeltaTime * rotationSpeed);

				animator.enabled = true;

				Vector3 velocity = moveDirection * speed;
				rigidbody.MovePosition(rigidbody.position + velocity * Time.fixedDeltaTime);
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
		rigidbody.constraints = RigidbodyConstraints.None;
		capsuleCollider.enabled = false;
		alive = false;
		yield return new WaitForSeconds(4);
		rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
		capsuleCollider.enabled = true;
		alive = true;
	}
}