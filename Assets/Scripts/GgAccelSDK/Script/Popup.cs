using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Object = UnityEngine.Object;

[RequireComponent(typeof(CanvasGroup))]
public abstract class Popup<T> : MonoBehaviour where T : Object
{
    public static Dictionary<string, Popup<T>> PopupDictionary = new();

    [SerializeField] private CanvasGroup canvasGroup;

    [Header("Animation Attributes")] [SerializeField]
    private Ease openEase = Ease.OutBack;

    [SerializeField] private Ease closeEase = Ease.InBack;

    [SerializeField] private float openTime = 0.3f;
    [SerializeField] private float closeTime = 0.3f;

    [SerializeField] private float openScale = 0.4f;
    [SerializeField] private float closeScale = 0.4f;
    protected Transform cacheTransform;
    private Sequence _openCloseSequence;

    private void OnValidate()
    {
        canvasGroup = transform.GetComponent<CanvasGroup>();
    }

    private void Awake()
    {
        cacheTransform = transform;
    }

    public static void Open(Action<T> onOpen = null)
    {
        var key = typeof(T).Name;
        if (PopupDictionary.ContainsKey(key))
        {
            PopupDictionary[key].OnOpen();
            onOpen?.Invoke(PopupDictionary[key] as T);
        }
        else
        {
            var popupPrefab = Resources.Load<T>($"{PopupManager.FolderName}/{key}");
            if (!popupPrefab)
            {
                Debug.LogError("Popup is not in Resource folder");
            }
            else
            {
                if (popupPrefab is Popup<T> p)
                {
                    var popupView = Instantiate(p, PopupManager.Instance.CacheTransform);
                    popupView.OnOpen();
                    onOpen?.Invoke(popupView as T);
                    PopupDictionary[key] = popupView;
                }
                else
                {
                    Debug.LogError("Class is not inherit from Popup");
                }
            }
        }
    }

    public static void Close()
    {
        var key = typeof(T).Name;
        if (PopupDictionary.ContainsKey(key))
        {
            PopupDictionary[key].OnClose();
        }
        else
        {
            var popupPrefab = Resources.Load<T>($"{PopupManager.FolderName}/{key}");
            if (!popupPrefab)
            {
                Debug.LogError("Popup is not in Resource folder");
            }
            else
            {
                if (popupPrefab is Popup<T> p)
                {
                    var popupView = Instantiate(p, PopupManager.Instance.CacheTransform);
                    popupView.OnOpen();
                    PopupDictionary[key] = popupView;
                }
                else
                {
                    Debug.LogError("Class is not inherit from Popup");
                }
            }
        }
    }

    private void OnOpen()
    {
        gameObject.SetActive(true);
        cacheTransform.localScale = Vector3.one * openScale;
        canvasGroup.alpha = 0f;
        _openCloseSequence?.Kill();
        _openCloseSequence = DOTween.Sequence();

        _openCloseSequence.Append(cacheTransform.DOScale(Vector3.one, openTime).SetEase(openEase));
        _openCloseSequence.Join(canvasGroup.DOFade(1f, openTime));
    }

    private void OnClose()
    {
        _openCloseSequence?.Kill();
        _openCloseSequence = DOTween.Sequence();

        _openCloseSequence.Append(cacheTransform.DOScale(Vector3.one * closeScale, closeTime).SetEase(closeEase));
        _openCloseSequence.Join(canvasGroup.DOFade(0f, closeTime));
        _openCloseSequence.OnComplete(() => { gameObject.SetActive(false); });
    }
}