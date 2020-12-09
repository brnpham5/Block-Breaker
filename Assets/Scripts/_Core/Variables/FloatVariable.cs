using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core {
    [CreateAssetMenu(menuName = "Variable/Float")]
    [System.Serializable]
    public class FloatVariable : ScriptableObject {
        public delegate void OnChangeDelegate();
        public event OnChangeDelegate OnChange;

#if UNITY_EDITOR
        [Multiline]
        public string DeveloperDescription = "";
#endif

        [SerializeField]
        private float Value;

        public float GetValue() {
            return Value;
        }

        public void ChangeEvent() {
            OnChange?.Invoke();
        }

        public void SetValue(float value) {
            Value = value;
            ChangeEvent();
        }

        public void SetValue(FloatVariable value) {
            Value = value.Value;
            ChangeEvent();
        }

        public void ApplyChange(float amount) {
            Value += amount;
            ChangeEvent();
        }

        public void ApplyChange(FloatVariable amount) {
            Value += amount.Value;
            ChangeEvent();
        }
    }
}