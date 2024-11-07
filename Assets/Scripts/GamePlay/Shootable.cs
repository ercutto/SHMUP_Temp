using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shootable : MonoBehaviour
{
    public float health = 10;
    public float radiusOrHeigth = 10;
    public float hight = 10;
    public bool box = false;
    public bool polygon = false;

    public bool remainDestroyed = false;
    private bool destroyed = false;
    public int damageHealth = 5;//at what health is damage sprite displayed

    //puan toplama
    public int hitScore = 10;
    public int destroyScore = 1000;

    private Collider2D polyCollider;
    private int layerMask = 0;

    private Vector2 halfExtend;

    public bool DamageByBullets = true;
    public bool DamageByBeams = true;
    public bool DamageByBombs = true;

    public bool spawnCyclicPickUp=false;
    public PickUp[] spawnSpecificPickUp;

    public SoundEffects destroySound=null;

    private void Start()
    {
        layerMask = ~LayerMask.GetMask("Enemy") & ~LayerMask.GetMask("EnemyBullets")
            & ~LayerMask.GetMask("GroundEnemy");
        if (polygon)
        {
            polyCollider = GetComponent<Collider2D>();
            Debug.Assert(polyCollider);
        }
        else
            halfExtend = new Vector3(radiusOrHeigth / 2, hight / 2, 0);
    }

    private void FixedUpdate()
    {
        if (destroyed) return;

        int maxColliders = 10;
        Collider2D[] hits=new Collider2D[maxColliders];

        int noOfHits = 0;
        if(box)
            noOfHits = Physics2D.OverlapBoxNonAlloc(transform.position, halfExtend,0, hits,/*transform.rotation,*/ layerMask);
        else if (polygon)
        {
            ContactFilter2D contactFilter = new ContactFilter2D();
            contactFilter.useTriggers=false;
            contactFilter.SetLayerMask(layerMask);
            contactFilter.useLayerMask = true;
            noOfHits = Physics2D.OverlapCollider(polyCollider, contactFilter, hits);
        }
        else
            noOfHits = Physics2D.OverlapCircleNonAlloc(transform.position, radiusOrHeigth, hits, layerMask);

        if(noOfHits > 0)
        {
            for(int _hits = 0; _hits < noOfHits; _hits++)
            {
                if (DamageByBullets)
                {
                    Bullet b = hits[_hits].GetComponent<Bullet>();
                    if (b != null)
                    {
                        TakeDamage(1,(byte)b.playerIndex);
                        GameManager.Instance.bulletManager.DeActivateBullet(b.index);
                    }
                }
                if (DamageByBombs)
                {

                    Bomb bomb = hits[_hits].GetComponent<Bomb>();
                    if (bomb != null)
                    {

                        TakeDamage(bomb.power,bomb.playerIndex);


                    }
                }
                //if (DamageByBeams)
                //{

                //    Beam beam = hits[_hits].GetComponent<Beam>();
                //    if (beam != null)
                //    {

                //        TakeDamage(1);

                //    }
                //}

            }
        }
    }
    public void TakeDamage(int amount,byte fromPlayer)
    {
        if(destroyed) return;

        //obje hasar alinca oyuncu puan kazaniyor
        if (fromPlayer < 2)
        {
            ScoreManager.instance.ShootableHit(fromPlayer, hitScore);
        }

        health -= amount;

        EnemyPart part=GetComponent<EnemyPart>();
        if (part)
        {
            if (health <= damageHealth)
                part.Damaged(true);
            else
                part.Damaged(false);
        }

        if(health <= 0)//obje yok edildi
        {
            destroyed = true;
            if(part)
                part.Destroyed(fromPlayer);

            if(destroySound)
                destroySound.Play();

            if(fromPlayer < 2)
            {
                //obje yok edildigi icin oyuncu yok etme puani kazaniyor
                ScoreManager.instance.ShootableDestroyed(fromPlayer,destroyScore);

                GameManager.Instance.playerDatas[fromPlayer].chain++;
                ScoreManager.instance.UpdateChaninMultiplier(fromPlayer);
                GameManager.Instance.playerDatas[fromPlayer].chainTimer=PlayerData.MAXCHAINTIMER;

            }


            Vector2 pos=transform.position;
            if (spawnCyclicPickUp)
            {
                PickUp spawn = GameManager.Instance.GetNextDrop();
                GameManager.Instance.SpawnPickup(spawn, pos);
               
            }

            foreach (PickUp pickup in spawnSpecificPickUp)
            {
                GameManager.Instance.SpawnPickup(pickup, pos);
            }

            if (remainDestroyed)
                destroyed = true;
            else
                gameObject.SetActive(false);
            
        }
    }
}
