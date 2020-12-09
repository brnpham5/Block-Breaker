using System;

namespace Core {
    [Serializable]
    public class FloatReference {
        public event FloatVariable.OnChangeDelegate OnChange {
            add {
                Variable.OnChange += value;
            }

            remove {
                Variable.OnChange -= value;
            }
        }

        public bool UseConstant = true;
        public float ConstantValue;
        public FloatVariable Variable;

        public FloatReference() { }

        public FloatReference(float value) {
            UseConstant = true;
            ConstantValue = value;
        }

        public float Value {
            get { return UseConstant ? ConstantValue : Variable.GetValue(); }
            set { Variable.SetValue(value); }
        }

        public static implicit operator float(FloatReference reference) {
            return reference.Value;
        }
    }

}
