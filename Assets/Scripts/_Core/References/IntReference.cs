using System;

namespace Core
{
	[Serializable]
	public class IntReference
	{
        public event IntVariable.OnChangeDelegate OnChange {
            add {
                Variable.OnChange += value;
            }

            remove {
                Variable.OnChange -= value;
            }
        }

        public bool UseConstant = false;
		public int ConstantValue;
		public IntVariable Variable;

        public IntReference() {}

        public IntReference(int value) {
            UseConstant = true;
            ConstantValue = value;
        }

		public int Value
		{
			get { return UseConstant ? ConstantValue : Variable.GetValue(); }
            set { Variable.SetValue(value); }
        }

        public static implicit operator int(IntReference reference) {
            return reference.Value;
        }
    }
}
