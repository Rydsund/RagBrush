using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintBrush : MonoBehaviour
{
    public Color paintColor;

    public float innerRadius;
    public float outerRadius;

    /// <summary>
    /// Sätter a komponenten till 1 så att det fungerar när det målar
    /// </summary>
    private void Start()
    {
        paintColor.a = 1;
    }
}
