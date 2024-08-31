using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shootable : MonoBehaviour
{
    public float health = 10;
    public float radiusOrHeigth = 10;
    public float hight = 10;
    public bool box = false;
    private int layerMask = 0;

    private Vector2 halfExtend;

    public bool DamageByBullets = true;
    public bool DamageByBeams = true;
    public bool DamageByBombs = true;

    public bool spawnCyclicPickUp=false;
    public PickUp[] spawnSpecificPickUp;

    private void Start()
    {
        layerMask = ~LayerMask.GetMask("Enemy") & ~LayerMask.GetMask("EnemyBullets")
            & ~LayerMask.GetMask("GroundEnemy");
        halfExtend = new Vector3(radiusOrHeigth / 2, hight / 2, 0);
    }

    private void FixedUpdate()
    {
        int maxColliders = 10;
        Collider[] hits=new Collider[maxColliders];

        int noOfHits = 0;
        if(box)
            noOfHits = Physics.OverlapBoxNonAlloc(transform.position, halfExtend, hits,transform.rotation, layerMask);
        else
            noOfHits = Physics.OverlapSphereNonAlloc(transform.position, radiusOrHeigth, hits, layerMask);

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
        health -= amount;
        if(health <= 0)
        {
            if(fromPlayer < 2)
            {
                GameManager.Instance.playerDatas[fromPlayer].chain++;
                GameManager.Instance.playerDatas[fromPlayer].chainTimer=PlayerData.MAXCHAINTIMER;

            }


            Vector2 pos=transform.position;
            if (spawnCyclicPickUp)
            {
                PickUp spawn = GameManager.Instance.GetNextDrop();
                PickUp p =Instantiate(spawn,pos,Quaternion.identity);
                if (p)
                {
                    p.transform.SetParent(GameManager.Instance.transform);
                }
            }

            foreach (PickUp pickup in spawnSpecificPickUp) {
                PickUp p=Instantiate(pickup,pos,Quaternion.identity);
                if (p)
                {
                    p.transform.SetParent(GameManager.Instance.transform);
                }
                else
                {
                    Debug.LogError("Failed to spawn pickup!");
                }
            }

            Destroy(gameObject);
        }
    }
}
