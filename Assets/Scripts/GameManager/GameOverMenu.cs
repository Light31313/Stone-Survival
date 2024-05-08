using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI survivedText;

    private void OnEnable()
    {
        Time.timeScale = 0f;
        GameManager.isPlaying = false;
        var timeSpan = TimeSpan.FromSeconds(Time.timeSinceLevelLoad);
        var text = string.Format("{0:D1}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
        survivedText.text = "Survived time: " + text;
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
