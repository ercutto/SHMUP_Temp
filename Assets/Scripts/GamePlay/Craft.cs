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

    bool alive = true;
    bool invunerable=true;

    int invunerableTimer = 120;
    const int INVUNERABLELENGTH = 120;
    public CraftConfiguration config;

    SpriteRenderer spriteRenderer = null;

    private void Start()
    {
       animator = GetComponent<Animator>();
        Debug.Assert(animator);

        leftBoolID = Animator.StringToHash("Left");
        rightBoolID = Animator.StringToHash("Right");

        spriteRenderer = GetComponent<SpriteRenderer>();
        Debug.Assert(spriteRenderer);
    }
    private void FixedUpdate()
    {
        if (invunerable)
        {
            if(invunerableTimer%12<6)
            {
                spriteRenderer.material.SetColor("_Overbrigth", Color.black);
            }
            else
            {
                spriteRenderer.material.SetColor("_Overbrigth", Color.white);

            }

            invunerableTimer--;

            if(invunerableTimer <= 0)
            {
                invunerable = false;
                spriteRenderer.material.SetColor("_Overbrigth", Color.black);

            }

        }

        if (InputManager.instance && alive)
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
        alive = false;
        StartCoroutine(Exploding());
    }

    IEnumerator Exploding()
    {
        Color col=Color.white;
        for (float redness=0;redness<=1;redness+=0.3f)
        {
            col.g = 1-redness;
            col.b = 1-redness;
            spriteRenderer.color = col;

            yield return new WaitForSeconds(0.1f);
           
        }
        EffectSystem.instance.CraftExplosion(transform.position);
        Destroy(gameObject);
        GameManager.Instance.playerOneCraft = null;
        yield return null;
    }

    public void Invunerable()
    {
        invunerable = true;
        invunerableTimer = INVUNERABLELENGTH;
    }
}

public class CraftData
{
    public float positionX;
    public float positionY;
}