using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;

[CreateAssetMenu(menuName = "CREATE/Weapon")]
public class Weapon : ScriptableObject
{
    public int bulletsPerShot = 1;
    public float shotDelay = 1F;
    public float accuracyDegrees = 0;
    public float range = 5F;
    public int damage = 1;
    public float bulletVelocity = 5F;
    public Sprite bulletSprite;
}
