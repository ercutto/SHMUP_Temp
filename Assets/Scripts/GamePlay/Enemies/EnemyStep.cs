using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class EnemyStep 
{
   public enum MovementType
    {
        INVALID,

        none,//waiting at a position
        direction,
        spline,
        atTarget,
        homing,
        follow,
        circle,

        NOOFMOVEMENTTYPES
    }
    [SerializeField]
    public MovementType movement;
    [SerializeField]
    public Vector2 direction;
    [SerializeField]
    [Range(0, 20)]
    public float movementSpeed = 4f;
    [SerializeField]
    public float framesToWait = 30;
    
    public List<String> activateStates =new List<String>();
    public List<String> deActivateStates =new List<String>();

    public float TimeToComplate()
    {
        if(movement == MovementType.direction)
        {
            float timeToTravel = direction.magnitude / movementSpeed;
            return timeToTravel;
        }
        else if(movement == MovementType.none)
        {
            return framesToWait;
        }

        Debug.LogError("TimeToComplete unprocces movement type, returning 1 ");
        return 1;
    }

   public Vector2 EndPosition(Vector3 startPosition)
    {
        Vector2 result = startPosition;
        if(movement == MovementType.direction)
        {
            result += direction;
            return result;
        }
        else if (movement == MovementType.none)
        {
            return startPosition;
        }

            Debug.LogError("EndPosition unprocces movement type, returning start ");
        return result;
    }

    public Vector3 CalculetePosition(Vector2 startPos,float steptime)
    {
        if (movement == MovementType.direction)
        {
            float timeToTravel=direction.magnitude / movementSpeed;
            float ration = steptime / timeToTravel;

            Vector2 place =startPos +(direction * ration);
            return place;
        }
        else if (movement == MovementType.none)
        {
            return startPos;
        }

            Debug.LogError("CalculetePosition unprocces movement type, returning startPosition ");
        return startPos;
    }
    public void FireActivateStates(Enemy enemy)
    {
        Debug.Log("Fire activate State:");

        foreach (string state in activateStates)
        {
           
            enemy.EnableState(state);
           
        }
        
    }
    public void FireDeactivateStates(Enemy enemy)
    {
        Debug.Log("Fire Deactivate State:");
        foreach (string state in deActivateStates)
        {
            enemy.DisableState(state);

            
        }
    }
}
