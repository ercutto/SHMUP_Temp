using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
   public int index;// index in to bullet pool
}
[Serializable]
public struct BulletData
{
    public float positionX;
    public float positionY;
    public float dX;
    public float dY;
    public float angle;
    public float dAngle;
    public int   type;
    public bool  active;
    public bool hooming;

    public BulletData(float inX,float inY,float inDX,float inDY,float inAngle,float inDAngle,int inType,bool inActive,bool inhooming)
    {
        positionX=inX;
        positionY=inY;
        dX=inDX;
        dY=inDY;
        angle = inAngle;
        dAngle=inDAngle;
        type =inType;
        active=inActive;
        hooming=inhooming;
    }
}

