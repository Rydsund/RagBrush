using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Motion : MonoBehaviour
{
	private HingeJoint Hj;
	public Transform legAnimation;
	public bool Mirror;
    void Start()
    {
		Hj = GetComponent<HingeJoint>();
    }


    void Update()
    {
		if (legAnimation != null)
		{
			JointSpring js = Hj.spring;
			js.targetPosition = legAnimation.localEulerAngles.x;
			if (js.targetPosition > 180)
			{
				js.targetPosition = js.targetPosition - 360;
			}
			js.targetPosition = Mathf.Clamp(js.targetPosition, Hj.limits.min + 5, Hj.limits.max - 5);
			if (Mirror)
			{
				js.targetPosition = js.targetPosition *= -1;
			}
            //js.targetPosition = Mathf.Clamp(js.targetPosition, Hj.limits.min + 5, Hj.limits.max - 5);
            Hj.spring = js;
            
        }
    }
}
