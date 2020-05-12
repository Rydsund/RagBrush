using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grab : MonoBehaviour //Johan
{
	new Rigidbody rigidbody;
	public KeyCode grabInput;
	public KeyCode grabInput2;
	public GameObject myGrabdObj;
	public bool isGrab = false;

	/// <summary>
	/// 
	/// /Johan
	/// </summary>
	void Start()
    {
		rigidbody = GetComponent<Rigidbody>();
    }

	/// <summary>
	/// 
	/// /Johan
	/// </summary>
	void Update()
    {
		if (myGrabdObj != null)
		{
			if (Input.GetKey(grabInput) || Input.GetKey(grabInput2))
			{
				if (!isGrab)
				{
					FixedJoint fixedJoint = myGrabdObj.AddComponent<FixedJoint>();
					fixedJoint.connectedBody = rigidbody;
					fixedJoint.breakForce = 8000;
					isGrab = true;
				}
			}
			else if (!Input.GetKey(grabInput) || !Input.GetKey(grabInput2))
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

	//public void OnTriggerExit(Collider other)//Johan
	//{
	//	if (other.gameObject.CompareTag("Item"))
	//	{
	//		myGrabdObj = null;
	//	}
	//}
}
