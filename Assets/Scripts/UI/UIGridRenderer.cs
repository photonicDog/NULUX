using UnityEngine;
using UnityEngine.UI;

public class UIGridRenderer : Graphic {
    protected override void OnPopulateMesh(VertexHelper vh) {
        vh.Clear();

        float width = rectTransform.rect.width;
        float height = rectTransform.rect.height;
        
        UIVertex vert = UIVertex.simpleVert;
        vert.color = color;
        
        vert.position = new Vector3(0, 0);
        vh.AddVert(vert);
        
        vert.position = new Vector3(0, height);
        vh.AddVert(vert);
        
        vert.position = new Vector3(width, height);
        vh.AddVert(vert);        
        
        vert.position = new Vector3(width, 0);
        vh.AddVert(vert);
        
        vh.AddTriangle(0, 1, 2);
        vh.AddTriangle(2, 3, 0);
    }
}

