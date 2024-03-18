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

    [Header("Animation Variables")]

    Animator animator;
    int leftBoolID;
    int rightBoolID;


    public int playerIndex;
    public CraftConfiguration config;

    private void Start()
    {
       animator = GetComponent<Animator>();
        Debug.Assert(animator);

        leftBoolID = Animator.StringToHash("Left");
        rightBoolID = Animator.StringToHash("Right");
    }
    private void FixedUpdate()
    {
        if (InputManager.instance)
        {
            craftData.positionX += InputManager.instance.playerState[0].movement.x*config.speed;
            craftData.positionY += InputManager.instance.playerState[0].movement.y*config.speed;
            newPositon.x=(int)craftData.positionX;
            newPositon.y = (int)craftData.positionY;

            gameObject.transform.position = newPositon;
            CheckFlames();
        }
    }
    void CheckFlames()
    {
        if (InputManager.instance.playerState[0].up)
        {
            aftFlame1.SetActive(true);
            aftFlame2.SetActive(true);
        }
        else
        {
            aftFlame1.SetActive(false);
            aftFlame2.SetActive(false);

        }

        if (InputManager.instance.playerState[0].down)
        {
            frontFlame1.SetActive(true);
            frontFlame2.SetActive(true);
        }
        else
        {
            frontFlame1.SetActive(false);
            frontFlame2.SetActive(false);

        }

        if (InputManager.instance.playerState[0].right)
        {
            leftFlame1.SetActive(true);
            animator.SetBool(rightBoolID, true);

        }
        else
        {
            leftFlame1.SetActive(false);
            animator.SetBool(rightBoolID, false);


        }

        if (InputManager.instance.playerState[0].left)
        {
            rightFlame1.SetActive(true);
            animator.SetBool(leftBoolID, true);
        }
        else
        {
            rightFlame1.SetActive(false);
            animator.SetBool(leftBoolID,false);


        }
    }

    public void Explode()
    {
        EffectSystem.instance.CraftExplosion(transform.position);
        Destroy(gameObject);
    }

}

public class CraftData
{
    public float positionX;
    public float positionY;
}