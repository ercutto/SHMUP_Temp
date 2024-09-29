using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class EnemyBeam : MonoBehaviour
{
    public LineRenderer lineRenderer = null;
    public float beamWidth = 10f;
    public GameObject beamFlash = null;
    private bool firing=false;
    private int layerMask;
    public GameObject endPoint = null;
    private bool charging=false;
    private const int FULL_CHARGE_TIMER = 60;
    private int chargeTimer=FULL_CHARGE_TIMER;
    void Start()
    {

        layerMask = ~LayerMask.GetMask("Enemy") & ~LayerMask.GetMask("enemyBullets");
    }

    public void Fire()
    {
        if (!firing)
        {
            firing = true;
            charging = true;
            
            UpdateBeam();
           
         
            gameObject.SetActive(true);
          
        }
    }
    private void FixedUpdate()
    {
        if (firing)
            UpdateBeam();
    }

    public void StopFiring()
    {
        firing=false;
        charging=false;
        gameObject.SetActive(false);
        if (beamFlash)
            beamFlash.SetActive(false);
    }
   
    void UpdateBeam()
    {

        if (!charging)
        {
            int maxColliders = 20;
            Collider2D[] hits = new Collider2D[maxColliders];
            Vector2 center = Vector2.Lerp(transform.position, endPoint.transform.position, 0.5f);
            Vector2 halfsize = new Vector2(beamWidth * 0.5f, (endPoint.transform.position - transform.position).magnitude * 0.5f);

            int noOfHits = Physics2D.OverlapBoxNonAlloc(center, halfsize, transform.eulerAngles.z, hits, layerMask);
            for (int i = 0; i < noOfHits; i++)
            {
                Craft craft = hits[i].GetComponent<Craft>();
                if (craft)
                {
                    //endPoint.transform.position=craft.transform.position;
                    craft.Hit();
                }
            }

            lineRenderer.startWidth = beamWidth;
            lineRenderer.endWidth = beamWidth;
        }
        else
        {
            
            lineRenderer.startWidth = 1;
            lineRenderer.endWidth = 1;
            chargeTimer--;
            if (chargeTimer <= 0)
            {
                charging = false;
                chargeTimer=FULL_CHARGE_TIMER;

                if (beamFlash)
                {
                    beamFlash.SetActive(true);
                }
            }
           
        }

        lineRenderer.SetPosition(0, transform.position);

        lineRenderer.SetPosition(1, endPoint.transform.position);
    }
}
