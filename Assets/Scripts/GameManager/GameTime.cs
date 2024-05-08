using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameTime : MonoBehaviour
{
    private TextMeshProUGUI timeText;

    private void Start()
    {
        timeText = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        var timeSpan = TimeSpan.FromSeconds(Time.timeSinceLevelLoad);
        var text = string.Format("{0:D1}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
        timeText.text = text;
    }
}
