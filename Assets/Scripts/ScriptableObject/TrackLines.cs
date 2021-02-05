using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

[CreateAssetMenu(fileName = "TrackLines", menuName = "ScriptableObjects/Charting/TrackLines", order = 1)]
public class TrackLines : SerializedScriptableObject {
    public List<LineData> trackLines;
}
