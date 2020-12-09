using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core {
    [CreateAssetMenu(menuName = "Variable/Bool")]
    public class BoolVariable : ScriptableObject {
        public delegate void OnChangeDelegate();
        public event OnChangeDelegate OnChange;

#if UNITY_EDITOR
        [Multiline]
        public string DeveloperDescription = "";
#endif

        [SerializeField]
        private bool Value;

        public bool GetValue() {
            return Value;
        }

        public void ChangeEvent() {
            OnChange?.Invoke();
        }

        public void SetValue(bool value) {
            Value = value;
            ChangeEvent();
        }

        public void SetValue(BoolVariable value) {
            Value = value.Value;
            ChangeEvent();
        }
    }
}