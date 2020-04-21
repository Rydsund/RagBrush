using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colouring : MonoBehaviour
{
    // Write black pixels onto the GameObject that is located
    // by the script. The script is attached to the camera.
    // Determine where the collider hits and modify the texture at that point.
    //
    // Note that the MeshCollider on the GameObject must have Convex turned off. This allows
    // concave GameObjects to be included in collision in this example.
    //
    // Also to allow the texture to be updated by mouse button clicks it must have the Read/Write
    // Enabled option set to true in its Advanced import settings.
    public Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {


        if (!Input.GetMouseButton(0))
            return;

        RaycastHit hit;
        if (!Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hit))
            return;

        Renderer rend = hit.transform.GetComponent<Renderer>();
        MeshCollider meshCollider = hit.collider as MeshCollider;

        if (rend == null || rend.material == null || rend.material.mainTexture == null || meshCollider == null)
            return;

        Texture2D tex = rend.material.mainTexture as Texture2D;
        Vector2 pixelUV = hit.textureCoord;
        pixelUV.x *= tex.width;
        pixelUV.y *= tex.height;

        tex.SetPixel(10, 10, Color.black);
        tex.Apply();
    }
}
