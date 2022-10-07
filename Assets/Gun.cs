using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Gun : MonoBehaviour
{
    public Weapon weaponScriptable;
    public Bullet bulletPrefab;

    void Start()
    {
        
    }

    float _nextShot = 0F;
    void Update()
    {
        if (Time.time > _nextShot)
        {
            var monster = nextMonster();
            if (monster != null)
            {
                Shot(monster);
                _nextShot = Time.time + weaponScriptable.shotDelay;
            }
        }
    }

    void Shot(Monster monster)
    {
        var bullet = Instantiate<Bullet>(bulletPrefab, transform.position, Quaternion.identity, null);
        bullet.damage = weaponScriptable.damage;
        bullet.GetComponent<SpriteRenderer>().sprite = weaponScriptable.bulletSprite;
        var dir = (monster.transform.position - transform.position).normalized;
        bullet.GetComponent<Rigidbody2D>().velocity = weaponScriptable.bulletVelocity * dir;
        Destroy(bullet.gameObject, 1F);
    }

    Monster nextMonster()
    {
        var monsters = FindObjectsOfType<Monster>();
        if(monsters.Length == 0)
        {
            return null;
        }
        var closest = monsters.Aggregate((min, next) => 
            (min.transform.position - transform.position).magnitude < (next.transform.position - transform.position).magnitude
            ? min
            : next
        );
        return closest;
    }
}
