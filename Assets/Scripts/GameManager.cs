using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
   public static GameManager Instance=null;

    public bool twoPlayer = false;
    public GameObject[] craftPrefab;

    

    Craft playerOneCraft = null;
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
        if(Input.GetKeyDown(KeyCode.Space)) {
            if (!playerOneCraft) SpawnPlayer(1, 0);
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            if (playerOneCraft) playerOneCraft.Explode();
        }
    }
}
