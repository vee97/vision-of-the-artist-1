using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]

public class GraphicsLineRenderer : MonoBehaviour
{
    // lmat for line material
    public Material lmat;

    private Mesh ml;

    private Vector3 s;

    private float lineSize = .1f;

    // is this the first quad drawn?
    // need two points to know where start and end are
    // is this the first point drawn? then do slight
    // variation to delay until get to second point
    private bool firstQuad = true;

    // Start is called before the first frame update
    void Start()
    {
        // ml getting the mesh from MeshFilter
        // when going back to manager have to change
        // code to add MeshFilter + MeshRenderer to it
        // meshFilter keeps track of mesh
        // GetComponent<MeshRenderer> visualizes mesh for us
        ml = GetComponent<MeshFilter>().mesh;
        // change what mesh looks like from pink to something else
        GetComponent<MeshRenderer>().material = lmat;
    }

    public void SetWidth(float width)
    {
        lineSize = width;
    }

    //public void SetMaterial(Material material)
    //{
    //    material = material;
    //}

    // exactly same as setPosition
    // checks to make sure start isn't at zero - tells us haven't drawn anything
    public void AddPoint(Vector3 point, Vector3 direction)
    //public void AddPoint(Vector3 point)
    {
        // if(s != Vector3.zero)
        // if first quad, going to add a line
        // get mesh, and then quad, set first quad to false b/c added first quad
        // every single time, after drawn line, start point is line we ever added
        if (s != Vector3.zero)
        {
            AddLine(ml, MakeQuad(s, point, lineSize, firstQuad));
            firstQuad = false;
        }
        s = point;
    }

    // index used b/c continuously making line
    // give line, that's the next point in line

    // given those two points, draw the 4 vertices
    // drawing those 4 vertices

    Vector3[] MakeQuad(Vector3 s, Vector3 e, float w, bool all)
    {
        // next, width set to be half
        w = w / 2;

        // Vector3 - list of quads
        Vector3[] q;

        // if we want all 4 vertices, set size of quads to 4, otherwise to 2
        // reason - comes down to first quad you drew
        // next subsequent point you add, only need to add 2 vertices for previous drawn line
        if (all)
            q = new Vector3[4];
        else
            q = new Vector3[2];

        // get normal by getting cross product of start + end vector
        // take cross product out to calculate normal
        // perpendicular to velocity, normal is where line is drawn up and down
        Vector3 n = Vector3.Cross(s, e);
        Vector3 l = Vector3.Cross(n, e - s);
        l.Normalize();

        // if first quad or if subsequent
        // define what those vertex points are
        // define where points are based on line + width we want and return
        // creating the quad
        if (all)
        {
            q[0] = transform.InverseTransformPoint(s + l * w);
            q[1] = transform.InverseTransformPoint(s + l * -w);
            q[2] = transform.InverseTransformPoint(e + l * w);
            q[3] = transform.InverseTransformPoint(e + l * -w);
        }
        else
        {
            q[0] = transform.InverseTransformPoint(e + l * w);
            q[1] = transform.InverseTransformPoint(s + l * -w);
        }
        return q;
    }

    // building up mesh as we go
    // resize array, uv mapping + triangles
    // makes sure start isn't zero - haven't drawn anything

    // duplicate b/c we want it for the front and back
    void AddLine(Mesh m, Vector3[] quad)
    {
        int vl = m.vertices.Length;

        Vector3[] vs = m.vertices;
        vs = resizeVertices(vs, 2 * quad.Length);

        for (int i = 0; i < 2 * quad.Length; i += 2)
        {
            vs[vl + i] = quad[i / 2];
            vs[vl + i + 1] = quad[i / 2];
        }

        // uv mapping
        // if you want to paint a line
        // create an image
        // have basic material
        // for each vertex assigned
        // for each vertex you provide what point of image you want sample from
        // unity interpolates where you want to place those points from image
        // bc drawing a quad - bottom corner is 0,0, bottom right is 0,1
        // top left is 1,0, top right is 1,1.
        // assigning 0,0 , 1,1 values to line we draw
        Vector2[] uvs = m.uv;
        uvs = resizeUVs(uvs, 2 * quad.Length);

        if (quad.Length == 4)
        {
            uvs[vl] = Vector2.zero;
            uvs[vl + 1] = Vector2.zero;
            uvs[vl + 2] = Vector2.right;
            uvs[vl + 3] = Vector2.right;
            uvs[vl + 4] = Vector2.up;
            uvs[vl + 5] = Vector2.up;
            uvs[vl + 6] = Vector2.one;
            uvs[vl + 7] = Vector2.one;
        }
        else
        {
            if (vl % 8 == 0)
            {
                uvs[vl] = Vector2.zero;
                uvs[vl + 1] = Vector2.zero;
                uvs[vl + 2] = Vector2.right;
                uvs[vl + 3] = Vector2.right;
            }
            else
            {
                uvs[vl] = Vector2.up;
                uvs[vl + 1] = Vector2.up;
                uvs[vl + 2] = Vector2.one;
                uvs[vl + 3] = Vector2.one;
            }
        }

        // actually drawing triangles

        int tl = m.triangles.Length;

        int[] ts = m.triangles;
        ts = resizeTriangles(ts, 12); //!misspelling of triangles !!!

        if (quad.Length == 2)
            vl -= 4;

        //front facing quad built up of 2 triangles
        // taking bottom left point, bottom right point, top left point
        // defining that as a triangle
        ts[tl] = vl;
        ts[tl + 1] = vl + 2;
        ts[tl + 2] = vl + 4;

        ts[tl + 3] = vl + 2;
        ts[tl + 4] = vl + 6;
        ts[tl + 5] = vl + 4;

        //back facing quad built up of 2 triangles
        // does it for complementary one
        ts[tl + 6] = vl + 5;
        ts[tl + 7] = vl + 3;
        ts[tl + 8] = vl + 1;

        ts[tl + 9] = vl + 5;
        ts[tl + 10] = vl + 7;
        ts[tl + 11] = vl + 3;

        // use same copy vertices and do it for back
        // in a flipped order ** important
        // calculate boundaries - important for adding colliders
        m.vertices = vs;
        m.uv = uvs;
        m.triangles = ts;
        m.RecalculateBounds();
        m.RecalculateNormals();
    }

    Vector3[] resizeVertices(Vector3[] ovs, int ns)
    {
        Vector3[] nvs = new Vector3[ovs.Length + ns];
        for (int i = 0; i < ovs.Length; i++) nvs[i] = ovs[i];
        return nvs;
    }

    Vector2[] resizeUVs(Vector2[] uvs, int ns)
    {
        Vector2[] nvs = new Vector2[uvs.Length + ns];
        for (int i = 0; i < uvs.Length; i++) nvs[i] = uvs[i];
        return nvs;
    }

    int[] resizeTriangles(int[] ovs, int ns)
    { //!!Misspelled triangles !!!
        int[] nvs = new int[ovs.Length + ns];
        for (int i = 0; i < ovs.Length; i++) nvs[i] = ovs[i];
        return nvs;
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
