// MenuOptionBox
using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuOptionBox : SerializedMonoBehaviour
{
    public Dictionary<int, Tuple<string, Sprite>> entries;

    [SerializeField]
    private TextMeshProUGUI descriptionText;

    [SerializeField]
    private Image image;

    private void Start()
    {
    }

    private void Update()
    {
    }

    public void UpdateBox(int i)
    {
        if (entries[i] != null)
        descriptionText.text = entries[i].Item1;
    }
}
