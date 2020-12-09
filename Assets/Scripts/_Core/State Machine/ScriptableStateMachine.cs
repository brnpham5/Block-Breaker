using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core {
    [CreateAssetMenu(fileName = "New State Machine", menuName = "State Machine/Machine")]
    public class ScriptableStateMachine : ScriptableObject {
        public ScriptableState CurrentState {
            get { return CurrentState; }
            set { Transition(value); }
        }

        public List<ScriptableState> states;

        public ScriptableState defaultState;

        [SerializeField]
        protected ScriptableState currentState;
        protected bool inTransition;

        public ScriptableState GetState(ScriptableState value) {
            if (!states.Contains(value)) {
                states.Add(value);
            }
            return states.Find(state => state.Equals(value));
        }

        public void ChangeState(ScriptableState value) {
            CurrentState = GetState(value);
        }

        protected void Transition(ScriptableState value) {
            if (currentState == value || inTransition) {
                return;
            }

            inTransition = true;

            if (currentState != null) {
                currentState.Exit();
            }

            currentState = value;

            if (currentState != null) {
                currentState.Enter();
            }

            inTransition = false;
        }

    }

}
