// LineRendererArc

using System;
using UnityEngine;

namespace Song.Components {
    public class LineRendererArc : MonoBehaviour
    {
        public float radius;

        public float arcLength;

        public int pointCount;

        public float lineWidth;

        public LineRenderer lineRenderer;

        private void Update()
        {
            DrawPolygon(pointCount, radius, base.transform.position, lineWidth, lineWidth, arcLength);
        }

        private void DrawPolygon(int vertexNumber, float radius, Vector3 centerPos, float startWidth, float endWidth, float arcLength)
        {
            lineRenderer.loop = false;
            lineRenderer.positionCount = vertexNumber;
            float num = 0f;
            for (int i = 0; i < vertexNumber; i++)
            {
                float x = Mathf.Sin((float)Math.PI / 180f * num) * radius;
                float y = Mathf.Cos((float)Math.PI / 180f * num) * radius;
                lineRenderer.SetPosition(i, (Vector2)centerPos + new Vector2(x, y));
                num += arcLength / (float)vertexNumber;
            }
        }

        public void UpdateArc(float newRadius, float newLength)
        {
            DrawPolygon(pointCount, newRadius, base.transform.position, lineWidth, lineWidth, newLength);
            radius = newRadius;
            arcLength = newLength;
        }
    }
}
