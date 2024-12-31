using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseWeapon : MonoBehaviour
{
    public WeaponId id;
    public Bullet prefabBullet;

    private Character shooter;

    public void CreateBullet(Character shooter)
    {
        this.shooter = shooter;

        Bullet bullet = PoolingManager.Instance.GetBullet(id, prefabBullet);
        if (bullet != null)
        {
            bullet.transform.position = shooter.firePoint.position;
            bullet.transform.rotation = shooter.transform.rotation;
            bullet.shooter = shooter;
        }

        //Bullet bullet = Instantiate(prefabBullet, shooter.firePoint.position, shooter.transform.rotation);
        //bullet.shooter = shooter;
    }
}
