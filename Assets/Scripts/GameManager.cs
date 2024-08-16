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

    public Craft playerOneCraft = null;

    public LevelProgress progressWindow = null;

    public Session gameSession =new Session();
    //Craft playerTwoCraft = null;
    void Start()
    {
        if(Instance)
        {
            Debug.LogError("trying to create more than one Game Manager!");
            Destroy(gameObject);
            return;
        }

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

        
        playerOneCraft= Instantiate(craftPrefab[craftType]).GetComponent<Craft>();
        playerOneCraft.playerIndex=playerIndex;
    }
    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha0)){
            if (!playerOneCraft) SpawnPlayer(0, 0);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (playerOneCraft &&(int)playerOneCraft.craftData.ShotPower < CraftConfiguration.MAX_SHOT_POWER-1)
                playerOneCraft.craftData.ShotPower++;
        }

        if (Input.GetKey(KeyCode.S))
        {
           if(bulletManager)
            {
                
               // bulletManager.SpawnBullet(BulletManager.BulletType.Bullet1_Size3,0,150,Random.Range(-10f,10), Random.Range(-10f, 10),0);
            }
        }

        if (Input.GetKeyDown(KeyCode.RightBracket))
        {
            if (playerOneCraft)
            {
                playerOneCraft.IncreaseBeamStrength();
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            EnemyPattern testPattern = GameObject.FindAnyObjectByType<EnemyPattern>();
            testPattern.Spawn();
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            if (playerOneCraft )
                playerOneCraft.AddOption();
        }


    }
    public void StartGame()
    {
        SceneManager.LoadScene("Stage01");
    }
}
