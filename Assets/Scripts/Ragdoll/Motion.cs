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

	/// <summary>
	/// Handles leg animation. 
	/// Johan
	/// </summary>
    void Update()
    {
		if (legAnimation != null)
		{
			JointSpring jointSpring = hingeJoint.spring;
			jointSpring.targetPosition = legAnimation.localEulerAngles.x;
			
			if (jointSpring.targetPosition > 180)
			{
				jointSpring.targetPosition = jointSpring.targetPosition - 360;
			}
			
			if (mirror)
			{
				jointSpring.targetPosition = jointSpring.targetPosition *= -1;
			}
			jointSpring.targetPosition = Mathf.Clamp(jointSpring.targetPosition, hingeJoint.limits.min + 5, hingeJoint.limits.max - 5);
			hingeJoint.spring = jointSpring;
        }
    }
}
