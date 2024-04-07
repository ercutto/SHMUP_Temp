using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beam : MonoBehaviour
{
    public  LineRenderer lineRenderer = null;
    public float beamWidth = 20f;
    public Craft craft = null;

    void Start()
    {
       
        Debug.Assert(lineRenderer);
    }

    public void Fire()
    {
        if (!craft.craftData.beamFiring)
        {
            craft.craftData.beamFiring=true;
            craft.craftData.beamTimer=craft.craftData.beamCharge;
            UpdateBeam();
            gameObject.SetActive(true);
        }
    }
    private void FixedUpdate()
    {
        UpdateBeam();
    }
    void UpdateBeam()
    {
        craft.craftData.beamTimer--;
        if(craft.craftData.beamTimer == 0)
        {
            craft.craftData.beamFiring = false;
            gameObject.SetActive(false);
        }

        lineRenderer.startWidth = beamWidth;
        lineRenderer.endWidth = beamWidth;

        lineRenderer.SetPosition(0,transform.position);
        Vector3 top = transform.position;
        top.y = 180;
        lineRenderer.SetPosition(1,top);

    }
}
