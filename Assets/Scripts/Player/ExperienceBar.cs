using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExperienceBar : MonoBehaviour
{
    private Slider slider;

    [Header("Refer instance")]
    [SerializeField]
    private Image fill;
    [SerializeField]
    private PlayerStat stat;
    [SerializeField]
    private TextMeshProUGUI levelText;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        stat.onGetExperience += percentLevel => SetExperience(percentLevel);
        stat.onLevelUp += level => OnLevelUp(level);
    }

    public async void SetExperience(float percentLevel)
    {
        await Task.Yield();
        slider.value = percentLevel;
    }

    public void OnLevelUp(int level)
    {
        levelText.text = level.ToString();
        slider.value = 0;
    }
}
