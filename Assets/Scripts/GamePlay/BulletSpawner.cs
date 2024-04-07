using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    public BulletManager.BulletType bulletType = BulletManager.BulletType.Bullet1_Size1;  
    public float rate = 1f;
    public float spped = 10f;
    public float timer = 0;
    public GameObject muzzleFlash = null;
    public void Shoot(int size)
    {
        if (size < 0) return;

        if (timer == 0)
        {
            Vector3 velocity = transform.up * spped;
            BulletManager.BulletType bulletToShoot = bulletType + size;
            GameManager.Instance.bulletManager.SpawnBullet(bulletToShoot,transform.position.x,transform.position.y,velocity.x,velocity.y,0);

                muzzleFlash.SetActive(true);
        }
    }
    private void FixedUpdate()
    {
        timer++;
        if (timer >= rate) {

            
            timer = 0;
            if(muzzleFlash)
                muzzleFlash.SetActive(false);

        }
    }
}
