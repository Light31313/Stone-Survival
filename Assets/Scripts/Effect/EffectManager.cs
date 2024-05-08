using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class EffectManager : MonoBehaviour
{
    private Animator animator;
    [SerializeField]
    private float effectTime = 2f;

    [SerializeField]
    private AudioClip effectSound;

    private AudioSource audioSource;

    private UnityAction onDisable;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        audioSource?.PlayOneShot(effectSound);
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
        onDisable.Invoke();
    }
}
