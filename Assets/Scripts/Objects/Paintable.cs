using UnityEngine;

/// <summary>
/// Makes a gameobject paintable
/// Mattias Smedman
/// </summary>
public class Paintable : MonoBehaviour
{
    Mesh mesh;
    Color32[] verticeColors;
    Vector3[] vertices;

    /// <summary>
    /// Grabs components, sets a Color array to the lenght of the amount of vertices. Calls apply paint with an invisible colour to make the object draw it's texture
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
            ApplyPaint(contactPoint.point, brush.innerRadius, brush.outerRadius, brush.paintColor);
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
        Vector3 center = transform.InverseTransformPoint(position);

        float outerR = transform.InverseTransformVector(outerRadius * Vector3.right).magnitude;
        float outerRsqr = outerR * outerR;
        float innerR = innerRadius * outerR / outerRadius;
        float innerRsqr = innerR * innerR;
        float tFactor = 1f / (outerR - innerR);

        CalculateColorOnVertices(center, innerR, tFactor, innerRsqr, outerRsqr, color);

        mesh.colors32 = verticeColors;
    }

    /// <summary>
    /// Calculates how the colour should look based on distance from impact and vertices
    /// </summary>
    /// <param name="center"></param>
    /// <param name="innerR"></param>
    /// <param name="tFactor"></param>
    /// <param name="innerRsqr"></param>
    /// <param name="outerRsqr"></param>
    /// <param name="color"></param>
    private void CalculateColorOnVertices(Vector3 center, float innerR, float tFactor, float innerRsqr, float outerRsqr, Color color)
    {
        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 delta = vertices[i] - center;
            float dsqr = delta.sqrMagnitude;
            
            if (dsqr > outerRsqr)
            {
                continue;
            }
            int a = verticeColors[i].a;
            verticeColors[i] = color;

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
