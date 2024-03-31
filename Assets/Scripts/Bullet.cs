using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
}
[Serializable]
public struct BulletData
{
    public float positionX;
    public float positionY;
    public float dX;
    public float dY;
    public float angle;
    public int   type;
    public bool  active;

    public BulletData(float inX,float inY,float inDX,float inDY,float inAngle,int inType,bool inActive)
    {
        positionX=inX;
        positionY=inY;
        dX=inDX;
        dY=inDY;
        angle=inAngle;
        type=inType;
        active=inActive;
    }
}

