using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UpgradeStat
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private float health;
    [SerializeField]
    private float invicibleTime;
    [SerializeField]
    private float knockedTime;
    [SerializeField]
    private float knockRes;
    [SerializeField]
    private float fireRate;
    [SerializeField]
    private float damage;
    [SerializeField]
    private float knockPower;
    [Header("Special Upgrade")]
    [SerializeField]
    private int piercing;
    [SerializeField]
    private int rotateBullet;
    [SerializeField]
    private int numberOfBullet;
    [SerializeField]
    private int auraLevel;
    [SerializeField]
    private bool isBulletChaseTarget;
    public float Speed => speed;
    public float Health => health;
    public float InvicibleTime => invicibleTime;
    public float KnockedTime => knockedTime;
    public float KnockRes => knockRes;
    public float FireRate => fireRate;
    public float Damage => damage;
    public int Piercing => piercing;
    public float KnockPower => knockPower;
    public int RotateBullet => rotateBullet;
    public int NumberOfBullet => numberOfBullet;
    public int AuraLevel => auraLevel;
    public bool IsBulletChaseTarget => isBulletChaseTarget;
}
