using System;

namespace Core {
    [Serializable]
    public class StringReference {
        public event StringVariable.OnChangeDelegate OnChange {
            add {
                Variable.OnChange += value;
            }

            remove {
                Variable.OnChange -= value;
            }
        }

        public bool UseConstant = false;
        public string ConstantValue;
        public StringVariable Variable;

        public StringReference() { }

        public StringReference(string value) {
            UseConstant = true;
            ConstantValue = value;
        }

        public string Value {
            get { return UseConstant ? ConstantValue : Variable.GetValue(); }
            set { Variable.SetValue(value); }
        }

        public static implicit operator string(StringReference reference) {
            return reference.Value;
        }
    }

}
