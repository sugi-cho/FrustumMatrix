using UnityEngine;

[RequireComponent(typeof(MeshRenderer)), RequireComponent(typeof(MeshFilter))]
public class VrWindow : MonoBehaviour
{
    public float size = 1f;
    public int width = 512;
    public int height = 512;
    float aspect { get { return (float)width / height; } }

    public RenderTexture output;

    public void SetupRenderer()
    {
        var meshRenderer = GetComponent<MeshRenderer>();
        var meshFilter = GetComponent<MeshFilter>();

        Mesh mesh = new Mesh();

        if (meshFilter.sharedMesh != null)
            mesh = meshFilter.sharedMesh;
        else
            mesh = new Mesh();

        var w = size * aspect;
        var h = size;
        var vertices = new[] {
            new Vector3(-0.5f*w,-0.5f*h,0f),
            new Vector3( 0.5f*w,-0.5f*h,0f),
            new Vector3(-0.5f*w, 0.5f*h,0f),
            new Vector3( 0.5f*w, 0.5f*h,0f),
        };
        var uv = new[]
        {
            new Vector2(0,0),
            new Vector2(1,0),
            new Vector2(0,1),
            new Vector2(1,1),
        };
        var indices = new[]
        {
            0,2,1,
            1,2,3,
        };
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.SetIndices(indices, MeshTopology.Triangles, 0);

        meshFilter.sharedMesh = mesh;

        output = new RenderTexture(width, height, 24, RenderTextureFormat.DefaultHDR);

        var mpb = new MaterialPropertyBlock();
        meshRenderer.GetPropertyBlock(mpb);
        mpb.SetTexture("_MainTex", output);
        meshRenderer.SetPropertyBlock(mpb);
    }

    private void OnDrawGizmos()
    {
        var tmp = Gizmos.matrix;
        Gizmos.matrix = transform.localToWorldMatrix;
        var h = size;
        var w = h * aspect;
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(w, h, 0));
        Gizmos.matrix = tmp;
    }

    private void OnDestroy()
    {
        if (output != null)
            output.Release();
    }

    public Vector3 right { get { return transform.position + transform.right * size * aspect * 0.5f; } }
    public Vector3 left { get { return transform.position - transform.right * size * aspect * 0.5f; } }
    public Vector3 top { get { return transform.position + transform.up * size * 0.5f; } }
    public Vector3 bottom { get { return transform.position - transform.up * size * 0.5f; } }
}
