using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core {
    [CreateAssetMenu(menuName = "Variable/String")]
    public class StringVariable : ScriptableObject {
        public delegate void OnChangeDelegate();
        public event OnChangeDelegate OnChange;

#if UNITY_EDITOR
        [Multiline]
        public string DeveloperDescription = "";
#endif

        [SerializeField]
        private string Value;

        public string GetValue() {
            return Value;
        }

        public void ChangeEvent() {
            OnChange?.Invoke();
        }

        public void SetValue(string value) {
            Value = value;
            ChangeEvent();
        }

        public void SetValue(StringVariable value) {
            Value = value.Value;
            ChangeEvent();
        }
    }
}