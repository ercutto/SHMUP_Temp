using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private EnemyPattern pattern;
    public EnemyData data;

    public EnemyRule[] rules;

    private EnemySection[] sections;

    public bool isBoss = false;
    private int timer;
    public int timeOut = 3600;
    private bool timedOut=false;
    Animator animator=null;
    public string timeOutParameterName = null;

    private WaveTrigger owningWave = null;
    private void Start()
    {
        sections=gameObject.GetComponentsInChildren<EnemySection>();
        animator=GetComponentInChildren<Animator>();
        timer=timeOut;
    }
    public void SetWave(WaveTrigger wave)
    {
        owningWave = wave;
    }
    public void SetPattern(EnemyPattern inPattern)
    {
        pattern = inPattern;
    }
    private void FixedUpdate()
    {
        //timeOut

        if (isBoss)
        {
            if (timer <= 0 && !timedOut)
            {
                timedOut = true;
                if (animator)
                    animator.SetTrigger(timeOutParameterName);
                sections[0].EnableState("TimeOut");
                
            }
            else timer--;
        }

        data.progressTimer++;
        if(pattern)
            pattern.Calculate(transform, data.progressTimer);

        //off Screen check
      
        float y = transform.position.y;
        if (GameManager.Instance && GameManager.Instance.progressWindow)
            y -= GameManager.Instance.progressWindow.data.positionY;
        if (y < -200)
            OutOfBounds();
        
        //update stateTimer
        foreach(EnemySection section in sections)
        {
            section.UpdateStateTimer();
        }

        
    }
    public void TimeOutDistruct()
    {
        Destroy(gameObject);
    }
    
    void OutOfBounds()
    {
        Destroy(gameObject);
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

    public void PartDestroyed()
    {
        //Go through all rules and check for parts matching rule set

        foreach(EnemyRule rule in rules)
        {
            if (!rule.triggered)
            {
                int noOFDestroyedParts = 0;
                foreach(EnemyPart part in rule.partsToCheck)
                {
                    if (part.destroyed)
                    {
                        noOFDestroyedParts++;
                    }
                }
                if (noOFDestroyedParts >= rule.noOfPartsRequired)
                {
                    rule.triggered = true;
                    rule.ruleEvent.Invoke();
                }
            }
        }
    }

    public void Destroyed(int triggeredFromRuleIndex)
    {
        EnemyRule triggeredRule= rules[triggeredFromRuleIndex];
        int playerIndex = triggeredRule.partsToCheck[0].destroyedByPlayer;//tode check using just first index is safe
        if (owningWave)
        {
            owningWave.EnemyDestroyed(transform.position, playerIndex);
        }
        Destroy(gameObject);
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
