using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Core;

namespace Game
{
    public class TutorialMenuManager : PanelController
    {
        public List<TutorialPanel> panels;

        private int currentIndex = 0;


        private void Start()
        {
            Hide();
        }

        public override void Show()
        {
            base.Show();
            panels[currentIndex].Show();
        }

        public override void Hide()
        {
            base.Hide();
            ResetPanels();
        }

        public void NextPanel()
        {
            panels[currentIndex].Hide();
            currentIndex++;
            panels[currentIndex].Show();
            
        }

        public void PrevPanel()
        {
            panels[currentIndex].Hide();
            currentIndex--;
            panels[currentIndex].Show();
        }

        public void ResetPanels()
        {
            currentIndex = 0;
            for(int i = 0; i < panels.Count; i++)
            {
                panels[i].Hide();
            }
        }

        private void OnEnable()
        {
            MainMenuManager.OnTutorial += Show;
            TutorialPanel.OnExit += Hide;
            TutorialPanel.OnNext += NextPanel;
            TutorialPanel.OnPrev += PrevPanel;
        }

        private void OnDisable()
        {
            MainMenuManager.OnTutorial -= Show;
            TutorialPanel.OnExit -= Hide;
            TutorialPanel.OnNext -= NextPanel;
            TutorialPanel.OnPrev -= PrevPanel;
        }
    }

}
