using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "ImageAssetKey", menuName = "ScriptableObjects/Asset Management/Image Asset Key", order = 1)]
public class ImageAssetKey : SerializedScriptableObject {
    public Dictionary<string, Sprite> images;

    public Sprite LookupAsset(string s) {
        return images[s];
    }
}