using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMenu : MonoBehaviour
{
    private void OnEnable()
    {
        Time.timeScale = 0f;
    }

    public void Play()
    {
        gameObject.SetActive(false);
        GameManager.isPlaying = true;
    }

    private void OnDisable()
    {
        Time.timeScale = 1f;
    }
}
