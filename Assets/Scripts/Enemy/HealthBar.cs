using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private const float SHOW_TIME = 2f;

    private Slider slider;
    public Gradient gradient;

    [Header("Refer instance")]
    [SerializeField]
    private Image fill;

    private Enemy enemy;
    private IEnumerator hideHealthBarCoroutine;
    
    private void Awake()
    {
        slider = GetComponent<Slider>();
        enemy = GetComponentInParent<Enemy>();
        enemy.changeHealthBarAction += healthPercent => SetHealth(healthPercent);
    }

    public void SetHealth(float healthPecent)
    { 
        fill.enabled = true;
        slider.value = healthPecent;
        fill.color = gradient.Evaluate(healthPecent);
        if (!isActiveAndEnabled)
            return;
        if (hideHealthBarCoroutine != null)
        {
            StopCoroutine(hideHealthBarCoroutine);
        }
        hideHealthBarCoroutine = HideHealthBar();
        StartCoroutine(hideHealthBarCoroutine);
    }

    private IEnumerator HideHealthBar()
    {
        yield return new WaitForSeconds(SHOW_TIME);
        fill.enabled = false;
    }

    private void OnDestroy()
    {
        enemy.changeHealthBarAction -= healthPercent => SetHealth(healthPercent);
    }
}
