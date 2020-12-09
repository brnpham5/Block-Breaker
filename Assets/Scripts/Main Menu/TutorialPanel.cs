using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Core;

namespace Game
{
    public class TutorialPanel : PanelController
    {
        public delegate void TutorialDelegate();
        public static event TutorialDelegate OnExit;
        public static event TutorialDelegate OnNext;
        public static event TutorialDelegate OnPrev;

        public Button buttonExit;
        public Button buttonNext;
        public Button buttonPrev;

        private void Start()
        {
            Hide();

            if (buttonExit.isActiveAndEnabled)
            {
                buttonExit.onClick.AddListener(PressExit);
            }

            if (buttonNext.isActiveAndEnabled)
            {
                buttonNext.onClick.AddListener(PressNext);
            }

            if (buttonPrev.isActiveAndEnabled)
            {
                buttonPrev.onClick.AddListener(PressPrev);
            }
        }

        private void PressExit()
        {
            OnExit?.Invoke();
        }

        private void PressNext()
        {
            OnNext?.Invoke();
        }

        private void PressPrev()
        {
            OnPrev?.Invoke();
        }
    }

}
