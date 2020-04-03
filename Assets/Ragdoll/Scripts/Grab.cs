using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grab : MonoBehaviour
{
	Rigidbody rb;
	public KeyCode grabInput;
	public GameObject myGrabdObj;
	public bool isGrab = false;

    void Start()//NY
    {
		rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
		if (myGrabdObj != null)
		{
			if (Input.GetKey(grabInput))
			{
				if (!isGrab)
				{
					FixedJoint FJ = myGrabdObj.AddComponent<FixedJoint>();
					FJ.connectedBody = rb;
					FJ.breakForce = 8000;
					isGrab = true;
				}
			}
			else if (Input.GetKeyUp(grabInput))
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

	public void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Item"))
		{
			myGrabdObj = other.gameObject;
		}
	}

	public void OnTriggerExit(Collider other)
	{
		if (other.gameObject.CompareTag("Item"))
		{
			myGrabdObj = null;
		}
	}
}
