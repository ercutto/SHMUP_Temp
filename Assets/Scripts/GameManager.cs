using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
   public static GameManager Instance=null;

    public bool twoPlayer = false;
    public GameObject[] craftPrefab;

    private BulletManager bulletManager=null;

    public Craft playerOneCraft = null;
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

    }
    public void SpawnPlayer(int playerIndex,int craftType)
    {
        Debug.Assert(craftType < craftPrefab.Length);
        Debug.Log("Spawning player " + playerIndex);

        
        playerOneCraft= Instantiate(craftPrefab[craftType]).GetComponent<Craft>();
        playerOneCraft.playerIndex=playerIndex;
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha0)){
            if (!playerOneCraft) SpawnPlayer(0, 0);
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            if (playerOneCraft) playerOneCraft.Explode();
        }

        if (Input.GetKey(KeyCode.S))
        {
           if(bulletManager)
            {
                bulletManager.SpawnBullet(BulletManager.BulletType.Bullet1_Size3,0,150,Random.Range(-10f,10), Random.Range(-10f, 10),0);
            }
        }
    }
}
