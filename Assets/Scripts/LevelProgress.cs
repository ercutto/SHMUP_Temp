using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelProgress : MonoBehaviour
{
    public ProgressData data;
    public int levelSize;
    public AnimationCurve speedCurve;
    public GameObject midGroudTileGrid=null;
    public float midgroundRate= 0.75f;

    private Craft player1Craft = null;
    public bool disableMovement = false;
    // Start is called before the first frame update
    void Start()
    {
        data.positionX=transform.position.x;
        data.positionY=transform.position.y;

        if (GameManager.Instance)
        {
            GameManager.Instance.progressWindow = this;
        }

       
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CraftData craftdata = GameManager.Instance.gameSession.craftDatas[0];

        if (data.progress < levelSize)
        {
            if (player1Craft == null)
                 player1Craft = GameManager.Instance.playerCrafts[0];
            if (player1Craft && !disableMovement)
            {
                float ratio = (float)data.progress / (float)levelSize;
                float movement = speedCurve.Evaluate(ratio);
                data.progress++;
                //UpdateProgressWindow(player1Craft.craftData.positionX, movement);
                UpdateProgressWindow(craftdata.positionX,movement);
            }

        }

    }

    void UpdateProgressWindow(float shipX,float movement)
    {
        data.positionX = shipX / 10f;
        data.positionY+=movement;
        transform.position =new Vector3(data.positionX, data.positionY, 0);
        midGroudTileGrid.transform.position = new Vector3(0,data.positionY*midgroundRate,0);
    }
}

[Serializable]
public class ProgressData
{
    public int progress;
    public float positionX;
    public float positionY;
}


