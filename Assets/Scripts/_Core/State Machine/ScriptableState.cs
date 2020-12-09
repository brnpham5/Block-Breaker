using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core {
    [CreateAssetMenu(fileName = "New State", menuName = "State Machine/State")]
    public class ScriptableState : ScriptableObject {
        public GameEvent EnterEvent;
        public GameEvent ExitEvent;

        public void Enter() {
            EnterEvent.Raise();
        }

        public void Exit() {
            ExitEvent.Raise();
        }

        private void OnDestroy() {
            
        }
    }

}
