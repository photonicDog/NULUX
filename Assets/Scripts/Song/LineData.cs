using System;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

[Serializable]
public class LineData {
    [HorizontalGroup("Data")] 
    [VerticalGroup("Data/Animation")] 
    
    [BoxGroup("Data/Animation/Movement")] 
    [HorizontalGroup("Data/Animation/Movement/X")] [LabelText("Constant X")] public bool constPosX;
    [HorizontalGroup("Data/Animation/Movement/X")] [HideLabel] [ShowIf("constPosX")] public float PosX;
    [HorizontalGroup("Data/Animation/Movement/X")] [HideLabel] [HideIf("constPosX")] public AnimationCurve PosXCurve;
    
    [BoxGroup("Data/Animation/Movement")] 
    [HorizontalGroup("Data/Animation/Movement/Y")] [LabelText("Constant Y")] public bool constPosY;
    [HorizontalGroup("Data/Animation/Movement/Y")] [HideLabel] [ShowIf("constPosY")] public float PosY;
    [HorizontalGroup("Data/Animation/Movement/Y")] [HideLabel] [HideIf("constPosY")] public AnimationCurve PosYCurve;
    
    [BoxGroup("Data/Animation/Size")] 
    [HorizontalGroup("Data/Animation/Size/Selector")][LabelText("Constant Size")] public bool constSize;
    [HorizontalGroup("Data/Animation/Size/Selector")] [HideLabel] [ShowIf("constSize")] public float Size;
    [HorizontalGroup("Data/Animation/Size/Selector")] [HideLabel] [HideIf("constSize")] public AnimationCurve SizeCurve;

    [BoxGroup("Data/Animation/Direction")] 
    [HorizontalGroup("Data/Animation/Direction/Selector")] [LabelText("Constant Direction")] public bool constDirection;
    [HorizontalGroup("Data/Animation/Direction/Selector")] [HideLabel] [ShowIf("constDirection")] public float Direction;
    [HorizontalGroup("Data/Animation/Direction/Selector")] [HideLabel] [HideIf("constDirection")] public AnimationCurve DirectionCurve;
    
    [VerticalGroup("Data/Misc")]
    [BoxGroup("Data/Misc/Stationary")] public bool stationary;
    [BoxGroup("Data/Misc/Telegraphing")] public float warnTime;
    [BoxGroup("Data/Misc/Telegraphing")] public float fadeLength;
    [BoxGroup("Data/Misc/TypeData")] public int index;

    public LineStyle Style = LineStyle.NULL; 

}

public class LineDataCommand {
    public LineData data;
    public float startTime;
    public float endTime;

    public LineDataCommand(LineData data, float startTime, float endTime) {
        this.data = data;
        this.startTime = startTime;
        this.endTime = endTime;
    }

    public LineState CalculateLineState(float currentBeat) {
        LineState res = new LineState();
        
        // Get values based on if they're constant or evaluate at time.
        float normalTime = Mathf.InverseLerp(startTime, endTime, currentBeat);
        float currentDir = data.constDirection ? data.Direction : data.DirectionCurve.Evaluate(normalTime);
        float currentScale = data.constSize ? data.Size : data.SizeCurve.Evaluate(normalTime);
        Vector2 positionVector = new Vector2(
            data.constPosX ? data.PosX : data.PosXCurve.Evaluate(normalTime), 
            data.constPosY ? data.PosY : data.PosYCurve.Evaluate(normalTime)
        );  
        
        Quaternion rotationVector = Quaternion.Euler(0f, 0f, 0f - currentDir);

        res.position = positionVector;
        res.rotation = rotationVector;
        res.scale = currentScale;

        return res;
    }

    public Vector2 GetNotePosition(float currentBeat, float pos) {
        LineState line = CalculateLineState(currentBeat);

        float correctedPosPct = (pos - 50f) * 2f;

        if (data.Style == LineStyle.STRAIGHT) {
            Vector2 upwardPosition = Vector2.up * (correctedPosPct/100f) / 4;
            return (Vector2)(line.rotation * (upwardPosition * line.scale * 2)) + line.position ;
        }
        if (data.Style == LineStyle.CIRCLE) {
            Vector2 upwardPosition = Vector2.up * (correctedPosPct/100f) / 2;
            return (Vector2)(line.rotation * (upwardPosition * line.scale)) + line.position;
        }
        Vector2 angle = Quaternion.Euler(0, 0, -(pos/100) * 360f) * Vector3.down;
        return angle * line.scale + line.position;
    }
}

public class LineState {
    public Vector2 position;
    public Quaternion rotation;
    public float scale;
}

public enum LineStyle {
    NULL,
    STRAIGHT,
    CIRCLE,
    PULSE
}