using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class Paintable : MonoBehaviour
{
    Mesh mesh;
    Color32[] verticeColors;
    Vector3[] vertices;

    [SerializeField]
    GameObject paintBrush;

    /// <summary>
    /// Grabs components, sets a Color array to the lenght of the amount of vertices. 
    /// </summary>
    void Start()
    {
        Material material = GetComponent<Material>();
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        vertices = meshFilter.mesh.vertices;
        mesh = meshFilter.mesh;
        verticeColors = new Color32[vertices.Length];


        ApplyPaint(transform.position, 0, 0, new Color(0, 0, 0, 0));
    }

    /// <summary>
    /// When collision happens, it grabs the PaintBrush component of the gameobject that collided with it. If it has the component, it grabs the point of contact and paints using the PaintBrush color on that point.
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        PaintBrush brush = collision.gameObject.GetComponent<PaintBrush>();

        if(brush != null)
        {
            ContactPoint contactPoint = collision.GetContact(0);
            ApplyPaint(contactPoint.point, 0.1f, 1f, brush.paintColor);
        }

    }

    /// <summary>
    /// Applies paint to the game object. 
    /// </summary>
    /// <param name="position"></param>
    /// <param name="innerRadius"></param>
    /// <param name="outerRadius"></param>
    /// <param name="color"></param>
    private void ApplyPaint(Vector3 position, float innerRadius, float outerRadius, Color color)
    {
        // Center of the collision
        Vector3 center = transform.InverseTransformPoint(position);

        // Outer radius of the collision
        float outerR = transform.InverseTransformVector(outerRadius * Vector3.right).magnitude;

        // Inner Radius
        float innerR = innerRadius * outerR / outerRadius;

        // Inner Radius Squared
        float innerRsqr = innerR * innerR;

        // Outer Radius Squared
        float outerRsqr = outerR * outerR;
        float tFactor = 1f / (outerR - innerR);

        CalculateColorOnVertices(center, innerR, tFactor, innerRsqr, outerRsqr, color);

        mesh.colors32 = verticeColors;
    }

    private void CalculateColorOnVertices(Vector3 center, float innerR, float tFactor, float innerRsqr, float outerRsqr, Color color)
    {
        for (int i = 0; i < vertices.Length; i++)
        {
            // Distance to vertice
            Vector3 delta = vertices[i] - center;
            float dsqr = delta.sqrMagnitude;

            // If distance is greater than outer radius, go to next vertice
            if (dsqr > outerRsqr)
            {
                continue;
            }

            int a = verticeColors[i].a;
            verticeColors[i] = color;
            // If distance is less than inner radius, set alpha to 255. 
            // Else base it on distance, tfactor and inner radius
            if (dsqr < innerRsqr)
            {
                verticeColors[i].a = 255;
            }
            else
            {
                float d = Mathf.Sqrt(dsqr);
                byte blobA = (byte)(255 - 255 * (d - innerR) * tFactor);
                if (blobA >= a) verticeColors[i].a = blobA;
            }
        }
    }
}
