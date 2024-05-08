using System;
using System.Collections;
using GgAccel;
using UnityEngine;

public enum PlayerState
{
    IS_KNOCKED,
    DIE,
    NORMAL
}

public class PlayerController : MonoBehaviour
{
    public const string TAG = "Player";
    public const string HORIZONTAL = "Horizontal";
    public const string VERTICAL = "Vertical";

    private SpriteRenderer playerRenderer;

    private Animator animator;
    private Rigidbody2D player;

    [Header("Refer instance")] [SerializeField]
    private Transform firePoint;

    [SerializeField] private Transform centerPosition;
    [SerializeField] private GameObject deathEffect;
    [SerializeField] private PlayerStat stat;
    [SerializeField] private GameObject[] rotateBullets;
    [SerializeField] private AudioClip hurtSound;

    private PlayerState state = PlayerState.NORMAL;
    private bool isInvicible = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        player = GetComponent<Rigidbody2D>();
        playerRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        stat.onChangeRotateBullet += value => OnChangeRotateBullet(value);
        OnChangeRotateBullet(stat.TotalRotateBullet);
    }

    private void Update()
    {
        Moving();
    }

    private void Moving()
    {
        if (state == PlayerState.IS_KNOCKED) return;
        var hoz = Input.GetAxisRaw(HORIZONTAL);
        var ver = Input.GetAxisRaw(VERTICAL);

        if (hoz != 0 || ver != 0)
        {
            animator.SetFloat(HORIZONTAL, hoz);
            animator.SetFloat(VERTICAL, ver);
        }

        var dir = new Vector2(hoz, ver).normalized;

        player.velocity = stat.TotalSpeed * dir;
    }

    public void TakeDamage(float damageTaken, float knockPower, Vector3 sourcePosition)
    {
        if (isInvicible) return;

        StartCoroutine(StartInvicibleMode());
        StartCoroutine(StartKnockState(knockPower, sourcePosition));
        AudioManager.PlaySound(hurtSound);
        stat.CurrentHealth -= damageTaken;
        if (stat.CurrentHealth <= 0)
        {
            Die();
        }
    }

    public void Health(float amount)
    {
        stat.CurrentHealth += amount;
    }

    private IEnumerator StartKnockState(float knockPower, Vector3 sourcePosition)
    {
        if (stat.TotalKnockedTime > 0)
        {
            state = PlayerState.IS_KNOCKED;
            var direction = (centerPosition.position - sourcePosition).normalized;
            player.velocity = direction * knockPower / stat.TotalKnockRes;
            yield return new WaitForSeconds(stat.TotalKnockedTime);
            state = PlayerState.NORMAL;
            player.velocity = Vector2.zero;
        }
    }

    private IEnumerator StartInvicibleMode()
    {
        var currentTime = 0.2f;
        isInvicible = true;
        while (currentTime <= stat.TotalInvicibleTime)
        {
            playerRenderer.enabled = false;
            yield return new WaitForSeconds(0.1f);
            playerRenderer.enabled = true;
            yield return new WaitForSeconds(0.1f);
            currentTime += 0.2f;
        }

        isInvicible = false;
    }

    private void Die()
    {
        state = PlayerState.DIE;
        Destroy(gameObject);
        Instantiate(deathEffect, transform.position, Quaternion.identity);
    }

    private void OnChangeRotateBullet(int numberOfRotateBullets)
    {
        foreach (var rotateBullet in rotateBullets)
        {
            if (rotateBullet.activeSelf)
            {
                rotateBullet.SetActive(false);
            }
        }

        var number = numberOfRotateBullets <= rotateBullets.Length ? numberOfRotateBullets : rotateBullets.Length;
        for (int i = 0; i < number; i++)
        {
            rotateBullets[i].SetActive(true);
        }
    }

    private void OnDestroy()
    {
        stat.ClearAllEvents();
    }
}