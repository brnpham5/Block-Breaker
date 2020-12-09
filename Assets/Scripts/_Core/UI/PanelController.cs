using UnityEngine;

namespace Core
{
	[RequireComponent(typeof(CanvasGroup))]
	public class PanelController : MonoBehaviour
	{
		public CanvasGroup canvasGroup;

		protected virtual void Awake()
		{
			canvasGroup = GetComponent<CanvasGroup>();
		}

		public virtual void Show()
		{
			canvasGroup.Show();
		}

		public virtual void Hide()
		{
			canvasGroup.Hide();
		}
	}
}
