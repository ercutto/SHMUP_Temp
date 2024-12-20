
using UnityEngine;

public class RotateToFace : MonoBehaviour
{
    public bool facePlayer = false;
    public bool faceTarget = false;
    public GameObject target = null;
    private bool active = false;
    public bool isBoss = false;

    // Update is called once per frame
    void FixedUpdate()
    {
        
        if (active||isBoss)
        {
           
            if (facePlayer || faceTarget)
            {
                Vector2 targetPosition = Vector2.zero;

                if (facePlayer)
                {
                    if(!GameManager.Instance||!GameManager.Instance.playerCrafts[0]) 
                        return;

                    targetPosition=GameManager.Instance.playerCrafts[0].transform.position;
                    
                }
                else if (faceTarget)
                {
                    if(target==null)
                        return;

                    targetPosition=target.transform.position;
                }

               
                Vector2 direction=(Vector2)transform.position - targetPosition;
                direction.Normalize();
                transform.rotation = Quaternion.LookRotation(Vector3.forward,direction);
            }
        }
    }
    public void Activate()
    {
        active = true;
    }
    public void Deactivate() 
    {
        active = false;
    }
}
