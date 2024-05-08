using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class GameMessageController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtGameMessage;
    [SerializeField] private float transitionInDuration = 0.5f;
    [SerializeField] private float transitionOutDuration = 0.5f;
    [SerializeField] private float showDuration = 2f;
    [SerializeField] private float moveUpDistance = 100f;
    private Sequence _showSequence;
    private static Action<string> _onShowMessage;

    private void OnEnable()
    {
        _onShowMessage += OnShowGameMessageSignal;
    }

    private void OnDisable()
    {
        _onShowMessage += OnShowGameMessageSignal;
    }

    public static void ShowMessage(string message)
    {
        _onShowMessage?.Invoke(message);
    }

    private void OnShowGameMessageSignal(string message)
    {
        txtGameMessage.gameObject.SetActive(true);
        txtGameMessage.text = message;
        txtGameMessage.transform.localPosition = Vector3.zero;
        txtGameMessage.alpha = 0;

        _showSequence?.Kill();
        _showSequence = DOTween.Sequence();
        _showSequence.Append(txtGameMessage.DOFade(1f, transitionInDuration))
            .Join(txtGameMessage.transform.DOLocalMoveY(moveUpDistance, transitionInDuration))
            .AppendInterval(showDuration).Append(txtGameMessage.DOFade(0f, transitionOutDuration))
            .OnComplete(() => txtGameMessage.gameObject.SetActive(false));
    }

    [Button]
    public void TestShowMessage(string message)
    {
        OnShowGameMessageSignal(message);
    }
}