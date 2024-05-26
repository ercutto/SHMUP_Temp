using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private EnemyPattern pattern;
    public EnemyData data;
    public void SetPattern(EnemyPattern inPattern)
    {
        pattern = inPattern;
    }
    private void FixedUpdate()
    {
        data.progressTimer++;
        Vector3 pos= pattern.CalculatePosition(data.progressTimer);
        Quaternion rot=pattern.CalculateRotation(data.progressTimer);

        transform.position = pos;
        transform.rotation = rot;
    }
    [Serializable]
    public struct EnemyData
    {
        public float progressTimer;
        public float positionX;
        public float positionY;
        public int patternUID;
    }
}
