using System;

using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class EnemyState 
{
    public string stateName;
    public bool active = false;

    [Space(10)]
    [Header("Start Event")]
    public UnityEvent eventOnStart = null;

    [Space(10)]
    [Header("End Event")]
    public UnityEvent eventOnEnd = null;

    [Space(10)]
    [Header("Timer")]
    public UnityEvent eventOnTime= null;

    public bool useTimer=false;
    public float timer = 0;
    [HideInInspector]
    public float currentTime = 0;

    public void Enable()
    {
        currentTime = 0;
        
        eventOnStart.Invoke();
        active = true;
    }
    public void Disable()
    {
        eventOnEnd.Invoke();
        active = false;
    }

    public void IncrimentTime()
    {
        if (useTimer)
        {
            currentTime++;
            if (currentTime >= timer)
            {
                Debug.Log(currentTime);
                eventOnTime.Invoke();
                currentTime = 0;
            }
        }
    }
}
