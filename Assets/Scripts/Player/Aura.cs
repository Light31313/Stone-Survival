using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Aura : MonoBehaviour
{
    [SerializeField]
    private float[] scaleEachLevel;
    [SerializeField]
    private float[] damageHealthRatioEachLevel;
    [SerializeField]
    private float dealDamagePeriod = 0.2f;
    [Header("Refer Instance")]
    [SerializeField]
    private PlayerStat stat;
    private float timer = 0;
    private Collider2D effectRange;

    // Start is called before the first frame update
    void Start()
    {
        effectRange = GetComponent<Collider2D>();
        timer = dealDamagePeriod;
        stat.onChangeAuraLevel += level => OnChangeAuraLevel(level);
        OnChangeAuraLevel(stat.TotalAuraLevel);
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            EnableEffectRange();
        }
    }

    private async void EnableEffectRange()
    {
        effectRange.enabled = true;
        timer = dealDamagePeriod;
        await Task.Delay(100);
        if (effectRange != null)
            effectRange.enabled = false;
    }

    private void OnChangeAuraLevel(int auraLevel)
    {
        var level = auraLevel <= scaleEachLevel.Length ? auraLevel : scaleEachLevel.Length;
        transform.localScale = GetScale(scaleEachLevel[level]);

    }

    private Vector3 GetScale(float scale)
    {
        return new Vector3(scale, scale, scale);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var enemy = collision.GetComponent<Enemy>();
        if (enemy != null)
        {
            var level = stat.TotalAuraLevel <= damageHealthRatioEachLevel.Length ? stat.TotalAuraLevel : scaleEachLevel.Length;
            enemy.TakeDamage(stat.TotalHealth * damageHealthRatioEachLevel[level], stat.TotalKnockPower, transform.position);
        }
    }
}
