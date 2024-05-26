using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyPattern : MonoBehaviour
{
    private int UID;
    public List<EnemyStep> steps = new List<EnemyStep>();
    public bool stayOnLast = true;
    public Enemy enemyPrefab;
    private Enemy spawnedEnemy;
    [MenuItem("GameObject/SHMUP/EnemyPattern", false, 10)]
    static void CreateEnenmyPatternObject(MenuCommand menuComand)
    {
        Helpers helper = (Helpers)Resources.Load("Helper");
        if (helper != null)
        {
            GameObject go = new GameObject("EnemyPattern" + helper.nextFreePatternID);
            EnemyPattern pattern = go.AddComponent<EnemyPattern>();
            pattern.UID = helper.nextFreePatternID;
            helper.nextFreePatternID++;

            //register creation with undo system
            Undo.RegisterCompleteObjectUndo(go, "Create" + go.name);
            Selection.activeObject = go;
        }
        else
        {
            Debug.LogError("Could not find Helper");
        }
    }

    public void Spawn()
    {
        spawnedEnemy = Instantiate(enemyPrefab, transform.position, transform.rotation).GetComponent<Enemy>();
        spawnedEnemy.SetPattern(this);
    }

    public Vector2 CalculatePosition(float progressTimer)
    {
        int s = WhichStep(progressTimer);

        if(s<0) return spawnedEnemy.transform.position;

        EnemyStep step = steps[s];
        float stepTime = progressTimer - StartTime(s);
        Vector3 startPos = EndPosition(s - 1);

        return step.CalculetePosition(startPos, stepTime);

    }
    public Quaternion CalculateRotation(float progressTimer)
    {
        return Quaternion.identity;
    }

    int WhichStep(float timer)
    {
        float timeToCheck = timer;
        for (int s = 0; s<steps.Count; s++)
        {
            if (timeToCheck < steps[s].TimeToComplate())
            {
                return s;
            }

            timeToCheck -= steps[s].TimeToComplate();


        }
        if(stayOnLast)
            return steps.Count - 1;
        return -1;
    }
    public float StartTime(int step)
    {
        if (step <= 0) return 0;
        
        float result = 0;
        for(int s=0;s<step;s++)
        {
            result += steps[s].TimeToComplate();
        }
        
        return result;
    }
    public Vector3 EndPosition(int stepIndex)
    {
        Vector3 result = transform.position;
        if (stepIndex >= 0)
        {
            for(int s=0; s <= stepIndex; s++)
            {
                result = steps[s].EndPosition(result);
            }
        }
        return result;
    }
}

    
