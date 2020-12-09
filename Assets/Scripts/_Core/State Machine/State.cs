using UnityEngine;
using System.Collections;

namespace Core {
    public abstract class State {
        public virtual void Enter() {
            AddListeners();
        }

        public virtual void Exit() {
            RemoveListeners();
        }

        protected virtual void AddListeners() { }

        protected virtual void RemoveListeners() { }
    }
}
