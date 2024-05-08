using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Player Stat")]
public class PlayerStat : ScriptableObject
{
    public event UnityAction<float> onChangeHealth;
    public event UnityAction<int> onLevelUp;
    public event UnityAction onDie;
    public event UnityAction<float> onGetExperience;
    public event UnityAction<int> onChangeRotateBullet;
    public event UnityAction<int> onChangeAuraLevel;

    [SerializeField]
    private int level = 1;
    public int Level
    {
        get
        {
            return level;
        }
        set
        {
            level = value;
            onLevelUp?.Invoke(level);
        }
    }
    public int ExpToNextLevel
    {
        get
        {
            if (level >= 52)
            {
                return (10 + level) * (10 + level) * 20;
            }
            else if (level >= 40)
            {
                return (10 + level) * (10 + level) * 10;
            }
            else if (level >= 24)
            {
                return (10 + level) * (10 + level) * 5;
            }
            else if (level >= 12)
            {
                return (10 + level) * (10 + level) * 2;
            }
            else
            {
                return (10 + level) * (10 + level);
            }
        }
    }
    [SerializeField]
    private int currentExp;
    public int CurrentExp
    {
        get
        {
            return currentExp;
        }
        set
        {
            currentExp = value;
            onGetExperience?.Invoke((float)currentExp / ExpToNextLevel);
        }
    }
    [SerializeField]
    private float health;
    public float Health
    {
        get
        {
            return health;
        }
        set
        {
            health = value;
            InvokeOnChangeHealth();
        }
    }
    [SerializeField]
    private float currentHealth;
    public float CurrentHealth
    {
        get
        {
            return currentHealth;
        }
        set
        {
            if(value > TotalHealth)
            {
                currentHealth = TotalHealth;
            } else
            {
                currentHealth = value;
            }
            InvokeOnChangeHealth();
            if(value <= 0)
            {
                onDie?.Invoke();
            }
        }
    }

    [SerializeField] private float speed;
    [SerializeField] private float invicibleTime;
    [SerializeField] private float knockedTime;
    [SerializeField] private float knockRes;
    [SerializeField] private float fireRate;
    [SerializeField] private float damage;
    [SerializeField] private int pierceEnemy;
    [SerializeField] private float knockPower;
    [SerializeField] private int rotateBullet;
    [SerializeField] private int RotateBullet
    {
        get
        {
            return rotateBullet;
        }
        set
        {
            rotateBullet = value;
            onChangeRotateBullet?.Invoke(TotalRotateBullet);
        }
    }
    [SerializeField] private int numberOfBullet;
    [SerializeField] private int auraLevel;
    private int Aura
    {
        get
        {
            return auraLevel;
        }
        set
        {
            auraLevel = value;
            onChangeAuraLevel?.Invoke(TotalAuraLevel);
        }
    }

    [SerializeField]
    private float upgradedHealth;
    public float UpgradedHealth
    {
        get
        {
            return upgradedHealth;
        }
        set
        {
            upgradedHealth = value;
            InvokeOnChangeHealth();
        }
    }
    [SerializeField] private float upgradedSpeed;
    [SerializeField] private float upgradedInvicibleTime;
    [SerializeField] private float upgradedKnockedTime;
    [SerializeField] private float upgradedKnockRes;
    [SerializeField] private float upgradedFireRate;
    [SerializeField] private float upgradedDamage;
    [SerializeField] private float upgradedKnockPower;
    [SerializeField] private int upgradedPierceEnemy;
    [SerializeField] private int upgradedRotateBullet;
    private int UpgradedRotateBullet
    {
        get
        {
            return upgradedRotateBullet;
        }
        set
        {
            upgradedRotateBullet = value;
            onChangeRotateBullet?.Invoke(TotalRotateBullet);
        }
    }
    [SerializeField] private int upgradedNumberOfBullet;
    [SerializeField] private int upgradedAuraLevel;
    private int UpgradedAura
    {
        get
        {
            return upgradedAuraLevel;
        }
        set
        {
            upgradedAuraLevel = value;
            onChangeAuraLevel?.Invoke(TotalAuraLevel);
        }
    }

    [SerializeField] private bool isBulletChaseTarget;

    public float TotalSpeed => speed + upgradedSpeed;
    public float TotalHealth => health + upgradedHealth;
    public float TotalInvicibleTime => invicibleTime + upgradedInvicibleTime;
    public float TotalKnockedTime => knockedTime + upgradedKnockedTime;
    public float TotalKnockRes => knockRes + upgradedKnockRes;
    public float TotalFireRate => fireRate + upgradedFireRate;
    public float TotalDamage => damage + upgradedDamage;
    public float TotalPiercing => pierceEnemy + upgradedPierceEnemy;
    public float TotalKnockPower => knockPower + upgradedKnockPower;
    public int TotalRotateBullet => rotateBullet + upgradedRotateBullet;
    public int TotalNumberOfBullet => numberOfBullet + upgradedNumberOfBullet;
    public int TotalAuraLevel => auraLevel + upgradedAuraLevel;
    public bool IsBulletChaseTarget => isBulletChaseTarget;

    public void ResetStat()
    {

        speed = 3;
        health = 100;
        invicibleTime = 1f;
        knockedTime = 0.3f;
        knockRes = 2f;
        fireRate = 2f;
        damage = 10;
        level = 1;
        currentExp = 0;
        knockPower = 2;
        pierceEnemy = 1;
        rotateBullet = 0;
        numberOfBullet = 1;
        auraLevel = 0;

        upgradedSpeed = 0;
        upgradedHealth = 0;
        upgradedInvicibleTime = 0;
        upgradedKnockRes = 0;
        upgradedKnockedTime = 0;
        upgradedFireRate = 0;
        upgradedDamage = 0;
        upgradedPierceEnemy = 0;
        upgradedKnockPower = 0;
        upgradedRotateBullet = 0;
        upgradedNumberOfBullet = 0;
        upgradedAuraLevel = 0;
        isBulletChaseTarget = false;

        currentHealth = TotalHealth;
    }

    public void UpdateStatOnLevelUp()
    {
        CurrentExp -= ExpToNextLevel;
        Level += 1;
    }

    public void Upgrade(UpgradeStat upgrade)
    {
        upgradedSpeed += upgrade.Speed;
        UpgradedHealth += upgrade.Health;
        CurrentHealth += upgrade.Health;
        upgradedInvicibleTime += upgrade.InvicibleTime;
        upgradedKnockedTime += upgrade.KnockedTime;
        upgradedKnockRes += upgrade.KnockRes;
        upgradedFireRate += upgrade.FireRate;
        upgradedDamage += upgrade.Damage;
        upgradedPierceEnemy += upgrade.Piercing;
        upgradedKnockPower += upgrade.KnockPower;
        UpgradedRotateBullet += upgrade.RotateBullet;
        upgradedNumberOfBullet += upgrade.NumberOfBullet;
        UpgradedAura += upgrade.AuraLevel;
        if(!isBulletChaseTarget)
        {
            isBulletChaseTarget = upgrade.IsBulletChaseTarget;
        }
    }

    public void ClearAllEvents()
    {
        onChangeHealth = null;
        onLevelUp = null;
        onDie = null;
        onGetExperience = null;
        onChangeRotateBullet = null;
        onChangeAuraLevel = null;
    }

    private void InvokeOnChangeHealth()
    {
        var healthPercent = currentHealth / TotalHealth;
        onChangeHealth?.Invoke(healthPercent);
        if (healthPercent <= 0f)
        {
            onDie?.Invoke();
        }
    }
}
