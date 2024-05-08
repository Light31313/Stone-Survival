using UnityEngine;

public abstract class UIScreen : MonoBehaviour
{
    public abstract ScreenName ScreenName { get; }

    public void Open()
    {
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}

public enum ScreenName
{
    GamePlay,
    Lobby,
    Login,
    None
}