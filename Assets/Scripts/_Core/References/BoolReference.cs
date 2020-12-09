using System;

namespace Core
{
	[Serializable]
	public class BoolReference
	{
        public event BoolVariable.OnChangeDelegate OnChange {
            add {
                Variable.OnChange += value;
            }

            remove {
                Variable.OnChange -= value;
            }
        }

        public bool UseConstant = false;
		public bool ConstantValue;
		public BoolVariable Variable;

        public BoolReference() { }

        public BoolReference(bool value) {
            UseConstant = true;
            ConstantValue = value;
        }

		public bool Value
		{
			get { return UseConstant ? ConstantValue : Variable.GetValue(); }
            set { Variable.SetValue(value); }
        }

        public static implicit operator bool (BoolReference reference) {
            return reference.Value;
        }
    }

}
