using System;

using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class EnemyState 
{
    public string stateName;
    private bool active = false;

    [Space(10)]
    [Header("Start Event")]
    public UnityEvent eventOnStart = null;

    [Space(10)]
    [Header("End Event")]
    public UnityEvent eventOnEnd = null;

    [Space(10)]
    [Header("Timer")]
    public UnityEvent evenOnTime= null;

    public bool useTimer=false;
    public float timer = 0;
    public float currentTime = 0;

    public void Enable()
    {
       
        eventOnStart.Invoke();
    }
    public void Disable()
    {
        eventOnEnd.Invoke();
    }
}
