using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyPart : MonoBehaviour
{
    [HideInInspector]
    public bool destroyed=false;
    [HideInInspector]
    public bool damaged = false;
    public bool usingDamageSprite = false;
    public Sprite damageVersion = null;
    public Sprite destroyedVersion = null;

    public UnityEvent triggerDestroyed;

    public void Destroyed()
    {
        if(destroyed)return;

        triggerDestroyed.Invoke();

        if (destroyedVersion)
        {
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            
            if (spriteRenderer)
            {
                spriteRenderer.sprite = destroyedVersion;
            }
        }

        destroyed = true;
        Enemy enemy=transform.root.GetComponent<Enemy>();
        if (enemy)
            enemy.PartDestroyed();
    }

    public void Damaged(bool switchToDamageSprite)
    { 
        if (destroyed) return;

        

        if (switchToDamageSprite && !usingDamageSprite)
        {
           
            if (damageVersion)
            {
                SpriteRenderer spriteRenderer=GetComponent<SpriteRenderer>();
                if (spriteRenderer)
                { 
                    spriteRenderer.sprite = damageVersion;
                }
            }
            usingDamageSprite = true;
        }    
        damaged = true;
    }
}
