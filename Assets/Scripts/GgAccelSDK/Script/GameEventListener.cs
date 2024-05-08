using GgAccel;
using UnityEngine;
using UnityEngine.Events;

namespace GgAccel
{
    [System.Serializable]
    public class CustomGameEvent : UnityEvent<object> { }
    
    public class GameEventListener : MonoBehaviour
    {
        [SerializeField]
        private GameEvent gameEvent;

        public CustomGameEvent response;

        private void OnEnable()
        {
            gameEvent.RegisterListenter(this);
        }

        private void OnDisable()
        {
            gameEvent.UnregisterListener(this);
        }

        public void OnEventRaised(object data)
        {
            response.Invoke(data);
        }
    }
}

