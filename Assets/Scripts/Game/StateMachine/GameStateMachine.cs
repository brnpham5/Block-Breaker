using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Core;

namespace Game {
    public class GameStateMachine : MonoStateMachine {
        [Header("Editor Reference")]
        public Spawner spawner;
        public Playspace playspace;

        //States
        public ControlState controlState;
        public FallState fallState;
        public GroupState groupState;
        public BreakState breakState;

        private void Awake()
        {
            playspace.Setup();
            spawner.transform.localPosition = new Vector3(0, -1, 0);
        }

        public void Restart()
        {
            spawner.SetNext();
            CurrentState = firstState;
        }

        public void EndGame() {
            ChangeState<GameEndState>();
        }

        private void OnEnable()
        {
            GameStartManager.OnGameStart += Restart;
            GameEndManager.OnGameEnd += EndGame;
        }

        private void OnDisable()
        {
            GameStartManager.OnGameStart -= Restart;
            GameEndManager.OnGameEnd -= EndGame;
        }
    }

}
