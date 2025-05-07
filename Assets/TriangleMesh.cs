using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class TriangleMesh : MonoBehaviour
{
    void Start()
    {
        Mesh mesh = new Mesh();

        Vector3[] verts =
        {
            new (0, 0, 0),
            new (0, 1, 0),
            new (1, 0, 0)
        };
        int[] tris = { 0, 1, 2 };

        mesh.vertices = verts;
        mesh.triangles = tris;

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        GetComponent<MeshFilter>().mesh = mesh;
    }
}