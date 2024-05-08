using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ScreenTransition : MonoBehaviour
{
    [SerializeField] private Image imgScreen;
    private Sequence _transitionSequence;

    public void Transition(Action onAppear)
    {
        gameObject.SetActive(true);
        _transitionSequence?.Kill();
        _transitionSequence = DOTween.Sequence();
        _transitionSequence.Append(imgScreen.DOFade(1f, 0.3f).OnComplete(onAppear.Invoke))
            .Append(imgScreen.DOFade(0f, 0.3f)).OnComplete(() => { gameObject.SetActive(false); });
    }
}