using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class DamageTaken : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI text;
    [SerializeField]
    private float travelTime = 0.8f;
    [SerializeField]
    private float travelDistance = 0.4f;
    private UnityAction onDestroy;
    private Tween slideTween;

    public void InitData(int damage, Vector3 spawnPosition, UnityAction onDestroy)
    {
        text.text = damage.ToString();
        text.transform.position = spawnPosition;
        this.onDestroy = onDestroy;
        slideTween?.Kill();
        slideTween = text.transform.DOMoveY(spawnPosition.y + travelDistance, travelTime).OnComplete(() => onDestroy.Invoke());
    }
}
