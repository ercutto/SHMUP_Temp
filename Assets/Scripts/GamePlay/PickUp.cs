using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class PickUp : MonoBehaviour
{
   public enum PickupType
    {
        INVALID,

        Bomb,
        Coin,
        PowerUp,
        BeamUp,
        Options,
        Medal,
        Secret,
        Lives,

        NOOFPICKUPTYPES
    };

    public PickUpConfig config;

    public Vector2 position;
    public Vector2 velocity;

    public SoundEffects sounds = null;
    public void OnEnable()
    {
        position = transform.position;
        velocity.x = Random.Range(-4, 4);
        velocity.y = Random.Range(-4, 4);
    }

    private void FixedUpdate()
    {
        //move
        position += velocity;
        velocity /= 1.3f;

        position.y -= config.fallSpeed;
        if (GameManager.Instance && GameManager.Instance.progressWindow)
        {
            float posY=position.y -GameManager.Instance.progressWindow.transform.position.y;
            if(posY<-180)//off screen
            {
                GameManager.Instance.PickUpFallOffScreen(this);
                Destroy(gameObject);
                return;
            }
        }
        transform.position = position;
    }
    public void ProcessPickUp(int playerIndex, CraftData craftData)
    {
        if(sounds)
            sounds.Play();

        switch (config.type)
        {
            case PickupType.Coin:
                {
                    ScoreManager.instance.PickupCollected(playerIndex, config.coinValue);
                    //GameManager.Instance.playerCrafts[playerIndex].IncreaseScore(config.coinValue);
                    break;

                }
            default:
                {
                    Debug.LogError("Unprocessed config type: " + config.type);
                    break;
                }
            case PickupType.PowerUp:
                {
                    GameManager.Instance.playerCrafts[playerIndex].PowerUp((byte)config.powerLevel,config.surplusValue);
                    break;
                }
            case PickupType.Lives:
                {
                    GameManager.Instance.playerCrafts[playerIndex].OneUp(config.surplusValue);
                    break;
                }
            case PickupType.Secret:
                {
                    ScoreManager.instance.PickupCollected(playerIndex, config.coinValue);
                    //GameManager.Instance.playerCrafts[playerIndex].IncreaseScore(config.coinValue);
                    break;
                }
            case PickupType.BeamUp:
                {
                    GameManager.Instance.playerCrafts[playerIndex].IncreaseBeamStrength(config.surplusValue);
                    break;
                }
            case PickupType.Options:
                {
                    GameManager.Instance.playerCrafts[playerIndex].AddOption(config.surplusValue);
                    break;
                }
            case PickupType.Bomb:
                {
                    GameManager.Instance.playerCrafts[playerIndex].AddBomb(config.bombPower,config.surplusValue);
                    break;
                }
            case PickupType.Medal:
                {
                    GameManager.Instance.playerCrafts[playerIndex].AddMedal(config.medalLevel,config.medalvalue);
                    break;
                }

        }

        Destroy(gameObject);
    }
}
