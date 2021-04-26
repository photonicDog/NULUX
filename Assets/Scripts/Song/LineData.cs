using System;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

[Serializable]
public class LineData {
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
    [BoxGroup("Misc/Telegraphing")] public float warnTime;
    [BoxGroup("Misc/Telegraphing")] public float fadeLength;
    [BoxGroup("Misc/TypeData")] public int index;
    [BoxGroup("Misc/TypeData")] public LineStyle Style = LineStyle.NULL; 
    
    [ShowIfGroup("Animation/Mobile")]
    [BoxGroup("Animation/Mobile/Movement")] 
    [HorizontalGroup("Animation/Mobile/Movement/X")] [LabelText("Constant X")] public bool mobileConstPosX;
    [HorizontalGroup("Animation/Mobile/Movement/X")] [HideLabel] [ShowIf("mobileConstPosX")] public float MobilePosX;
    [HorizontalGroup("Animation/Mobile/Movement/X")] [HideLabel] [HideIf("mobileConstPosX")] public AnimationCurve MobilePosXCurve;
    
    [HorizontalGroup("Animation/Mobile/Movement/Y")] [LabelText("Constant Y")] public bool mobileConstPosY;
    [HorizontalGroup("Animation/Mobile/Movement/Y")] [HideLabel] [ShowIf("mobileConstPosY")] public float MobilePosY;
    [HorizontalGroup("Animation/Mobile/Movement/Y")] [HideLabel] [HideIf("mobileConstPosY")] public AnimationCurve MobilePosYCurve;
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

    public LineState CalculateLineState(float currentBeat)
    {
        var res = new LineState();

        if (currentBeat < startTime)
        {
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
        else
        {
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

    public LineState CalculateMobileLineState(float currentBeat)
    {
        var res = new LineState();
        if (currentBeat < startTime)
        {
            // Lead-in interp math.
            float leadInTime = ((currentBeat - startTime) / (endTime - startTime));
            Vector2 positionVector = new Vector2(
                data.mobileConstPosX ? data.MobilePosX : CurveExtrap(data.MobilePosXCurve, leadInTime),
                data.mobileConstPosY ? data.MobilePosY : CurveExtrap(data.MobilePosYCurve, leadInTime)
            );

            res.position = positionVector;
        }
        else
        {
            // Get values based on if they're constant or evaluate at time.
            var time = Conductor.Instance.GetSongTime();
            float normalTime = Mathf.InverseLerp(currentBeat - data.fadeLength, currentBeat, time);
            Vector2 positionVector = new Vector2(
                data.mobileConstPosX ? data.MobilePosX : data.MobilePosXCurve.Evaluate(normalTime),
                data.mobileConstPosY ? data.MobilePosY : data.MobilePosYCurve.Evaluate(normalTime)
            );

            res.position = positionVector;
        }

        return res;
    }

    public Vector2 GetNotePosition(float noteBeat, float pos, bool isMobile) {
        if(isMobile)
        {
            LineState line = CalculateMobileLineState(noteBeat);

            float correctedPosPct = (pos - (data.Style != LineStyle.CIRCLE ? 50f : 25f)) * 2f;

            if (data.Style == LineStyle.STRAIGHT)
            {
                Vector2 upwardPosition = Vector2.up * (correctedPosPct / 100f) / 2;
                return (Vector2)(line.rotation * (upwardPosition * line.scale)) + line.position;
            }

            if (data.Style == LineStyle.CIRCLE)
            {
                Vector2 upwardPosition = Vector2.up * (correctedPosPct / 100f);
                return (Vector2)(line.rotation * (upwardPosition * line.scale)) + line.position;
            }

            Vector2 angle = Quaternion.Euler(0, 0, -(pos / 100) * 360f) * Vector3.down;
            return angle * line.scale + line.position;

        } else
        {
            LineState line = CalculateLineState(noteBeat);

            float correctedPosPct = (pos - (data.Style != LineStyle.CIRCLE ? 50f : 25f)) * 2f;

            if (data.Style == LineStyle.STRAIGHT)
            {
                Vector2 upwardPosition = Vector2.up * (correctedPosPct / 100f) / 2;
                return (Vector2)(line.rotation * (upwardPosition * line.scale)) + line.position;
            }

            if (data.Style == LineStyle.CIRCLE)
            {
                Vector2 upwardPosition = Vector2.up * (correctedPosPct / 100f);
                return (Vector2)(line.rotation * (upwardPosition * line.scale)) + line.position;
            }

            Vector2 angle = Quaternion.Euler(0, 0, -(pos / 100) * 360f) * Vector3.down;
            return angle * line.scale + line.position;
        }

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