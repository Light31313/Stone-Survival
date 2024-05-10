using System;
using GgAccel;
using System.Threading.Tasks;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public enum EnemyState
{
    NORMAL,
    IS_KNOCKED,
    DIE
}

public class Enemy : MonoBehaviour
{
    public const string TAG = "Enemy";
    public EnemyType Type => blueprint.type;
    protected float currentHealth;
    protected float attackDamage;
    protected float speed;
    protected float knockTime;
    protected float knockRes;
    protected float knockPower;

    protected Rigidbody2D enemy;

    [Header("Refer instance")] [SerializeField]
    private EnemyBlueprint blueprint;

    [SerializeField] private Transform centerPoint;
    public EnemyState state;
    [SerializeField] private PlayerStat playerStat;

    [SerializeField] private DamageTaken damageTakenPrefab;
    [SerializeField] private Effect dieEffectPrefab;
    private NativeArray<float3> _newMovePos;
    private Transform _cacheTransform;
    private JobHandle _movingJobHandle;
    private MovingEnemyJob _movingJob;
    private bool _isInit;
    public event UnityAction<float> changeHealthBarAction;

    private UnityAction onEnemyDie;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        enemy = GetComponent<Rigidbody2D>();
        _cacheTransform = transform;
    }

    public void initDieAction(UnityAction onDie)
    {
        onEnemyDie = onDie;
    }

    private void OnValidate()
    {
        centerPoint = transform.Find("CenterPoint");
    }

    private void OnEnable()
    {
        _newMovePos = new NativeArray<float3>(1, Allocator.Persistent);
        currentHealth = blueprint.health;
        attackDamage = blueprint.attackDamage;
        speed = blueprint.speed;

        knockTime = blueprint.knockTime;
        knockRes = blueprint.knockRes;
        knockPower = blueprint.knockPower;
        state = EnemyState.NORMAL;
    }

    private void OnDisable()
    {
        _isInit = false;
        _movingJobHandle.Complete();
        if (_newMovePos.IsCreated) _newMovePos.Dispose();
    }

    public void ChaseTarget(Transform target)
    {
        if (state == EnemyState.IS_KNOCKED) return;
        if (!_isInit)
        {
            _isInit = true;
        }
        else
        {
            _movingJobHandle.Complete();
            enemy.MovePosition((Vector3)_movingJob.newMovePos[0]);
        }

        float3 dir = _cacheTransform.GetDirection(target.position);
        if (dir.x > 0)
        {
            _cacheTransform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            _cacheTransform.localScale = new Vector3(-1, 1, 1);
        }

        if (!_newMovePos.IsCreated) _newMovePos = new NativeArray<float3>(1, Allocator.Persistent);

        _movingJob = new MovingEnemyJob
        {
            speed = speed,
            currentPos = _cacheTransform.position,
            deltaTime = Time.fixedDeltaTime,
            moveDir = dir,
            newMovePos = _newMovePos
        };
        _movingJobHandle = _movingJob.Schedule();
    }

    public virtual async void TakeDamage(float damage, float knockPower, Vector3 bulletPosition)
    {
        currentHealth -= damage;

        //InitiateDamageTakenText(Mathf.RoundToInt(damage));

        changeHealthBarAction?.Invoke(currentHealth / blueprint.health);
        InitiateDamageTakenText((int)damage);

        if (currentHealth <= 0)
        {
            Die();
        }

        if (knockTime == 0f) return;

        state = EnemyState.IS_KNOCKED;
        var knockDirection = centerPoint.GetDirection(bulletPosition);
        var velo = -knockDirection * knockPower / knockRes;
        enemy.velocity = velo;
        await Task.Delay(Mathf.RoundToInt(knockTime * 1000));
        state = EnemyState.NORMAL;
    }

    private void InitiateDamageTakenText(int damage)
    {
        var text = Pool.Get(damageTakenPrefab);
        text.InitData(damage, transform.position, () => Pool.Release(text));
    }

    private void Die()
    {
        state = EnemyState.DIE;
        playerStat.CurrentExp += blueprint.experience;
        DropItem();
        var dieEffect = Pool.Get(dieEffectPrefab);
        dieEffect.InitEffectEvent(() => Pool.Release(dieEffect));
        dieEffect.transform.position = transform.position;
        onEnemyDie.Invoke();
    }

    private void DropItem()
    {
        if (blueprint.dropItem == null) return;
        var isDrop = blueprint.dropRate > Random.Range(0f, 1f);
        if (isDrop)
        {
            Instantiate(blueprint.dropItem, transform.position, Quaternion.identity);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        var player = collision.collider.GetComponent<PlayerController>();
        if (player != null)
        {
            player.TakeDamage(attackDamage, knockPower, centerPoint.position);
        }
    }
}