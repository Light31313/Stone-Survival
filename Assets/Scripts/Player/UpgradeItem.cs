using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrade Item")]
public class UpgradeItem : ScriptableObject
{
    [SerializeField]
    private string upgradeName;
    [SerializeField]
    private Sprite sprite;
    [SerializeField]
    private string description;
    [SerializeField]
    private UpgradeStat upgradeStat;
    [Header("For special upgrade only")]
    [SerializeField]
    private int maxLevel;
    private int currentLevel;

    public string UpgradeName => maxLevel == 0 ? upgradeName : upgradeName + " LV." + (currentLevel + 1 >= maxLevel ? "MAX" : (currentLevel + 1));
    public Sprite Sprite => sprite;
    public string Description => description;
    public UpgradeStat UpgradeStat => upgradeStat;
    public int MaxLevel => maxLevel;
    public int CurrentLevel
    {
        get => currentLevel;
        set
        {
            if(value <= maxLevel)
            {
                currentLevel = value;
            }
        }
    }
}
