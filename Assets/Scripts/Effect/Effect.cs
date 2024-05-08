using System.Collections;
using GgAccel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Effect : MonoBehaviour
{
    [SerializeField]
    private float effectTime = 2f;

    [SerializeField]
    private AudioClip effectSound;

    private UnityAction onDisable;

    private void OnEnable()
    {
        AudioManager.PlaySound(effectSound);
        StartCoroutine(DestroyEffect());
    }

    public void InitEffectEvent(UnityAction onDisable)
    {
        this.onDisable = onDisable;
    }

    private void OnDisable()
    {
        StopCoroutine(DestroyEffect());
    }

    // Update is called once per frame
    IEnumerator DestroyEffect()
    {
        yield return new WaitForSeconds(effectTime);
        onDisable?.Invoke();
    }
}
