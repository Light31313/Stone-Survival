using GgAccel;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Bullet : MonoBehaviour
{
    private const float LifeTime = 3;

    [SerializeField] private float knockPower = 10f;
    [SerializeField] private float chasingRange = 5f;
    [SerializeField] private LayerMask enemyLayers;
    [SerializeField] private float findTargetRate = 0.2f;
    private WaitForSeconds waitFindingTarget;
    [SerializeField] private float findingForce = 2f;

    [Header("Refer instance")] [SerializeField]
    private PlayerStat stat;

    private Rigidbody2D rigidBody;

    private int numberOfHitEnemies = 0;

    private UnityAction onDisable;

    [SerializeField] private Effect impactEffectPrefab;

    // Start is called before the first frame update
    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        waitFindingTarget = new WaitForSeconds(findTargetRate);
    }

    private void OnEnable()
    {
        StartCoroutine(DestroyIfHitNothing());
        numberOfHitEnemies = 0;
        if (stat.IsBulletChaseTarget)
        {
            StartCoroutine(ChasingEnemy());
        }
    }

    private void OnDisable()
    {
        StopCoroutine(DestroyIfHitNothing());
        if (stat.IsBulletChaseTarget)
        {
            StopCoroutine(ChasingEnemy());
        }
    }

    public void InitBulletEvent(UnityAction onDisable)
    {
        this.onDisable = onDisable;
    }

    IEnumerator DestroyIfHitNothing()
    {
        yield return new WaitForSeconds(LifeTime);
        onDisable.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (numberOfHitEnemies >= stat.TotalPiercing) return;

        var enemy = collision.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(stat.TotalDamage, knockPower + stat.TotalKnockPower, transform.position);
            numberOfHitEnemies++;
        }

        if (numberOfHitEnemies >= stat.TotalPiercing)
        {
            var impactEffect = Pool.Get(impactEffectPrefab);
            impactEffect.transform.position = transform.position;
            impactEffect.InitEffectEvent(() =>
                Pool.Release(impactEffect)
            );
            onDisable.Invoke();
        }
    }

    private IEnumerator ChasingEnemy()
    {
        while (isActiveAndEnabled)
        {
            var bulletPosition = transform.position;
            var enemiesInRange = Physics2D.OverlapCircleAll(bulletPosition, chasingRange, enemyLayers);
            var shortestDistance = Mathf.Infinity;
            Transform nearestEnemy = null;
            foreach (var enemy in enemiesInRange)
            {
                var enemyTransform = enemy.transform;
                var distance = (enemyTransform.position - bulletPosition).magnitude;
                if (distance < shortestDistance)
                {
                    nearestEnemy = enemyTransform;
                    shortestDistance = distance;
                }
            }

            if (nearestEnemy != null)
            {
                var dir = transform.GetDirection(nearestEnemy.position);
                rigidBody.AddForce(dir * findingForce, ForceMode2D.Impulse);
            }

            yield return waitFindingTarget;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chasingRange);
    }
}