using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class ParallaxBG : SerializedMonoBehaviour {
    public List<ParallaxPlane> plx;

    public float x1, y1, x2, y2, x3, y3;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate() {
        // TODO: Make this not suck so bad lol;
        plx[0].xVel = x1;
        plx[0].yVel = y1;
        plx[1].xVel = x2;
        plx[1].yVel = y2;
        plx[2].xVel = x3;
        plx[2].yVel = y3;

        foreach (ParallaxPlane plane in plx) {
            plane.spr.uvRect = new Rect(
                new Vector2(
                    plane.spr.uvRect.x + (plane.x ? plane.xVel*0.01f : 0), 
                    plane.spr.uvRect.y + (plane.y ? plane.yVel*0.01f : 0)),
                plane.spr.uvRect.size);

        }
    }
}

[Serializable]
public class ParallaxPlane {
    public float xVel;
    public float yVel;
    public RawImage spr;
    public bool x;
    public bool y;

    public ParallaxPlane(float xVel, float yVel, RawImage spr, bool x, bool y) {
        this.xVel = xVel;
        this.yVel = yVel;
        this.spr = spr;
        this.x = x;
        this.y = y;
    }
}
