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
    private int currentStateIndex = 0;
    private int previoustStateIndex = -1;
    public bool startActive = false;

    //hardness
    public bool spawnOnEasy = true;
    public bool spawnOnNormal = true;
    public bool spawnOnHard = false;
    public bool spawnOnInsane = false;

    [HideInInspector]
    public Vector3 lastPosition=Vector3.zero;
    [HideInInspector]
    public Vector3 currentPosition=Vector3.zero;
    [HideInInspector]
    public Quaternion lastAngle = Quaternion.identity;


#if UNITY_EDITOR
    [MenuItem("GameObject/SHMUP/EnemyPattern", false, 10)]
    static void CreateEnenmyPatternObject(MenuCommand menuCommand)
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
#endif
    private void Start()
    {
        if (startActive)
        {
            Spawn();
        }
    }
    public void Spawn()
    {
        if (spawnedEnemy == null)
        {
            spawnedEnemy = Instantiate(enemyPrefab, transform.position, transform.rotation).GetComponent<Enemy>();
            spawnedEnemy.SetPattern(this);

            lastPosition=spawnedEnemy.transform.position;
            currentPosition = lastPosition;
        }
    }
    public void Calculate(Transform enemyTransform , float progressTimer)
    {
        Vector3 pos = CalculatePosition(progressTimer);
        Quaternion rot = CalculateRotation(progressTimer);

        enemyTransform.position=pos;
        enemyTransform.rotation=rot;

        if (currentStateIndex != previoustStateIndex) //So state is changed
        {
           
            if (previoustStateIndex >= 0)
            {

                //call deActivateState
               
                EnemyStep prevStep = steps[previoustStateIndex];
                prevStep.FireDeactivateStates(spawnedEnemy);
            }
            if (currentStateIndex >= 0)
            {
              
                // call activateState
                EnemyStep currStep = steps[currentStateIndex];
                currStep.FireActivateStates(spawnedEnemy);

            }
            previoustStateIndex = currentStateIndex;
        }


    }
    public Vector2 CalculatePosition(float progressTimer)
    {
        currentStateIndex = WhichStep(progressTimer);//in Charge of calculating State index

        if(currentStateIndex<0) return spawnedEnemy.transform.position;

        lastPosition=currentPosition;

        EnemyStep step = steps[currentStateIndex];
        float stepTime = progressTimer - StartTime(currentStateIndex);
        Vector3 startPos = EndPosition(currentStateIndex - 1);

        currentPosition= step.CalculetePosition(startPos, stepTime,lastPosition,lastAngle);

        return currentPosition;

    }
    public Quaternion CalculateRotation(float progressTimer)
    {
        currentStateIndex = WhichStep(progressTimer);
        float startRotation = 0;
        if (currentStateIndex > 0)
            startRotation = steps[currentStateIndex - 1].EndRotation();
        float stepTime=progressTimer - StartTime(currentStateIndex);
        lastAngle = steps[currentStateIndex].CalculateRotation(startRotation,currentPosition,lastPosition,stepTime);

        return lastAngle;
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
            else
            {
                timeToCheck -= steps[s].TimeToComplate();
            }

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
    public EnemyStep AddStep(EnemyStep.MovementType movement)
    {
        EnemyStep newStep = new EnemyStep(movement);
        steps.Add(newStep);
        return newStep;
    }

    private void OnValidate()
    {
        foreach(EnemyStep step in steps)
        {
            if (step.movementSpeed < 0.5f)
                step.movementSpeed = 0.5f;

            if (step.movement == EnemyStep.MovementType.spline)
            {
                step.spline.CalculatePoints(step.movementSpeed);
            }
        }
    }
    public float TotalTime()
    {
        float result =0;
        foreach(EnemyStep step in steps)
        {
            result += step.TimeToComplate();
        }
        return result;
    }
}

    
