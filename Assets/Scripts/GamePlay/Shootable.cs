using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shootable : MonoBehaviour
{
    public float health = 10;
    public float radius = 10;
    private int layerMask = 0;
    private void Start()
    {
        layerMask = ~LayerMask.GetMask("Enemy") & ~LayerMask.GetMask("EnemyBullets");
    }

    private void FixedUpdate()
    {
        int maxColliders = 10;
        Collider[] hits=new Collider[maxColliders];

        int noOfHits = Physics.OverlapSphereNonAlloc(transform.position, radius, hits, layerMask);
        if(noOfHits > 0)
        {
            for(int _hits = 0; _hits < noOfHits; _hits++)
            {
               
                Bullet b = hits[_hits].GetComponent<Bullet>();
                if (b != null)
                {
                    TakeDamage(1);
                    GameManager.Instance.bulletManager.DeActivateBullet(b.index);
                }
                else
                {

                    Bomb bomb = hits[_hits].GetComponent<Bomb>();
                    if (bomb != null)
                    {
                       
                        TakeDamage(bomb.power);

                    }
                }
                
            }
        }
    }
    public void TakeDamage(int amount)
    {
        health -= amount;
        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
