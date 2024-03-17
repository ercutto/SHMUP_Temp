using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Craft : MonoBehaviour
{
    CraftData craftData=new CraftData();
    Vector3 newPositon = new Vector3();
    [Header("Engines Effects ")]
    public GameObject aftFlame1;
    public GameObject aftFlame2;
    public GameObject leftFlame1;
    public GameObject rightFlame1;
    public GameObject frontFlame1;
    public GameObject frontFlame2;

    public int playerIndex;
    public CraftConfiguration config;
    private void FixedUpdate()
    {
        if (InputManager.instance)
        {
            craftData.positionX += InputManager.instance.playerState[0].movement.x*config.speed;
            craftData.positionY += InputManager.instance.playerState[0].movement.y*config.speed;
            newPositon.x=(int)craftData.positionX;
            newPositon.y = (int)craftData.positionY;

            gameObject.transform.position = newPositon; 
        }
    }

}

public class CraftData
{
    public float positionX;
    public float positionY;
}