// LineRendererCircle

using System;
using UnityEngine;

namespace Song.Components {
    [ExecuteInEditMode]
    public class LineRendererCircle : MonoBehaviour
    {
        public float radius;

        public int pointCount;

        public float lineWidth;

        public LineRenderer lineRenderer;


        private void Update()
        {
            DrawPolygon(pointCount, radius, base.transform.position, lineWidth, lineWidth);
        }

        private void DrawPolygon(int vertexNumber, float radius, Vector3 centerPos, float startWidth, float endWidth)
        {
            lineRenderer.startWidth = startWidth;
            lineRenderer.endWidth = endWidth;
            lineRenderer.loop = true;
            float num = (float)Math.PI * 2f / (float)vertexNumber;
            lineRenderer.positionCount = vertexNumber;
            for (int i = 0; i < vertexNumber; i++)
            {
                Matrix4x4 matrix4x = new Matrix4x4(new Vector4(Mathf.Cos(num * (float)i), Mathf.Sin(num * (float)i), 0f, 0f), new Vector4(-1f * Mathf.Sin(num * (float)i), Mathf.Cos(num * (float)i), 0f, 0f), new Vector4(0f, 0f, 1f, 0f), new Vector4(0f, 0f, 0f, 1f));
                Vector3 point = new Vector3(0f, radius, 0f);
                lineRenderer.SetPosition(i, centerPos + matrix4x.MultiplyPoint(point));
            }
        }

        public void UpdateRadius(float newRadius)
        {
            DrawPolygon(pointCount, newRadius, base.transform.position, lineWidth, lineWidth);
            radius = newRadius;
        }
    }
}
