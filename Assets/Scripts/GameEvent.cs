using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameEvent")]
public class GameEvent : ScriptableObject
{
    public List<GameEventListener> listeners = new List<GameEventListener>();

    public void Raise(object data = null)
    {
        for(int i = 0; i < listeners.Count; i++)
        {
            listeners[i].OnEventRaised(data);
        }
    }
    
    public void RegisterListenter(GameEventListener listener)
    {
        if(!listeners.Contains(listener))
            listeners.Add(listener);
    }

    public void UnregisterListener(GameEventListener listener)
    {
        if (listeners.Contains(listener))
            listeners.Remove(listener);
    }
}
