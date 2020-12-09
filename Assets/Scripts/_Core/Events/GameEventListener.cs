using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Core
{
    [Serializable]
	public class GameEventListener : MonoBehaviour
	{
		public GameEvent Event;
		public UnityEvent Response;

        public void OnEventRaised() {
            Response.Invoke();
        }

        private void OnEnable() {
            Event.RegisterListener(this);
        }

        private void OnDisable() {
            Event.UnregisterListener(this);
        }
    }

}
