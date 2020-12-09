using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Core;

namespace Game {
    public class PauseManager : MonoBehaviour {
        public delegate void PauseDelegate();
        public static event PauseDelegate OnPause;
        public static event PauseDelegate OnUnpause;

        [Header("Scriptable Reference")]
        public BoolReference gamePause;

        public bool isPaused = false;
        // Start is called before the first frame update
        void Start() {
            gamePause.Value = false;
        }

        public void PauseGame() {
            isPaused = true;
            gamePause.Value = true;
            OnPause?.Invoke();
            Time.timeScale = 0;
            
        }

        public void UnpauseGame() {
            isPaused = false;
            gamePause.Value = false;
            OnUnpause?.Invoke();
            Time.timeScale = 1;
            
            
        }

        public void ChangeState() {
            if (isPaused == false) {
                PauseGame();
            } else if (isPaused == true) {
                UnpauseGame();
            }
        }

        // Update is called once per frame
        void Update() {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                ChangeState();
            }
        }

        private void OnEnable() {
            PauseUIManager.OnResume += UnpauseGame;
            PauseUIManager.OnRestart += UnpauseGame;
            GameEndManager.OnGameEnd += UnpauseGame;
            GameEndManager.OnGameRestart += UnpauseGame;
        }

        private void OnDisable() {
            PauseUIManager.OnResume -= UnpauseGame;
            PauseUIManager.OnRestart -= UnpauseGame;
            GameEndManager.OnGameEnd -= UnpauseGame;
            GameEndManager.OnGameRestart -= UnpauseGame;
        }
    }

}
