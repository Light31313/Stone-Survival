using DG.Tweening;
using TMPro;
using UnityEngine;

public class GeneralText : MonoBehaviour
{
    private TextMeshProUGUI text;
    [SerializeField]
    private float travelTime = 0.8f;
    [SerializeField]
    private float travelDistance = 0.4f;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        var tween = text.transform.DOMoveY(text.transform.position.y + travelDistance, travelTime);
        tween.OnComplete(() => {
            transform.localPosition = Vector3.zero;
            gameObject.SetActive(false);
        });
    }
}
