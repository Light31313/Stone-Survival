using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using GgAccel;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Refer Instance")] [SerializeField]
    private GameObject levelUpMenu;

    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject gameOverMenu;
    [SerializeField] private PlayerStat playerStat;

    public static bool isPlaying = false;


    // Start is called before the first frame update
    private void Start()
    {
        playerStat.HackPlayerStat();
    }

    private void OnEnable()
    {
        playerStat.onGetExperience += OnGetExperience;
        playerStat.onLevelUp += OnLevelUp;
        playerStat.onDie += OnDie;
    }

    private void OnDisable()
    {
        playerStat.onGetExperience -= OnGetExperience;
        playerStat.onLevelUp -= OnLevelUp;
        playerStat.onDie -= OnDie;
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPlaying)
            {
                pauseMenu.SetActive(!pauseMenu.gameObject.activeSelf);
            }
        }
    }

    private void OnLevelUp(int level)
    {
    }

    private async void OnDie()
    {
        await Task.Delay(1000);
        gameOverMenu.SetActive(true);
        AudioManager.PlayPauseAudio();
    }

    private async void OnGetExperience(float percentLevel)
    {
        if (percentLevel >= 1f)
        {
            await Task.Yield();
            levelUpMenu.SetActive(true);
        }
    }
}