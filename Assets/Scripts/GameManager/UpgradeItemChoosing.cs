using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeItemChoosing : MonoBehaviour
{
    [SerializeField]
    private int shopIndex;

    [Header("Refer Instance")]
    [SerializeField]
    private TextMeshProUGUI itemName;
    [SerializeField]
    private Image image;
    [SerializeField]
    private TextMeshProUGUI desText;
    [SerializeField]
    private PlayerStat playerStat;
    [SerializeField]
    private GameObject levelUpMenu;
    private LevelUpMenu menuScript;

    private UpgradeItem item;

    private void Awake()
    {
        menuScript = levelUpMenu.GetComponent<LevelUpMenu>();   
    }

    private async void OnEnable()
    {
        await Task.Yield();
        item = menuScript.ShowUpItems[shopIndex];
        itemName.text = item.UpgradeName;
        image.sprite = item.Sprite;
        desText.text = item.Description;
    }

    public void UpgradePlayer()
    {
        item.CurrentLevel++;
        playerStat.Upgrade(item.UpgradeStat);
        levelUpMenu.SetActive(false);
    }
}
