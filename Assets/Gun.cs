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
                Debug.Log("SHOT");
                Shot(monster);
                _nextShot = Time.time + weaponScriptable.shotDelay;
            }
        }
    }

    void Shot(Monster monster)
    {
        var go = Instantiate<Bullet>(bulletPrefab);
        go.transform.position = transform.position;
        go.damage = weaponScriptable.damage;
        go.GetComponent<SpriteRenderer>().sprite = weaponScriptable.bulletSprite;
        var dir = (monster.transform.position - transform.position).normalized;
        go.GetComponent<Rigidbody2D>().velocity = weaponScriptable.bulletVelocity * dir;
    }

    Monster nextMonster()
    {
        var monsters = FindObjectsOfType<Monster>();
        //var closest = monsters.ToList().Min(it => (it.transform.position - transform.position));
        var closest = monsters.Aggregate((min, next) => 
            (min.transform.position - transform.position).magnitude < (next.transform.position - transform.position).magnitude
            ? min
            : next
        );
        return closest;
    }
}
