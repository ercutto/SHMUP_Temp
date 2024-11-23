using System;
using System.Collections;

using System.IO;
using UnityEngine;

public class Craft : MonoBehaviour
{
   
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

    public static int MAXIMUMBEAMCHARGE = 64;
    const int MAXLIVES = 5;
    const int MAXSMALLBOMBS = 8;
    const int MAXLARGEBOMBS = 5;

    SpriteRenderer spriteRenderer = null;
    int layerMask=0;
    int pickUpLayer = 0;
    public BulletSpawner[] bulletSpawner = new BulletSpawner[5];
    public Options[] options = new Options[4];
    public GameObject[] optionMarkers1= new GameObject[4];
    public GameObject[] optionMarkers2= new GameObject[4];
    public GameObject[] optionMarkers3= new GameObject[4];
    public GameObject[] optionMarkers4= new GameObject[4];

    public float screenSize = 140;
    public GameObject bombPrefeb = null;

    public Beam beam=null;
    public SoundEffects explodingNoice = null;
    public SoundEffects bombSound = null;
    private  void Start()
    {
       animator = GetComponent<Animator>();
        Debug.Assert(animator);

        leftBoolID = Animator.StringToHash("Left");
        rightBoolID = Animator.StringToHash("Right");

        spriteRenderer = GetComponent<SpriteRenderer>();
        Debug.Assert(spriteRenderer);

        layerMask = ~LayerMask.GetMask("PlayerBullets") &
                    ~LayerMask.GetMask("PlayerBombs")&
                    ~LayerMask.GetMask("Player")&
                    ~LayerMask.GetMask("GroundEnemy");
        pickUpLayer = LayerMask.NameToLayer("PickUp");
       
    }
    private void FixedUpdate()
    {
        CraftData craftData = GameManager.Instance.gameSession.craftDatas[playerIndex];

        if (InputManager.instance && alive)
        {
            //chain Drop
            if (GameManager.Instance.playerDatas[playerIndex].chainTimer > 0)
            {
                GameManager.Instance.playerDatas[playerIndex].chainTimer--;
                if (GameManager.Instance.playerDatas[playerIndex].chainTimer == 0)
                {
                    GameManager.Instance.playerDatas[playerIndex].chain = 0;
                    ScoreManager.instance.UpdateChaninMultiplier(playerIndex);
                }
            }

            //invunerable flashing
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
            Collider2D[] hits = new Collider2D[20];
            Vector2 halfSize = new Vector2(3f, 4f);
            //bullets hits
            int noOfHits = Physics2D.OverlapBoxNonAlloc(transform.position,halfSize,0,hits,layerMask);

            if (noOfHits > 0)
            {
                foreach (Collider2D hit in hits)
                {
                    if (hit)
                    {
                        if (hit.gameObject.layer != pickUpLayer)
                            Hit();


                    }


                }
            }
            //pickup hits and bullet Grazing
            halfSize = new Vector2(15f, 21f);
            noOfHits = Physics2D.OverlapBoxNonAlloc(transform.position, halfSize,0, hits, layerMask);

            if (noOfHits > 0)
            {
                foreach (Collider2D hit in hits)
                {
                    if (hit)
                    {
                        if (hit.gameObject.layer == pickUpLayer)
                        {
                            PickingUp(hit.GetComponent<PickUp>());
                        }
                        else//bullet Graze
                        if(craftData.beamCharge<MAXIMUMBEAMCHARGE)
                        {
                            craftData.beamCharge++;
                            craftData.beamTimer++;
                        }

                    }


                }
            }


            //movement
            craftData.positionX += InputManager.instance.playerState[0].movement.x*config.speed;
            craftData.positionY += InputManager.instance.playerState[0].movement.y*config.speed;
            if (craftData.positionX < -screenSize) craftData.positionX = -screenSize;
            if (craftData.positionX > screenSize) craftData.positionX = screenSize;
            
            if (craftData.positionY < -180) craftData.positionY = -180;
            if (craftData.positionY > 180) craftData.positionY = 180;

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
                    craftData.optionsLayout = (byte)0;
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
    public void PickingUp(PickUp pickUp)
    {
        CraftData craftData = GameManager.Instance.gameSession.craftDatas[playerIndex];
        if (pickUp)
        {
            pickUp.ProcessPickUp(playerIndex, craftData);
        }
    }
    public void Hit()
    {
        if (!invunerable&&!GameManager.Instance.gameSession.invincible)
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
        CraftData craftData = GameManager.Instance.gameSession.craftDatas[playerIndex];
        if (craftData.smallBombs>0) {
            craftData.smallBombs--;
            Vector3 pos = transform.position;
            pos.y += 100;
            if(bombSound)
                bombSound.Play();
            Bomb bomb= Instantiate(bombPrefeb, pos, Quaternion.identity).GetComponent<Bomb>();
            if(bomb)
                bomb.playerIndex=(byte)playerIndex;
        }
    }
    public void PowerUp(byte powerLevel,int surplusValue)
    {
        CraftData craftData = GameManager.Instance.gameSession.craftDatas[playerIndex];
        craftData.ShotPower += powerLevel;
        if (craftData.ShotPower > 9)
        {
            ScoreManager.instance.PickupCollected(playerIndex,surplusValue);
            craftData.ShotPower = 9;
        }
    }
    public void OneUp(int surplusValue)
    {
        GameManager.Instance.playerDatas[playerIndex].lives++;
        if (GameManager.Instance.playerDatas[playerIndex].lives > MAXLIVES)
        {
            ScoreManager.instance.PickupCollected(playerIndex, surplusValue);
            GameManager.Instance.playerDatas[playerIndex].lives = MAXLIVES;
        }
    }
    public void AddBomb(int power,int surplusValue)
    {
        CraftData craftData = GameManager.Instance.gameSession.craftDatas[playerIndex];
        if (power == 1)
        {
            craftData.smallBombs++;
            if(craftData.smallBombs > MAXSMALLBOMBS)
            {
                craftData.smallBombs = MAXSMALLBOMBS;
                ScoreManager.instance.PickupCollected(playerIndex, surplusValue);
            }

        } else if (power == 2)
        {
            craftData.largeBombs++;
            if (craftData.largeBombs > MAXLARGEBOMBS)
            {
                craftData.largeBombs = MAXLARGEBOMBS;
                ScoreManager.instance.PickupCollected(playerIndex, surplusValue);

            }
        }
        else
        {
            Debug.LogError("Invalid bomb power pickup");
        }
    }
    public void AddMedal(int level,int value)
    {
        ScoreManager.instance.MedalCollected(playerIndex,value);
        //IncreaseScore(value);
    }
    public void IncreaseScore(int value)
    {
        GameManager.Instance.playerDatas[playerIndex].score += value;
        GameManager.Instance.playerDatas[playerIndex].stageScore += value;
    }
    public void Explode()
    {
        alive = false;
        GameManager.Instance.playerDatas[playerIndex].lives--;
        StartCoroutine(Exploding());

        if(explodingNoice)
            explodingNoice.Play();

       
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
        GameManager.Instance.playerCrafts[playerIndex] = null;
        Destroy(gameObject);


        bool allLivesGone = false;
        if (GameManager.Instance.twoPlayer)
        {
            if ((GameManager.Instance.playerDatas[playerIndex].lives == 0)&& (GameManager.Instance.playerDatas[playerIndex].lives == 0))
            {
                allLivesGone = true;
            }
        }
        else
        {
            if (GameManager.Instance.playerDatas[playerIndex].lives == 0)
            {
                allLivesGone = true;
            }
        }


        if (allLivesGone)
        {
            GameOverMenu.instance.GameOver();
          
        }
        else
        {
            //// Eject powerUps and spawn next life
            CraftData craftData = GameManager.Instance.gameSession.craftDatas[playerIndex];
            int noOfOptionsToReSapwn = craftData.noOfEnableOptions - 1;
            int noOfPowrUpsToReSpawn = craftData.ShotPower - 1;
            int noOfBeamUpsToReSoawn = craftData.beamPower - 1;
            GameManager.Instance.ResetState(playerIndex);

            //for (int o = 0; o < noOfOptionsToReSapwn; o++)
            //{
                
                
            //    PickUp pickUp = GameManager.Instance.SpawnPickup(GameManager.Instance.option, transform.position);
            //    pickUp.transform.position += new Vector3(UnityEngine.Random.Range(-128, 128), UnityEngine.Random.Range(-128, 128), 0);
            //}

            //for (int o = 0; o < noOfPowrUpsToReSpawn; o++)
            //{
            //    PickUp pickUp = GameManager.Instance.SpawnPickup(GameManager.Instance.powrup, transform.position);
            //    pickUp.transform.position += new Vector3(UnityEngine.Random.Range(-128, 128), UnityEngine.Random.Range(-128, 128), 0);
            //}

            //for (int o = 0; o < noOfBeamUpsToReSoawn; o++)
            //{
            //    PickUp pickUp = GameManager.Instance.SpawnPickup(GameManager.Instance.beamup, transform.position);
            //    pickUp.transform.position += new Vector3(UnityEngine.Random.Range(-128, 128), UnityEngine.Random.Range(-128, 128), 0);
            //}

            if (GameManager.Instance.playerDatas[playerIndex].lives>0)
                GameManager.Instance.DelayedRespawn(playerIndex);
           
            
        }

        yield return null;
    }

    public void Invunerable()
    {
        invunerable = true;
        invunerableTimer = INVUNERABLELENGTH;
    }
    public void AddOption(int surplusValue)
    {
        CraftData craftData = GameManager.Instance.gameSession.craftDatas[playerIndex];
        if (craftData.noOfEnableOptions < 4)
        {
            options[craftData.noOfEnableOptions].gameObject.SetActive(true);
            craftData.noOfEnableOptions++;

        }
        else
        {
            ScoreManager.instance.PickupCollected(playerIndex, surplusValue);
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

    public void IncreaseBeamStrength(int surplusValue)
    {
        CraftData craftData = GameManager.Instance.gameSession.craftDatas[playerIndex];

        if (craftData.beamPower < 5)
        {
            craftData.beamPower++;
            UpdateBeam();
        }
        else
        {
            ScoreManager.instance.PickupCollected(playerIndex, surplusValue);
        }
    }
    void UpdateBeam()
    {
        CraftData craftData = GameManager.Instance.gameSession.craftDatas[playerIndex];

        beam.beamWidth = (craftData.beamPower + 2) * 8f;
    }
   
}
[Serializable]
public class CraftData
{
    public float positionX;
    public float positionY;

    public byte ShotPower;

    public byte noOfEnableOptions;
    public byte optionsLayout;
    //beam
    public bool beamFiring;
    public byte beamPower; //power setting and width
    public byte beamCharge; // picked by charge
    public byte beamTimer;//current charge how much beam left
    public byte smallBombs;
    public byte largeBombs;

    public void Save(BinaryWriter writer)
    {
        writer.Write(ShotPower);
        writer.Write(noOfEnableOptions);
        writer.Write(optionsLayout);

        writer.Write(beamPower);
        writer.Write(beamCharge);

        writer.Write(smallBombs);
        writer.Write(largeBombs);
    }
    public void Load(BinaryReader reader)
    {  
        ShotPower = reader.ReadByte();

        noOfEnableOptions = reader.ReadByte();

        optionsLayout = reader.ReadByte();

        beamPower = reader.ReadByte();
        beamCharge = reader.ReadByte();

        smallBombs = reader.ReadByte();
        largeBombs = reader.ReadByte();
     
     

    }
}