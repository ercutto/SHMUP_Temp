using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    public BulletManager.BulletType bulletType = BulletManager.BulletType.Bullet1_Size1;
    public BulletSequence sequence;
    [Header("Bullet Frames")]
    public int rate = 1;
    public int speed = 10;
    public int timer = 0;
    public GameObject muzzleFlash = null;

    [Header("Shooting direction and amount")]
    public float startAngle = 0;
    public float endAngle = 0;
    public int radialNumber=1;

    public float dAngle = 0;
    private bool firing=false;
    private int frame = 0;
    //For Enemy Ships
    public bool autoFireActive=false;

    public bool fireAtPlayer=false;
    public bool fireAtTarget=false;
    public GameObject target = null;

    //ekleme 
    public bool hedefeAtes=false;

    public bool hooming = false;
    public bool isPlayer = false;

    public void Shoot(int size)
    {
        if (size < 0) return;

        if (!isPlayer)
        {
            float x =transform.position.x;
            if (GameManager.Instance && GameManager.Instance.progressWindow)
                x-=GameManager.Instance.progressWindow.data.positionX;
            if(x<-110||x>110)
                return;
            
            float y =transform.position.y;
            if (GameManager.Instance && GameManager.Instance.progressWindow)
                y -= GameManager.Instance.progressWindow.data.positionY;
            if(y<-100||y>180)
                return;
        }
        Vector2 primaryDirection = transform.up;
       
        if (fireAtPlayer || fireAtTarget)
        {
            Vector2 targetPosition = Vector2.zero;
            
            if (fireAtPlayer && target != null)
            {
                    
                targetPosition = GameManager.Instance.playerOneCraft.transform.position;
                
            }
            else if (fireAtTarget && target != null)
            {
               
                targetPosition = target.transform.position;
            }
          

            primaryDirection = targetPosition - (Vector2)transform.position;
            primaryDirection.Normalize();
        }
        

        if (firing||timer == 0)
        {
            float angle = startAngle;
            if (hedefeAtes)
            {
                angle = transform.rotation.eulerAngles.z;
                Quaternion myRotatioton = Quaternion.AngleAxis(angle, transform.forward);
                Vector2 velocity = myRotatioton * primaryDirection * speed;

                BulletManager.BulletType bulletToShoot = bulletType + size;
                GameManager.Instance.bulletManager.SpawnBullet(bulletToShoot, transform.position.x, transform.position.y, velocity.x, velocity.y, angle, dAngle, hooming);
            }
            else
            {
                for (int a = 0; a < radialNumber; a++)
                {
                    Quaternion myRotatioton = Quaternion.AngleAxis(angle, Vector3.forward);
                    Vector2 velocity = myRotatioton * primaryDirection * speed;

                    BulletManager.BulletType bulletToShoot = bulletType + size;
                    GameManager.Instance.bulletManager.SpawnBullet(bulletToShoot, transform.position.x, transform.position.y, velocity.x, velocity.y, angle, dAngle, hooming);
                    angle = angle + ((endAngle - startAngle) / (radialNumber - 1));
                }
            }
            if(muzzleFlash != null)
                muzzleFlash.SetActive(true);
        }
    }
    private void FixedUpdate()
    {
        timer++;
        if ( timer >= rate) {

            
            timer = 0;
            if(muzzleFlash)
                muzzleFlash.SetActive(false);

            if (autoFireActive)
            {
                firing = true;
                frame = 0;
                //Shoot(1);
            }
               
           
        }

        if (firing)
        {
            if (sequence.ShouldFire(frame))
            {
                Shoot(1);
            }

            frame++;
            if(frame>sequence.totalFrames)
                firing = false;
        }
    }
    //Auto Fire
    public void Activate()
    {
        autoFireActive = true;
        timer = 0;
        frame = 0;
        firing = true;
    }
    //Auto Fire
    public void DeActivate()
    {
        autoFireActive= false;
    }
}
[Serializable]
public class BulletSequence
{
    public List<int> emmitTimes = new List<int>();
    public int totalFrames;

    public bool ShouldFire(int currentFrame)
    {
        foreach (int frame in emmitTimes)
        {
            if ((currentFrame ==frame))
            {
                return true;
            }
            
        }
        return false;

    }
}
