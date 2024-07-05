using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private EnemyPattern pattern;
    public EnemyData data;

    private EnemySection[] sections;
    private void Start()
    {
        sections=gameObject.GetComponentsInChildren<EnemySection>();
    }
    public void SetPattern(EnemyPattern inPattern)
    {
        pattern = inPattern;
    }
    private void FixedUpdate()
    {
        data.progressTimer++;

        pattern.Calculate(transform, data.progressTimer);
    }
    public void EnableState(String name)
    {
        foreach(EnemySection section in sections)
        {
            section.EnableState(name);
        }
    }
    public void DisableState(String name)
    {
        foreach (EnemySection section in sections)
        {
            section.DisableState(name);
        }
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
