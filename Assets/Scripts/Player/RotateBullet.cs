using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GgAccel;
using UnityEngine;

public class RotateBullet: MonoBehaviour
{
    
    [SerializeField]
    private float rotationSpeed = 2;
    [SerializeField]
    private float circleRadius = 2;
    [SerializeField]
    private float knockPower = 10;
    [SerializeField]
    private float startAngle;

    [Header("Refer Instance")]
    [SerializeField]
    private PlayerStat stat;
    [SerializeField]
    private Transform rotatePoint;
    [SerializeField]
    private AudioClip hitSound;

    private Vector3 positionOffset;
    private float angle;

    private Rigidbody2D rigidBody;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        angle = startAngle;
    }

    private void FixedUpdate()
    {
        positionOffset.Set(
            Mathf.Cos(angle) * circleRadius,
            Mathf.Sin(angle) * circleRadius,
            0
        );
        rigidBody.MovePosition(rotatePoint.position + positionOffset);
        angle += Time.deltaTime * (rotationSpeed + stat.TotalSpeed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var enemy = collision.GetComponent<Enemy>();
        if (enemy != null)
        {
            AudioManager.PlaySound(hitSound);
            enemy.TakeDamage(stat.TotalDamage, knockPower + stat.TotalKnockPower, transform.position);
        }
    }
}

