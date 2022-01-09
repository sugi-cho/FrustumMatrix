using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sugi.frustum
{
    [RequireComponent(typeof(Camera))]
    [ExecuteInEditMode]
    public class CameraToPlaneFrustrum : MonoBehaviour
    {
        [SerializeField] Transform targetPlane;
        public float zNear = 0.01f;
        public float zFar = 1000f;
        new Camera camera => _c == null ? _c = GetComponent<Camera>() : _c;
        Camera _c;

        FrustumPlanes frustumPlanes;
        Matrix4x4 frustumMatrix;

        void SetFrustrum()
        {
            transform.rotation = targetPlane.rotation;

            var far = transform.InverseTransformPoint(targetPlane.position).z;
            var frustrumScale = zNear / far;

            var left = targetPlane.position - targetPlane.right * targetPlane.localScale.x * 0.5f;
            var right = targetPlane.position + targetPlane.right * targetPlane.localScale.x * 0.5f;
            var bottom = targetPlane.position - targetPlane.up * targetPlane.localScale.y * 0.5f;
            var top = targetPlane.position + targetPlane.up * targetPlane.localScale.y * 0.5f;

            frustumPlanes.left = transform.InverseTransformPoint(left).x * frustrumScale;
            frustumPlanes.right = transform.InverseTransformPoint(right).x * frustrumScale;
            frustumPlanes.bottom = transform.InverseTransformPoint(bottom).y * frustrumScale;
            frustumPlanes.top = transform.InverseTransformPoint(top).y * frustrumScale;
            frustumPlanes.zNear = zNear;
            frustumPlanes.zFar = zFar;

            frustumMatrix = Matrix4x4.Frustum(frustumPlanes);
            camera.projectionMatrix = frustumMatrix;
        }

        // Update is called once per frame
        void Update()
        {
            if (targetPlane != null)
                SetFrustrum();
        }
    }
}