using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabViktor : MonoBehaviour //Johan
{
	new Rigidbody rigidbody;
	public KeyCode grabInput;
	public KeyCode grabInput2;
	public GameObject myGrabdObj;
	public bool isGrab = false;
	Player playerInventory;

    void Start()
    {
		rigidbody = GetComponent<Rigidbody>();//Johan
		playerInventory = GetComponentInParent<Player>();
    }

    void Update()
    {
		if (myGrabdObj != null)//Johan
		{
			// Hold item
			if (Input.GetKey(grabInput) || Input.GetKey(grabInput2))
			{
				if (!isGrab)
				{
					FixedJoint FJ = myGrabdObj.AddComponent<FixedJoint>();
					FJ.connectedBody = rigidbody;
					FJ.breakForce = 8000;
					isGrab = true;

			
				}
				// Put items to inventory.
				if (isGrab)
				{
					if (Input.GetKeyDown(KeyCode.G))
					{
						playerInventory.AddItemToInventory(myGrabdObj.GetComponent<Collider>());
					}
				}
			}
			// Drop item
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

		//if (isGrab)
		//{
		//	//myGrabdObj.GetComponent<Collider>().isTrigger = true;
		//	playerInventory.AddItemToInventory(myGrabdObj.GetComponent<Collider>());
		//}
	}

	public void OnTriggerExit(Collider other)//Johan
	{
		if (other.gameObject.CompareTag("Item"))
		{
			myGrabdObj = null;
		}
	}
}
