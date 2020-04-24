using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class Paintable : MonoBehaviour
{
    MeshCollider meshCollider;
    Mesh mesh;
    Color32[] vertColors;
    Vector3[] verts;
    Shader shader;
    void Start()
    {
        shader = GetComponent<Shader>();
        meshCollider = GetComponent<MeshCollider>();
        mesh = meshCollider.sharedMesh;
        verts = mesh.vertices;
        vertColors = mesh.colors32;       
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            ApplyPaint(transform.position, 0.1f, 1, Color.blue);
        }
    }

    /// <summary>
    /// Applies paint to the mesh. 
    /// </summary>
    /// <param name="position"></param>
    /// <param name="innerRadius"></param>
    /// <param name="outerRadius"></param>
    /// <param name="color"></param>
    public void ApplyPaint(Vector3 position, float innerRadius, float outerRadius, Color color)
    {
        Vector3 center = transform.InverseTransformPoint(position);
        float outerR = transform.InverseTransformVector(outerRadius * Vector3.right).magnitude;
        float innerR = innerRadius * outerR / outerRadius;
        float innerRsqr = innerR * innerR;
        float outerRsqr = outerR * outerR;
        float tFactor = 1f / (outerR - innerR);

        for (int i = 0; i < verts.Length; i++)
        {
            Vector3 delta = verts[i] - center;
            float dsqr = delta.sqrMagnitude;
            if (dsqr > outerRsqr)
            {
                continue;
            }

            int a = vertColors[i].a;
            vertColors[i] = color;
            if (dsqr < innerRsqr)
            {
                vertColors[i].a = 255;
            }
            else
            {
                float d = Mathf.Sqrt(dsqr);
                byte blobA = (byte)(255 - 255 * (d - innerR) * tFactor);
                if (blobA >= a) vertColors[i].a = blobA;
            }
        }

        mesh.colors32 = vertColors;
    }
}
