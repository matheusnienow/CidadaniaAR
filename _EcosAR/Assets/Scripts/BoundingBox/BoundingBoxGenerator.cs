using UnityEngine;

public class BoundingBoxGenerator : MonoBehaviour
{
    public Vector3[] Vertices;

    private Vector3 v3FrontTopLeft;
    private Vector3 v3FrontTopRight;
    private Vector3 v3FrontBottomLeft;
    private Vector3 v3FrontBottomRight;
    private Vector3 v3BackTopLeft;
    private Vector3 v3BackTopRight;
    private Vector3 v3BackBottomLeft;
    private Vector3 v3BackBottomRight;


    private void Start()
    {
        Vertices = CalcPositons();
    }

    void Update()
    {
        Vertices = CalcPositons();
    }

    Vector3[] CalcPositons()
    {
        Vector3[] points = new Vector3[8];

        Bounds bounds = GetComponent<MeshFilter>().mesh.bounds;

        Vector3 v3Center = bounds.center;
        Vector3 v3Extents = bounds.extents;

        v3FrontTopLeft = new Vector3(v3Center.x - v3Extents.x, v3Center.y + v3Extents.y, v3Center.z - v3Extents.z);  // Front top left corner
        v3FrontTopRight = new Vector3(v3Center.x + v3Extents.x, v3Center.y + v3Extents.y, v3Center.z - v3Extents.z);  // Front top right corner
        v3FrontBottomLeft = new Vector3(v3Center.x - v3Extents.x, v3Center.y - v3Extents.y, v3Center.z - v3Extents.z);  // Front bottom left corner
        v3FrontBottomRight = new Vector3(v3Center.x + v3Extents.x, v3Center.y - v3Extents.y, v3Center.z - v3Extents.z);  // Front bottom right corner
        v3BackTopLeft = new Vector3(v3Center.x - v3Extents.x, v3Center.y + v3Extents.y, v3Center.z + v3Extents.z);  // Back top left corner
        v3BackTopRight = new Vector3(v3Center.x + v3Extents.x, v3Center.y + v3Extents.y, v3Center.z + v3Extents.z);  // Back top right corner
        v3BackBottomLeft = new Vector3(v3Center.x - v3Extents.x, v3Center.y - v3Extents.y, v3Center.z + v3Extents.z);  // Back bottom left corner
        v3BackBottomRight = new Vector3(v3Center.x + v3Extents.x, v3Center.y - v3Extents.y, v3Center.z + v3Extents.z);  // Back bottom right corner

        v3FrontTopLeft = transform.TransformPoint(v3FrontTopLeft);
        points[0] = v3FrontTopLeft;
        v3FrontTopRight = transform.TransformPoint(v3FrontTopRight);
        points[1] = v3FrontTopRight;
        v3BackTopRight = transform.TransformPoint(v3BackTopRight);
        points[2] = v3BackTopRight;
        v3BackTopLeft = transform.TransformPoint(v3BackTopLeft);
        points[3] = v3BackTopLeft;
        v3FrontBottomLeft = transform.TransformPoint(v3FrontBottomLeft);
        points[4] = v3FrontBottomLeft;
        v3FrontBottomRight = transform.TransformPoint(v3FrontBottomRight);
        points[5] = v3FrontBottomRight;
        v3BackBottomRight = transform.TransformPoint(v3BackBottomRight);
        points[6] = v3BackBottomRight;
        v3BackBottomLeft = transform.TransformPoint(v3BackBottomLeft);
        points[7] = v3BackBottomLeft;

        return points;
    }
}