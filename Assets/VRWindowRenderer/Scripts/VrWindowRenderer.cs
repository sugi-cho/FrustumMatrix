using UnityEngine;

[ExecuteInEditMode]
public class VrWindowRenderer : MonoBehaviour
{
    public Camera cam;
    public VrWindow window;
    [SerializeField] RenderTexture output;

    FrustumPlanes frustrumPlanes;
    Matrix4x4 frustrumMatrix;

    void SetFrustrum()
    {
        cam.transform.rotation = window.transform.rotation;

        var camTrs = cam.transform;

        frustrumPlanes.left = camTrs.InverseTransformPoint(window.left).x;
        frustrumPlanes.right = camTrs.InverseTransformPoint(window.right).x;
        frustrumPlanes.bottom = camTrs.InverseTransformPoint(window.bottom).y;
        frustrumPlanes.top = camTrs.InverseTransformPoint(window.top).y;
        frustrumPlanes.zNear = camTrs.InverseTransformPoint(window.transform.position).z;
        frustrumPlanes.zFar = 1000;

        frustrumMatrix = Matrix4x4.Frustum(frustrumPlanes);
        cam.projectionMatrix = frustrumMatrix;
    }

    void Setup()
    {
        window.SetupRenderer();
        output = cam.targetTexture = window.output;
        SetFrustrum();
    }

    // Start is called before the first frame update
    void Start()
    {
        Setup();
    }

    // Update is called once per frame
    void Update()
    {
        if (cam.transform.hasChanged || window.transform.hasChanged)
            SetFrustrum();

        cam.transform.hasChanged = false;
        window.transform.hasChanged = false;
    }
}
