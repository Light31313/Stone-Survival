using System.Collections.Generic;
using GgAccel;
using UnityEngine;

public class ScreenManager : MonoSingleton<ScreenManager>
{
    [SerializeField] private UIScreen[] screens;
    [SerializeField] private ScreenTransition screenTransition;
    private readonly Dictionary<ScreenName, UIScreen> _screenDic = new();
    private ScreenName _currentOpenScreen = ScreenName.None;

    protected override void Start()
    {
        base.Start();
        _screenDic.Clear();
        foreach (var screen in screens)
        {
            _screenDic[screen.ScreenName] = screen;
        }
    }

    private void OnValidate()
    {
        screens = GetComponentsInChildren<UIScreen>(true);
    }

    public void OpenScreen(ScreenName screenName)
    {
        if (_currentOpenScreen == screenName) return;
        screenTransition.Transition(() =>
        {
            if (_currentOpenScreen != ScreenName.None)
                _screenDic[_currentOpenScreen].Close();
            _currentOpenScreen = screenName;
            _screenDic[_currentOpenScreen].Open();
        });
    }
}
