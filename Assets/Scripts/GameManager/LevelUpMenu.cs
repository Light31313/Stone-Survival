using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelUpMenu : MonoBehaviour
{
    private const int SPECIAL_UPGRADE = 4;
    private const int NUMBER_OF_SHOW_UP_ITEMS = 3;

    [Header("Refer Instance")]
    [SerializeField]
    private PlayerStat playerStat;

    [SerializeField]
    private UpgradeItem[] normalUpgradeItems;
    [SerializeField]
    private UpgradeItem[] specialUpgradeItems;
    [SerializeField]
    private UpgradeItem defaultSpecialItem;

    private UpgradeItem[] showUpItems = new UpgradeItem[3];
    public UpgradeItem[] ShowUpItems => showUpItems;

    private void Awake()
    {
        foreach(var item in specialUpgradeItems)
        {
            item.CurrentLevel = 0;
        }
    }

    private void OnEnable()
    {
        Time.timeScale = 0;
        var random = new System.Random();
        if ((playerStat.Level + 1) % SPECIAL_UPGRADE != 0)
        {
            showUpItems = normalUpgradeItems.OrderBy(x => random.Next()).Take(NUMBER_OF_SHOW_UP_ITEMS).ToArray();
        }
        else
        {
            var listItem = specialUpgradeItems.ToList();
            var levelMaxItems = new List<UpgradeItem>();
            foreach (var item in listItem)
            {
                if (item.CurrentLevel >= item.MaxLevel)
                {
                    levelMaxItems.Add(item);
                }
            }
            foreach (var item in levelMaxItems)
            {
                listItem.Remove(item);
            }
            while (listItem.Count < NUMBER_OF_SHOW_UP_ITEMS)
            {
                listItem.Add(defaultSpecialItem);
            }

            showUpItems = listItem.OrderBy(x => random.Next()).Take(NUMBER_OF_SHOW_UP_ITEMS).ToArray();
        }
    }

    private void OnDisable()
    {
        Time.timeScale = 1;
        playerStat.UpdateStatOnLevelUp();
    }
}
