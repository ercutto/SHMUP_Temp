using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Craft : MonoBehaviour
{
    public CraftData craftData=new CraftData();
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
    int layerMask=0;
    public BulletSpawner[] bulletSpawner = new BulletSpawner[5];
    public Options[] options = new Options[4];
    public GameObject[] optionMarkers1= new GameObject[4];
    public GameObject[] optionMarkers2= new GameObject[4];
    public GameObject[] optionMarkers3= new GameObject[4];
    public GameObject[] optionMarkers4= new GameObject[4];

    public GameObject bombPrefeb = null;

    public Beam beam=null;
    private  void Start()
    {
       animator = GetComponent<Animator>();
        Debug.Assert(animator);

        leftBoolID = Animator.StringToHash("Left");
        rightBoolID = Animator.StringToHash("Right");

        spriteRenderer = GetComponent<SpriteRenderer>();
        Debug.Assert(spriteRenderer);

        layerMask = ~LayerMask.GetMask("PlayerBullets") & ~LayerMask.GetMask("PlayerBombs")&~LayerMask.GetMask("Player");
        craftData.beamCharge = (char)100;
    }
    private void FixedUpdate()
    {


        if (InputManager.instance && alive)
        {
            if (invunerable)
            {
                if (invunerableTimer % 12 < 6)
                {
                    spriteRenderer.material.SetColor("_Overbrigth", Color.black);
                }
                else
                {
                    spriteRenderer.material.SetColor("_Overbrigth", Color.white);

                }

                invunerableTimer--;

                if (invunerableTimer <= 0)
                {
                    invunerable = false;
                    spriteRenderer.material.SetColor("_Overbrigth", Color.black);

                }

            }

            // hit Detection hafiza kullanmiyor
            //int maxColliders = 10;
            Collider[] hits = new Collider[20];
            Vector2 halfSize = new Vector2(3f, 4f);
            
            int noOfHits = Physics.OverlapBoxNonAlloc(transform.position,halfSize,hits, Quaternion.identity,layerMask);
          
            
            if (noOfHits > 0)
            {
                Debug.Log(noOfHits + " hasar aldi");
                Hit();
               
            }
            //movement
            craftData.positionX += InputManager.instance.playerState[0].movement.x*config.speed;
            craftData.positionY += InputManager.instance.playerState[0].movement.y*config.speed;
            if (craftData.positionX < -146) craftData.positionX = -146;
            if (craftData.positionX > 146) craftData.positionX = 146;
            newPositon.x=(int)craftData.positionX;
            if (!GameManager.Instance.progressWindow)
                GameManager.Instance.progressWindow = GameObject.FindObjectOfType<LevelProgress>();
            if (GameManager.Instance.progressWindow)
                newPositon.y = (int)craftData.positionY + GameManager.Instance.progressWindow.transform.position.y;
            else
                newPositon.y = (int)craftData.positionY;
            gameObject.transform.position = newPositon;
            CheckFlames();
            //shooting
            if (InputManager.instance.playerState[playerIndex].shoot)
            {
                ShotConfiguration shotConfiguration = config.shotLevel[craftData.ShotPower];
                for (int spawner = 0; spawner < 5; spawner++)
                {
                    bulletSpawner[spawner].Shoot(shotConfiguration.spawnerSizes[spawner]);
                }
                //option array
                for(int sO=0; sO < craftData.noOfEnableOptions; sO++)
                {
                    if (options[sO])
                    {
                        options[sO].Shoot();
                    }
                }
            }
            //options
            if (!InputManager.instance.playerPrevState[playerIndex].options && InputManager.instance.playerState[playerIndex].options)
            {
                craftData.optionsLayout++;
                if (craftData.optionsLayout > 3)
                {
                    craftData.optionsLayout = (char)0;
                }

                SetOptionsLayout(craftData.optionsLayout);
            }
            //Beam
            if (InputManager.instance.playerState[playerIndex].beam)
            {
                beam.Fire();
            }

            //Bomb
            if (!InputManager.instance.playerPrevState[playerIndex].bomb && InputManager.instance.playerState[playerIndex].bomb)
            {
                //Fire bomb
                FireBomb();
            }
        }
    }
    public void Hit()
    {
        if (!invunerable)
        {
            Explode();
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
    void FireBomb()
    {
        Vector3 pos = transform.position;
        pos.y += 100;
        Instantiate(bombPrefeb, pos, Quaternion.identity);
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
    public void AddOption()
    {
        if (craftData.noOfEnableOptions < 4)
        {
            options[craftData.noOfEnableOptions].gameObject.SetActive(true);
            craftData.noOfEnableOptions++;

        }
    }

    public void SetOptionsLayout(int LayoutIndex)
    {

        Debug.Assert(LayoutIndex < 4);

        for (int o = 0; o <4; o++)
        {
            switch (LayoutIndex)
            {
                case 0:
                    options[o].gameObject.transform.position=optionMarkers1[o].transform.position;
                    options[o].gameObject.transform.rotation=optionMarkers1[o].transform.rotation;
                    break;
                case 1:
                    options[o].gameObject.transform.position = optionMarkers2[o].transform.position;
                    options[o].gameObject.transform.rotation = optionMarkers2[o].transform.rotation;
                    break;
                case 2:
                    options[o].gameObject.transform.position = optionMarkers3[o].transform.position;
                    options[o].gameObject.transform.rotation = optionMarkers3[o].transform.rotation;
                    break;
                case 3:
                    options[o].gameObject.transform.position = optionMarkers4[o].transform.position;
                    options[o].gameObject.transform.rotation = optionMarkers4[o].transform.rotation;
                    break;
            }
        }
    }

    public void IncreaseBeamStrength()
    {
        if (craftData.beamPower < 5)
        {
            craftData.beamPower++;
            UpdateBeam();
        }
    }
    void UpdateBeam()
    {
        beam.beamWidth = (craftData.beamPower + 2) * 8f;
    }
   
}
public class CraftData
{
    public float positionX;
    public float positionY;

    public char ShotPower;

    public char noOfEnableOptions;
    public char optionsLayout;
    //beam
    public bool beamFiring;
    public char beamPower; //power setting and width
    public char beamCharge; // maximum charge (upgradable)
    public char beamTimer; //current charge how much beam left
}