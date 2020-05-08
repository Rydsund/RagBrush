using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Motion : MonoBehaviour
{
	private new HingeJoint hingeJoint;
    
    [SerializeField]
	private Transform legAnimation;

    [SerializeField]
	private bool mirror;

    void Start()
    {
		hingeJoint = GetComponent<HingeJoint>();
    }


    void Update()
    {
		if (legAnimation != null)
		{
			JointSpring jointSpring = hingeJoint.spring;
			jointSpring.targetPosition = legAnimation.localEulerAngles.x;
			if (jointSpring.targetPosition > 180)//Johan
			{
				jointSpring.targetPosition = jointSpring.targetPosition - 360;
			}
			//js.targetPosition = Mathf.Clamp(js.targetPosition, Hj.limits.min + 5, Hj.limits.max - 5);
			if (mirror)//Johan
			{
				jointSpring.targetPosition = jointSpring.targetPosition *= -1;
			}
			jointSpring.targetPosition = Mathf.Clamp(jointSpring.targetPosition, hingeJoint.limits.min + 5, hingeJoint.limits.max - 5);
			hingeJoint.spring = jointSpring;
        }
    }
}
