using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintBrush : MonoBehaviour
{
    public Color paintColor;

    public float innerRadius;
    public float outerRadius;

    /// <summary>
    /// Sets the a component of the paint to 1 so that it doesn't vary when being used to paint. 
    /// </summary>
    private void Start()
    {
        paintColor.a = 1;
    }
}
