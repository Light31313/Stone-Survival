using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHeart : MonoBehaviour
{
    private const int FULL_HEART_POSITION = 0;
    private const int THREE_QUARTERS_HEART_POSITION = 1;
    private const int HALF_HEART_POSITION = 2;
    private const int ONE_QUARTER_HEART_POSITION = 3;
    private const int EMPTY_HEART_POSITION = 4;
    [SerializeField]
    private int heartOrder;

    private Image heart;
    [SerializeField]
    private Sprite[] heartImages;

    [Header("Refer Instance")]
    [SerializeField]
    private PlayerStat playerStat;

    // Start is called before the first frame update
    void Awake()
    {
        heart = GetComponent<Image>();
        playerStat.onChangeHealth += healthPercent => UpdateHeartUI(healthPercent);
    }

    private void UpdateHeartUI(float healthPercent)
    {
        if (healthPercent >= 0.16 + heartOrder * 0.2)
        {
            heart.sprite = heartImages[FULL_HEART_POSITION];
        }
        else if (healthPercent >= 0.12 + heartOrder * 0.2)
        {
            heart.sprite = heartImages[THREE_QUARTERS_HEART_POSITION];
        }
        else if (healthPercent >= 0.08 + heartOrder * 0.2)
        {
            heart.sprite = heartImages[HALF_HEART_POSITION];

        }
        else if (healthPercent >= 0.04 + heartOrder * 0.2)
        {
            heart.sprite = heartImages[ONE_QUARTER_HEART_POSITION];
        }
        else
        {
            heart.sprite = heartImages[EMPTY_HEART_POSITION];
        }
    }
}
