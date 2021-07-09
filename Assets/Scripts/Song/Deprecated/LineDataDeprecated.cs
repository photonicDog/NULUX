using System;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

/*
[Serializable]
public class LineDataDeprecated {
    [VerticalGroup("Animation")] 
    
    [BoxGroup("Animation/Movement")] 
    [GUIColor(0.3f, 0.8f, 0.8f, 1f)] [HorizontalGroup("Animation/Movement/X")] [LabelText("Constant X")] public bool constPosX;
    [HorizontalGroup("Animation/Movement/X")] [HideLabel] [ShowIf("constPosX")] public float PosX;
    [HorizontalGroup("Animation/Movement/X")] [HideLabel] [HideIf("constPosX")] public AnimationCurve PosXCurve;
    
    [BoxGroup("Animation/Movement")] 
    [GUIColor(0.3f, 0.8f, 0.8f, 1f)] [HorizontalGroup("Animation/Movement/Y")] [LabelText("Constant Y")] public bool constPosY;
    [HorizontalGroup("Animation/Movement/Y")] [HideLabel] [ShowIf("constPosY")] public float PosY;
    [HorizontalGroup("Animation/Movement/Y")] [HideLabel] [HideIf("constPosY")] public AnimationCurve PosYCurve;
    
    [BoxGroup("Animation/Size")] 
    [GUIColor(0.8f, 0.3f, 0.2f, 1f)] [HorizontalGroup("Animation/Size/Selector")][LabelText("Constant Size")] public bool constSize;
    [HorizontalGroup("Animation/Size/Selector")] [HideLabel] [ShowIf("constSize")] public float Size;
    [HorizontalGroup("Animation/Size/Selector")] [HideLabel] [HideIf("constSize")] public AnimationCurve SizeCurve;

    [BoxGroup("Animation/Direction")] 
    [GUIColor(0.5f, 0.7f, 0.1f, 1f)] [HorizontalGroup("Animation/Direction/Selector")] [LabelText("Constant Direction")] public bool constDirection;
    [HorizontalGroup("Animation/Direction/Selector")] [HideLabel] [ShowIf("constDirection")] public float Direction;
    [HorizontalGroup("Animation/Direction/Selector")] [HideLabel] [HideIf("constDirection")] public AnimationCurve DirectionCurve;
    
    [BoxGroup("Animation/Mobility")]
    [GUIColor(0.9f, 0.2f, 0.9f, 1f)] [HorizontalGroup("Animation/Mobility/Selector")] [LabelText("Mobility")] public bool Mobile;
    
    [VerticalGroup("Misc")]
    [BoxGroup("Misc/Stationary")] public bool stationary;
    [BoxGroup("Misc/Telegraphing")] public float warnTime;
    [BoxGroup("Misc/Telegraphing")] public float fadeLength;
    [BoxGroup("Misc/TypeData")] public int index;
    [BoxGroup("Misc/TypeData")]public LineStyle Style = LineStyle.NULL; 
    
    [ShowIfGroup("Animation/Mobile")]
    [BoxGroup("Animation/Mobile/Movement")] 
    [HorizontalGroup("Animation/Mobile/Movement/X")] [LabelText("Constant X")] public bool dataConstPosX;
    [HorizontalGroup("Animation/Mobile/Movement/X")] [HideLabel] [ShowIf("dataConstPosX")] public float DataPosX;
    [HorizontalGroup("Animation/Mobile/Movement/X")] [HideLabel] [HideIf("dataConstPosX")] public AnimationCurve DataPosXCurve;
    
    [HorizontalGroup("Animation/Mobile/Movement/Y")] [LabelText("Constant Y")] public bool dataConstPosY;
    [HorizontalGroup("Animation/Mobile/Movement/Y")] [HideLabel] [ShowIf("dataConstPosY")] public float DataPosY;
    [HorizontalGroup("Animation/Mobile/Movement/Y")] [HideLabel] [HideIf("dataConstPosY")] public AnimationCurve DataPosYCurve;
    [BoxGroup("Animation/Mobile/Size")] 
    [HorizontalGroup("Animation/Mobile/Size/Selector")][LabelText("Constant Size")] public bool dataConstSize;
    [HorizontalGroup("Animation/Mobile/Size/Selector")] [HideLabel] [ShowIf("dataConstSize")] public float DataSize;
    [HorizontalGroup("Animation/Mobile/Size/Selector")] [HideLabel] [HideIf("dataConstSize")] public AnimationCurve DataSizeCurve;
    [BoxGroup("Animation/Mobile/Direction")] 
    [HorizontalGroup("Animation/Mobile/Direction/Selector")] [LabelText("Constant Direction")] public bool dataConstDirection;
    [HorizontalGroup("Animation/Mobile/Direction/Selector")] [HideLabel] [ShowIf("dataConstDirection")] public float DataDirection;
    [HorizontalGroup("Animation/Mobile/Direction/Selector")] [HideLabel] [HideIf("dataConstDirection")] public AnimationCurve DataDirectionCurve;
}

public class LineDataCommand {
    public LineDataDeprecated data;
    public float startTime;
    public float endTime;

    public LineDataCommand(LineDataDeprecated data, float startTime, float endTime) {
        this.data = data;
        this.startTime = startTime;
        this.endTime = endTime;
    }

    public LineState CalculateLineState(float currentBeat) {
        LineState res = new LineState();

        if (currentBeat < startTime) {
            // Lead-in interp math.
            float leadInTime = ((currentBeat - startTime) / (endTime - startTime));

            float currentDir = data.constDirection ? data.Direction : CurveExtrap(data.DirectionCurve, leadInTime);
            float currentScale = data.constSize ? data.Size : CurveExtrap(data.SizeCurve, leadInTime);
            Vector2 positionVector = new Vector2(
                data.constPosX ? data.PosX : CurveExtrap(data.PosXCurve, leadInTime), 
                data.constPosY ? data.PosY : CurveExtrap(data.PosYCurve, leadInTime)
            );  
            
            Quaternion rotationVector = Quaternion.Euler(0f, 0f, 0f - currentDir);

            res.position = positionVector;
            res.rotation = rotationVector;
            res.scale = currentScale;
        }
        else {
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

 
        }
        return res;
    }

    public LineState CalculateMobileLineState(float currentBeat) {
        LineState res = new LineState();
        LineState parent = CalculateLineState(currentBeat);
        if (currentBeat < startTime) {
            // Lead-in interp math.
            float leadInTime = ((currentBeat - startTime) / (endTime - startTime));

            float currentDir = data.dataConstDirection ? data.DataDirection : CurveExtrap(data.DataDirectionCurve, leadInTime);
            float currentScale = data.dataConstSize ? data.DataSize : CurveExtrap(data.DataSizeCurve, leadInTime);
            Vector2 positionVector = new Vector2(
                data.dataConstPosX ? data.DataPosX : CurveExtrap(data.DataPosXCurve, leadInTime), 
                data.dataConstPosY ? data.DataPosY : CurveExtrap(data.DataPosYCurve, leadInTime)
            );  
            
            Quaternion rotationVector = Quaternion.Euler(0f, 0f, 0f - currentDir);

            res.position = positionVector;
            res.rotation = rotationVector;
            res.scale = currentScale;
        }
        else {
            // Get values based on if they're constant or evaluate at time.
            float normalTime = Mathf.InverseLerp(startTime, endTime, currentBeat);
            float currentDir = data.dataConstDirection ? data.DataDirection : data.DataDirectionCurve.Evaluate(normalTime);
            float currentScale = data.dataConstSize ? data.DataSize : data.DataSizeCurve.Evaluate(normalTime);
            Vector2 positionVector = new Vector2(
                data.dataConstPosX ? data.DataPosX : data.DataPosXCurve.Evaluate(normalTime), 
                data.dataConstPosY ? data.DataPosY : data.DataPosYCurve.Evaluate(normalTime)
            );  
        
            Quaternion rotationVector = Quaternion.Euler(0f, 0f, 0f - currentDir);

            res.position = positionVector;
            res.rotation = rotationVector;
            res.scale = currentScale;
        }

        res.position += parent.position;
        res.rotation = Quaternion.Euler(res.rotation.eulerAngles + parent.rotation.eulerAngles);
        res.scale *= parent.scale;
        
        return res;
    }

    public Vector2 GetNotePosition(float noteBeat, float pos) {
        LineState line = CalculateLineState(noteBeat);

        float correctedPosPct = (pos - (data.Style != LineStyle.CIRCLE?50f:25f)) * 2f;

        if (data.Style == LineStyle.STRAIGHT) {
            Vector2 upwardPosition = Vector2.up * (correctedPosPct/100f) / 2;
            return (Vector2)(line.rotation * (upwardPosition * line.scale)) + line.position ;
        }
        
        if (data.Style == LineStyle.CIRCLE) {
            Vector2 upwardPosition = Vector2.up * (correctedPosPct/100f);
            return (Vector2)(line.rotation * (upwardPosition * line.scale)) + line.position;
        }
        
        Vector2 angle = Quaternion.Euler(0, 0, -(pos/100) * 360f) * Vector3.down;
        return angle * line.scale + line.position;
    }

    public Vector2 GetMobileNotePosition(float currentBeat, float pos) {
        LineState dataLine = CalculateMobileLineState(currentBeat);
        
        float correctedPosPct = (pos - (data.Style != LineStyle.CIRCLE?50f:25f)) * 2f;
        
        if (data.Style == LineStyle.STRAIGHT) {
            Vector2 upwardPosition = Vector2.up * (correctedPosPct/100f) / 2;
            return (Vector2)(dataLine.rotation * (upwardPosition * dataLine.scale)) + dataLine.position ;
        }
        
        if (data.Style == LineStyle.CIRCLE) {
            Vector2 upwardPosition = Vector2.up * (correctedPosPct/100f);
            return (Vector2)(dataLine.rotation * (upwardPosition * dataLine.scale)) + dataLine.position;
        }
        
        Vector2 angle = Quaternion.Euler(0, 0, -(pos/100) * 360f) * Vector3.down;
        return angle * dataLine.scale + dataLine.position;
    }

    private float CurveExtrap(AnimationCurve curve, float back) {
        float basePoint = curve.Evaluate(0);
        float interpPoint = curve.Evaluate(0.1f);

        return basePoint + ((interpPoint-basePoint)/0.1f) * (back);
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
*/