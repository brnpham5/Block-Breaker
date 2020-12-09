using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    [Serializable]
    [CreateAssetMenu(menuName ="Game Event/Game Event")]
	public class GameEvent : ScriptableObject
	{
        protected List<GameEventListener> listeners = new List<GameEventListener>();

        public virtual void Raise() {
            for(int i = listeners.Count - 1; i >= 0; i--) {
                listeners[i].OnEventRaised();
            }
        }

        public void RegisterListener(GameEventListener listener) {
            if (!listeners.Contains(listener)) {
                listeners.Add(listener);
            }
        }

        public void UnregisterListener(GameEventListener listener) {
            if (listeners.Contains(listener)) {
                listeners.Remove(listener);
            }
        }
	}
}
