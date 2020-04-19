using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grab : MonoBehaviour //Johan
{
	new Rigidbody rigidbody;
	public KeyCode punchInput1;
	public KeyCode punchInput2;
	public GameObject myGrabdObj;
	//public GameObject parentObject, grandparentObject;
	public bool isGrab = false;
	public float punchSpeed = 1;
	public Transform target/*, parentTarget, grandparentTarget*/;
	

	void Start()
    {
		rigidbody = GetComponent<Rigidbody>();//Johan
    }

    void Update()
    {

		if(Input.GetKey(punchInput1) || Input.GetKey(punchInput2))
		{

			//float step = punchSpeed * Time.deltaTime;
			//transform.position = Vector3.MoveTowards(transform.position, target.transform.position, step);
			//parentObject.transform.position = Vector3.MoveTowards(transform.position, parentTarget.transform.position, step);
			//grandparentObject.transform.position = Vector3.MoveTowards(transform.position, grandparentTarget.transform.position, step);

			//grandparentObject.transform.rotation = Vector3.RotateTowards(grandparentObject.transform.rotation, new Vector3(0, -90, -90), step, step + 1f);
			//grandparentObject.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, 75), Time.deltaTime);

		}
		if (myGrabdObj != null)//Johan
		{
			if (Input.GetKey(punchInput1) || Input.GetKey(punchInput2))
			{
				if (!isGrab)
				{
					FixedJoint FJ = myGrabdObj.AddComponent<FixedJoint>();
					FJ.connectedBody = rigidbody;
					FJ.breakForce = 8000;
					isGrab = true;
				}
			}
			else if (Input.GetKeyUp(punchInput1) || Input.GetKeyUp(punchInput2))
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
