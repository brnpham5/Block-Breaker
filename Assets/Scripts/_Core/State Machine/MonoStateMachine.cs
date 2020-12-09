using UnityEngine;
using System.Collections;
using System.Collections.Generic;



namespace Core {
    public class MonoStateMachine : MonoBehaviour {
        public virtual MonoState CurrentState {
            get { return currentState; }
            set { Transition(value); }
        }

        [SerializeField]
        protected MonoState currentState;
        protected bool inTransition;

        public MonoState firstState;

        protected virtual void Start() {
            this.CurrentState = firstState;
        }

        public virtual T GetState<T>() where T : MonoState {
            T target = GetComponent<T>();
            if (target == null) {
                target = gameObject.AddComponent<T>();
            }
            return target;
        }

        public virtual void ChangeState<T>() where T : MonoState {
            CurrentState = GetState<T>();
        }

        protected virtual void Transition(MonoState value) {
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

            currentState.OnAfterTransition();
        }
    }
}
