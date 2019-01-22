using UnityEngine;
using UnityEngine.Events;

[ExecuteInEditMode]
public class VrWindowRenderer : MonoBehaviour
{
    public Camera cam;
    public VrWindow window;
    public float zNear = 0.01f;
    public float zFar = 1000f;
    [SerializeField] RenderTexture output;
    public RenderTextureEvent onCreateOutput;

    FrustumPlanes frustrumPlanes;
    Matrix4x4 frustrumMatrix;

    void SetFrustrum()
    {
        cam.transform.rotation = window.transform.rotation;

        var camTrs = cam.transform;
        var far = camTrs.InverseTransformPoint(window.transform.position).z;
        var frustrumScale = zNear / far;

        frustrumPlanes.left = camTrs.InverseTransformPoint(window.left).x * frustrumScale;
        frustrumPlanes.right = camTrs.InverseTransformPoint(window.right).x * frustrumScale;
        frustrumPlanes.bottom = camTrs.InverseTransformPoint(window.bottom).y * frustrumScale;
        frustrumPlanes.top = camTrs.InverseTransformPoint(window.top).y * frustrumScale;
        frustrumPlanes.zNear = zNear;
        frustrumPlanes.zFar = zFar;

        frustrumMatrix = Matrix4x4.Frustum(frustrumPlanes);
        cam.projectionMatrix = frustrumMatrix;
    }

    void Setup()
    {
        window.SetupRenderer();
        output = cam.targetTexture = window.output;
        onCreateOutput.Invoke(output);
        SetFrustrum();
    }

    private void OnValidate()
    {
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

    [System.Serializable]
    public class RenderTextureEvent : UnityEvent<RenderTexture> { }
}
