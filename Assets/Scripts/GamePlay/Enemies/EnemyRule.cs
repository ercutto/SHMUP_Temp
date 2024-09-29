using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[Serializable]
public class EnemyRule
{
    public bool triggered;
    public int noOfPartsRequired;
    public List<EnemyPart>partsToCheck=new List<EnemyPart>();

    [Space(10)]
    [Header("--On rule triggered--")]
    [Space(10)]
    public UnityEvent ruleEvent = null;

    public List<int> eventDelays = new List<int>();
}
