using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using Core;

namespace Game
{
    public class GameStartManager : MonoBehaviour
    {
        public delegate void GameStartDelegate();
        public static event GameStartDelegate OnGameSetup;
        public static event GameStartDelegate OnGameStart;

        [Header("Scriptable Reference")]
        public SFXSet bgm;
        public FloatVariable score;
        public IntVariable breakCount;
        public BoolVariable GameState;

        [Header("Scene Reference")]
        public StartingBlocks startingBlocks;

        public void Start()
        {
            RestartGame();
        }

        public void RestartGame()
        {
            SetupGame();
            StartGame();
        }

        public void SetupGame()
        {
            GameState.SetValue(true);
            bgm.Play();
            startingBlocks.Setup();
            score.SetValue(0f);
            breakCount.SetValue(0);
            OnGameSetup?.Invoke();
        }

        public void StartGame()
        {
            OnGameStart?.Invoke();
        }

        private void OnEnable()
        {
            GameEndManager.OnGameRestart += RestartGame;
        }

        private void OnDisable()
        {
            GameEndManager.OnGameRestart -= RestartGame;
        }

    }

}
