using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 
/// /Johan
/// </summary>
public class Sun : MonoBehaviour
{
    private Light sunShine;

    [SerializeField]
    private float dayTime = 1;

    private float sunBreakPoint = -150;

    [SerializeField]
    private float maxIntensity = 1f, minIntensity = 0.1f, fadeSpeed = 0.08f;

    private void Start()
    {
        sunShine = GetComponent<Light>();
    }

    /// <summary>
    /// Handles a loop that rotates the light source and changes intensity based on y pos
    /// Johan
    /// </summary>
    void Update()
    {
        transform.RotateAround(Vector3.zero, Vector3.right, dayTime * Time.deltaTime);
        transform.LookAt(Vector3.zero);

        if (transform.position.y > sunBreakPoint && sunShine.intensity < maxIntensity)
        {
            sunShine.intensity += fadeSpeed;
        }
        else if (transform.position.y < sunBreakPoint && sunShine.intensity > minIntensity)
        {
            sunShine.intensity -= fadeSpeed;
        }
    }
}
