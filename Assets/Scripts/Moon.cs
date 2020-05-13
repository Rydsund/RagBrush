using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 
/// /Johan
/// </summary>
public class Moon : MonoBehaviour
{
    private Light moonShine;

    [SerializeField]
    private float nightTime = 1;

    private float moonBreakPoint = -150;

    [SerializeField]
    private float maxIntensity = 0.3f, minIntensity = 0.1f, fadeSpeed = 0.09f;

    private void Start()
    {
        moonShine = GetComponent<Light>();
    }

    void Update()
    {
        transform.RotateAround(Vector3.zero, Vector3.right, nightTime * Time.deltaTime);
        transform.LookAt(Vector3.zero);

        if (transform.position.y > moonBreakPoint && moonShine.intensity < maxIntensity)
        {
            moonShine.intensity += fadeSpeed;
        }
        else if(transform.position.y < moonBreakPoint && moonShine.intensity > minIntensity)
        {
            moonShine.intensity -= fadeSpeed;
        }
    }
}
