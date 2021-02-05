using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OffsetBar : MonoBehaviour {
    
    [SerializeField] private GameObject mark = default;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MakeMark(float offset) {
        GameObject newMark = Instantiate(mark, transform.position + new Vector3(0, offset, 0), Quaternion.identity, transform);
        StartCoroutine(FadeMark(newMark.GetComponent<Image>()));
    }

    IEnumerator FadeMark(Image markImage) {
        markImage.color = Color.black;
        
        while (markImage.color.a > 0.02f && markImage.color.r < 1) {
            markImage.color -= new Color(-0.02f, -0.02f, -0.02f, 0.02f);
            yield return new WaitForSeconds(0.05f);
        }
    }
}
