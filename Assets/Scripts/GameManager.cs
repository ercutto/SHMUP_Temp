using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
   public static GameManager Instance=null;

    public bool twoPlayer = false;
    public GameObject[] craftPrefab;

    public BulletManager bulletManager=null;

    //public Craft playerOneCraft = null;
    public Craft[] playerCrafts = new Craft[2];

    public LevelProgress progressWindow = null;

    public PlayerData[] playerDatas = null;

    public Session gameSession =new Session();

    public PickUp[] cylicDrops=new PickUp[15];
    public PickUp[] Medals=new PickUp[10];
    private int currentDropIndex = 0;
    public int currentMedalIndex = 0;
    //Craft playerTwoCraft = null;
    void Start()
    {
        if(Instance)
        {
            Debug.LogError("trying to create more than one Game Manager!");
            Destroy(gameObject);
            return;
        }

        playerDatas = new PlayerData[2];
        playerDatas[0] = new PlayerData();
        playerDatas[1] = new PlayerData();

        Instance = this;    
        DontDestroyOnLoad(gameObject);
        Debug.Log("GameManager created. ");
        bulletManager = GetComponent<BulletManager>();

        Application.targetFrameRate = 60;

    }
    public void SpawnPlayer(int playerIndex,int craftType)
    {
        Debug.Assert(craftType < craftPrefab.Length);
        Debug.Log("Spawning player " + playerIndex);


        playerCrafts[playerIndex]= Instantiate(craftPrefab[craftType]).GetComponent<Craft>();
        playerCrafts[playerIndex].playerIndex=playerIndex;
    }
    public void FixedUpdate()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1)){
            if (!playerCrafts[0]) SpawnPlayer(0, 0);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (playerCrafts[0] && (int)playerCrafts[0].craftData.ShotPower < CraftConfiguration.MAX_SHOT_POWER-1)
                playerCrafts[0].craftData.ShotPower++;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
           if(bulletManager)
            {
                
               // bulletManager.SpawnBullet(BulletManager.BulletType.Bullet1_Size3,0,150,Random.Range(-10f,10), Random.Range(-10f, 10),0);
            }
        }

        if (Input.GetKeyDown(KeyCode.RightBracket))
        {
            if (playerCrafts[0])
            {
                playerCrafts[0].IncreaseBeamStrength(0);
            }
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            EnemyPattern testPattern = GameObject.FindAnyObjectByType<EnemyPattern>();
            testPattern.Spawn();
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            if (playerCrafts[0])
                playerCrafts[0].AddOption(0);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            DebugManager.Instance.ToggleHUD();   
        }

    }
    public void StartGame()
    {
        SceneManager.LoadScene("Stage01");
    }
    public void PickUpFallOffScreen(PickUp pickUp)
    {
        if (pickUp.config.type == PickUp.PickupType.Medal)
        {
            currentMedalIndex = 0;
        }
    }
    public PickUp GetNextDrop()
    {
        PickUp result = cylicDrops[currentDropIndex];

        if (result.config.type == PickUp.PickupType.Medal)
        {
            result =Medals[currentMedalIndex];
            currentMedalIndex++;
            if (currentMedalIndex > 9)
            {
                currentDropIndex = 0;
            }
        }

        currentDropIndex++;
        if (currentDropIndex > 14)
        {
            currentDropIndex = 0;
        }

        return result;
    }
}
