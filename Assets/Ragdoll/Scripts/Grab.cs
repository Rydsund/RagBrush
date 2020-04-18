using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grab : MonoBehaviour //Johan
{
	new Rigidbody rigidbody;
	public KeyCode leftInput;
	public KeyCode rightInput;
	public GameObject myGrabdObj;
	public bool isGrab = false;
	public float punchSpeed = 1;
	public Transform leftTarget, rightTarget;

	void Start()
    {
		rigidbody = GetComponent<Rigidbody>();//Johan
    }

    void Update()
    {

		if(Input.GetKey(leftInput))
		{
			float step = punchSpeed * Time.deltaTime;
			transform.position = Vector3.MoveTowards(transform.position, leftTarget.transform.position, step);
		}
		if (Input.GetKey(rightInput))
		{
			float step = punchSpeed * Time.deltaTime;
			transform.position = Vector3.MoveTowards(transform.position, rightTarget.transform.position, step);
		}
		if (myGrabdObj != null)//Johan
		{
			if (Input.GetKey(leftInput) || Input.GetKey(rightInput))
			{
				if (!isGrab)
				{
					FixedJoint FJ = myGrabdObj.AddComponent<FixedJoint>();
					FJ.connectedBody = rigidbody;
					FJ.breakForce = 8000;
					isGrab = true;
				}
			}
			else if (Input.GetKeyUp(leftInput) || Input.GetKeyUp(rightInput))
			{
				if (myGrabdObj.CompareTag("Item"))
				{
					Destroy(myGrabdObj.GetComponent<Joint>());
				}
				myGrabdObj = null;
				isGrab = false;
			}
		}
	}

	public void OnTriggerEnter(Collider other)//Johan
	{
		if (other.gameObject.CompareTag("Item"))
		{
			myGrabdObj = other.gameObject;
		}
	}

	public void OnTriggerExit(Collider other)//Johan
	{
		if (other.gameObject.CompareTag("Item"))
		{
			myGrabdObj = null;
		}
	}
}
