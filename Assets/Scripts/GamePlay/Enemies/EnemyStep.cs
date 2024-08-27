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
    public enum RotationType
    {
        INVALID,
        none,
        setAngle,
        lookAhead,
        spining,
        facePlayer,

        NOOFROTATIONS
    }
    [SerializeField]
    public RotationType rotate= RotationType.lookAhead;

    [SerializeField]
    public float endAngle = 0;

    [SerializeField]
    [Range(0.01f, 4f)]
    public float angleSpeed = 0;

    [SerializeField]
    public float noOFSpins = 0;

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
        else if (movement == MovementType.homing)
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
        else if (movement == MovementType.spline)
        {
            result += (spline.LastPoint() - spline.StartPoint());
        }
        else if (movement == MovementType.homing)
        {
           if(GameManager.Instance && GameManager.Instance.playerCrafts[0])
            {
                return GameManager.Instance.playerCrafts[0].transform.position;
            }
            else
            {
                return startPosition;
            }
        }

        Debug.LogError("EndPosition unprocces movement type, returning start ");
        return result;
    }

  

    public Vector3 CalculetePosition(Vector2 startPos,float steptime,Vector2 oldPosition,Quaternion oldAngle)
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
        else if (movement == MovementType.homing)//hoomingIn
        {
            Vector2 dir = (oldAngle * Vector2.down);
            Vector2 mov=(dir*movementSpeed);
            Vector2 pos=oldPosition+ mov;
            return pos;

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
    public float EndRotation() //TODO This not finished
    {
        return endAngle;
    }
    public Quaternion CalculateRotation(float startRotation,Vector3 currentPosition,Vector3 oldPosition, float time)
    {
        float normalisedTime = time / TimeToComplate();
        if(normalisedTime < 0)
            normalisedTime = 0;

        if (rotate == RotationType.setAngle)
        {
            Quaternion result =Quaternion.Euler(0,0,endAngle);
            return result;
        }
        else if(rotate == RotationType.spining)
        {
            float start = endAngle - (noOFSpins * 360);
            float angle= Mathf.Lerp(start,endAngle,normalisedTime);
            Quaternion result=Quaternion.Euler(0,0,angle);
            return result;
        }
        else if (rotate == RotationType.facePlayer)
        {
            float angle = 0;
            Transform target = null;
            if(GameManager.Instance&& GameManager.Instance.playerCrafts[0])
            {
                target=GameManager.Instance.playerCrafts[0].transform;
            }

            if (target != null)
            {
                Vector2 currentDir =(currentPosition-oldPosition).normalized;
                Vector2 targetDir=(target.transform.position - currentPosition).normalized;
                Vector2 newDir=Vector2.Lerp(currentDir,targetDir,angleSpeed);
                angle =Vector2.SignedAngle(Vector2.down,newDir);
            }

            return Quaternion.Euler(0,0,angle);
        }
        else if (rotate == RotationType.lookAhead)
        {
            Vector2 dir=(currentPosition - oldPosition).normalized;
            float angle=Vector2.SignedAngle(Vector2.down,dir);
            return Quaternion.Euler(0,0,angle);
        }
       


        return Quaternion.Euler(0,0,0);
    }
}
