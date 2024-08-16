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
    public Spline spline;
    [SerializeField]
    [Range(0.1f, 20.0f)]
    public float movementSpeed = 4f;
    [SerializeField]
    public float framesToWait = 30;
    
    public List<String> activateStates =new List<String>();
    public List<String> deActivateStates =new List<String>();

    public EnemyStep(MovementType inMovement)
    {
        movement = inMovement;
        direction =Vector2.zero;
        if (inMovement == MovementType.spline)
        {
            spline = new Spline();
        }

    }

    public float TimeToComplate()
    {
        if (movement == MovementType.direction)
        {
            float timeToTravel = direction.magnitude / movementSpeed;
            return timeToTravel;
        }
        else if (movement == MovementType.none)
        {
            return framesToWait;
        }
        else if (movement == MovementType.spline)
        {
            return spline.Length()/movementSpeed;
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
        else if (movement == MovementType.spline)
        {
            result += (spline.LastPoint() - spline.StartPoint());
        }

        Debug.LogError("EndPosition unprocces movement type, returning start ");
        return result;
    }

    public Vector3 CalculetePosition(Vector2 startPos,float steptime)
    {
        float normalisedTime=steptime/TimeToComplate();
        if(normalisedTime < 0 )
            normalisedTime = 0;

        if (movement == MovementType.direction)
        {
            float timeToTravel = direction.magnitude / movementSpeed;
            float ration = 0;
            if (timeToTravel != 0)
                ration = steptime / timeToTravel;

            Vector2 place = startPos + (direction * ration);
            return place;
        }
        else if (movement == MovementType.none)
        {
            return startPos;
        }
        else if (movement == MovementType.spline)
        {
            return spline.Getpositon(normalisedTime)+startPos;
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
