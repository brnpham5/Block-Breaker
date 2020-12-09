using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Core;

namespace Game {
    public class PauseUIManager : PanelController {
        public delegate void PauseDelegate();
        public static event PauseDelegate OnResume;
        public static event PauseDelegate OnRestart;
        public static event PauseDelegate OnOptions;
        public static event PauseDelegate OnQuit;

        public Button buttonResume;
        public Button buttonRestart;
        public Button buttonOptions;
        public Button buttonQuit;

        private void Start() {
            buttonResume.onClick.AddListener(PressResume);
            buttonRestart.onClick.AddListener(PressRestart);
            buttonOptions.onClick.AddListener(PressOptions);
            buttonQuit.onClick.AddListener(PressQuit);

            Hide();
        }

        public void PressResume() {
            OnResume?.Invoke();
        }

        public void PressRestart() {
            OnRestart?.Invoke();
        }

        public void PressOptions() {
            OnOptions?.Invoke();
            Hide();
        }

        public void PressQuit() {
            OnQuit?.Invoke();
        }

        private void OnEnable() {
            PauseManager.OnPause += Show;
            PauseManager.OnUnpause += Hide;
            GameEndManager.OnGameRestart += Hide;
            OptionMenuManager.OnClose += Show;
            OptionMenuManager.OnForceClose += Hide;
        }

        private void OnDisable() {
            PauseManager.OnPause -= Show;
            PauseManager.OnUnpause -= Hide;
            GameEndManager.OnGameRestart -= Hide;
            OptionMenuManager.OnClose -= Show;
            OptionMenuManager.OnForceClose -= Hide;
        }

    }

}
