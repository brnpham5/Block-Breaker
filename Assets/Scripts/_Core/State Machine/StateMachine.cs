using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core {
    public class StateMachine: MonoBehaviour {

        public virtual State CurrentState {
            get { return currentState; }
            set { Transition(value); }
        }

        protected Dictionary<Type, State> states = new Dictionary<Type, State>();
        protected State currentState;
        protected bool inTransition;

        protected virtual void Start() {

        }

        public virtual T GetState<T>() where T : State {
            if (!states.ContainsKey(typeof(T))) {
                states.Add(typeof(T), (T)Activator.CreateInstance(typeof(T)));
            }
            return (T)states[typeof(T)];
        }

        /// <summary>
        /// Queues a state via a coroutine. Stops all Coroutines when called.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void QueueState<T>() where T : State {
            if (inTransition) {
                StopAllCoroutines();
                StartCoroutine(QueueStateCoroutine(GetState<T>()));
            } else {
                ChangeState<T>();
            }
        }

        private IEnumerator QueueStateCoroutine(State state) {
            while (inTransition == true) {
                yield return null;
            }

            this.CurrentState = state;
        }

        public virtual void ChangeState<T>() where T : State {
            CurrentState = GetState<T>();
        }

        protected virtual void Transition(State value) {
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
