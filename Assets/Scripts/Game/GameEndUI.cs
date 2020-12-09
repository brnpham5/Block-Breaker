using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using Core;


namespace Game 
{
    public class GameEndUI : PanelController
    {
        public delegate void ButtonDelegate();
        public static event ButtonDelegate OnRestart;
        public static event ButtonDelegate OnMainMenu;

        public TMP_Text scoreText;
        public Button buttonRestart;
        public Button buttonMainMenu;

        public FloatVariable score;


        private void Start()
        {
            buttonRestart.onClick.AddListener(PressRestart);
            buttonMainMenu.onClick.AddListener(PressMainMenu);
            Hide();
        }

        public override void Show()
        {
            base.Show();

            UpdateUI();
        }

        public void UpdateUI()
        {
            scoreText.text = score.GetValue().ToString();
        }

        public void PressRestart()
        {
            Hide();
            OnRestart?.Invoke();
        }

        public void PressMainMenu()
        {
            Hide();
            OnMainMenu?.Invoke();
        }

        private void OnEnable()
        {
            GameEndManager.OnGameEnd += Show;
        }

        private void OnDisable()
        {
            GameEndManager.OnGameEnd -= Show;
        }
    }

}
