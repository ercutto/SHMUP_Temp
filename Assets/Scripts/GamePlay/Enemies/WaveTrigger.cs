using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WaveTrigger : MonoBehaviour
{
    public EnemyPattern[] patterns = null;
    public float[] delays = null;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("degdi");
        StartCoroutine(SpawnWave());
        SpawnWave();
    }
   
    IEnumerator SpawnWave()
    {
        int i = 0;
        foreach (EnemyPattern pattern in patterns) 
        { 
            Session.Hardness hardness =GameManager.Instance.gameSession.hardness;
            if(delays!=null&& i<delays.Length) yield return new WaitForSeconds(delays[i]);
            if (pattern.spawnOnEasy && hardness == Session.Hardness.Easy)
                pattern.Spawn(); 
            if (pattern.spawnOnNormal && hardness == Session.Hardness.Normal)
                pattern.Spawn();
            if (pattern.spawnOnHard && hardness == Session.Hardness.Hard)
                pattern.Spawn();
            if (pattern.spawnOnInsane && hardness == Session.Hardness.Insane)
                pattern.Spawn();
            i++;
        }
        yield return null;
    }
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        foreach (EnemyPattern pattern in patterns)
        { 
            Handles.DrawLine(transform.position, pattern.transform.position);
        }

    }
#endif
}
