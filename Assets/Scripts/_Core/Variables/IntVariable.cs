using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core {
    [CreateAssetMenu(menuName = "Variable/Int")]
    public class IntVariable : ScriptableObject {
        public delegate void OnChangeDelegate();
        public event OnChangeDelegate OnChange;

#if UNITY_EDITOR
        [Multiline]
        public string DeveloperDescription = "";
#endif

        [SerializeField]
        private int Value;

        public int GetValue() {
            return Value;
        }

        public void ChangeEvent() {
            OnChange?.Invoke();
        }

        public void SetValue(int value) {
            Value = value;
            ChangeEvent();
        }

        public void SetValue(IntVariable value) {
            Value = value.Value;
            ChangeEvent();
        }

        public void ApplyChange(int amount) {
            Value += amount;
            ChangeEvent();
        }

        public void ApplyChange(IntVariable amount) {
            Value += amount.Value;
            ChangeEvent();
        }
    }
}