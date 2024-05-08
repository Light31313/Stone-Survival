using GgAccel;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

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

    public event UnityAction<float> changeHealthBarAction;

    private UnityAction onEnemyDie;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        enemy = GetComponent<Rigidbody2D>();
    }

    public void initDieAction(UnityAction onDie)
    {
        onEnemyDie = onDie;
    }

    private void OnEnable()
    {
        currentHealth = blueprint.health;
        attackDamage = blueprint.attackDamage;
        speed = blueprint.speed;

        knockTime = blueprint.knockTime;
        knockRes = blueprint.knockRes;
        knockPower = blueprint.knockPower;
        state = EnemyState.NORMAL;
    }

    public void ChaseTarget(Transform target)
    {
        if (target == null) return;
        if (state == EnemyState.IS_KNOCKED) return;

        Vector2 dir = transform.GetDirection(target.position);
        if (dir.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        enemy.MovePosition(enemy.position + Time.deltaTime * speed * dir);
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