using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PoolingManager : Singleton<PoolingManager>
{
    public Dictionary<WeaponId, List<Bullet>> bullets = new Dictionary<WeaponId, List<Bullet>>();

    private List<Bullet> Bullets(WeaponId id)
    {
        if (bullets.ContainsKey(id))
        {
            return bullets[id];
        }

        return new List<Bullet>();
    }

    public Bullet GetBullet(WeaponId id, Bullet bullet)
    {
        // B1: tìm trong pool xem có object nào còn không
        // B2: nếu không còn =>  Instantiate 

        List<Bullet> bulletList = Bullets(id);
        if (bullets.Count > 0)
        {
            for (int i = 0; i < bullets.Count; i++)
            {
                if (!bulletList[i].gameObject.activeInHierarchy)
                {
                    bulletList[i].gameObject.SetActive(true);
                    return bulletList[i];
                }
            }
        }

        Bullet newBullet = Instantiate(bullet);
        bulletList.Add(newBullet);
        return newBullet;
    }

    public void ReturnBullet(Bullet objBullet)
    {
        // ẩn object đi và nhét lại vào trong pool
        objBullet.gameObject.SetActive(false);
    }
}
