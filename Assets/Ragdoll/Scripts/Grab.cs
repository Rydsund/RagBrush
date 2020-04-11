using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grab : MonoBehaviour //Johan
{
	Rigidbody rb;
	public KeyCode grabInput;
	public KeyCode grabInput2;
	public GameObject myGrabdObj;
	public bool isGrab = false;

    void Start()
    {
		rb = GetComponent<Rigidbody>();//Johan
    }

    void Update()
    {
		if (myGrabdObj != null)//Johan
		{
			if (Input.GetKey(grabInput) || Input.GetKey(grabInput2))
			{
				if (!isGrab)
				{
					FixedJoint FJ = myGrabdObj.AddComponent<FixedJoint>();
					FJ.connectedBody = rb;
					FJ.breakForce = 8000;
					isGrab = true;
				}
			}
			else if (Input.GetKeyUp(grabInput) || Input.GetKeyUp(grabInput2))
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
