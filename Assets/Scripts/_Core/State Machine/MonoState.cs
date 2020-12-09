using UnityEngine;
using System.Collections;

namespace Core
{
	public abstract class MonoState : MonoBehaviour
	{
		public virtual void Enter()
		{
			AddListeners();
		}

		public virtual void OnAfterTransition()
		{

		}

		public virtual void Exit()
		{
			RemoveListeners();
		}

		protected virtual void OnDestroy()
		{
			RemoveListeners();
		}

        protected abstract void AddListeners();

        protected abstract void RemoveListeners();
	}

}
