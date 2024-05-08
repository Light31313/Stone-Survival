using System.Collections;
using System.Collections.Generic;
using GgAccel;
using UnityEngine;

public class Boss : Enemy
{
    [SerializeField]
    private string getHitAnimationName;
    [SerializeField]
    private AudioClip hurtSound;

    private Animator animator;

    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
    }

    public override void TakeDamage(float damage, float knockPower, Vector3 bulletPosition)
    {
        if(hurtSound != null)
        {
            AudioManager.PlaySound(hurtSound);
        }
        animator.Play(getHitAnimationName);
        base.TakeDamage(damage, knockPower, bulletPosition);
    }
}
