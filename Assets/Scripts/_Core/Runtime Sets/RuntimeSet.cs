using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
	public abstract class RuntimeSet<T> : ScriptableObject
	{
		public int Count
		{
			get { return Items.Count; }
		}

        public List<T> Items;
        public virtual void Add(T t)
		{
			if (!Items.Contains(t)) Items.Add(t);
		}

        public virtual void Remove(T t)
		{
			if (Items.Contains(t)) Items.Remove(t);
		}

		public virtual bool Contains(T t) {
			return Items.Contains(t);
        }

        private void OnEnable() {
			Items.Clear();
        }
    }

}
