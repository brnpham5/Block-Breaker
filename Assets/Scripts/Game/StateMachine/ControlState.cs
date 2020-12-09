using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Core;

namespace Game {
    public class ControlState : MonoState {
        public delegate void GameStateDelegate();
        public static event GameStateDelegate OnControlState;
        public static event GameStateDelegate OnLose;

        [Header("Scriptable Reference")]
        public BlockSet activeBlocks;
        public BoardSpace board;
        public BoolVariable GameState;
        public GroupMultiplierData multipliers;
        public FloatVariable scoreMultiplier;
        public FloatVariable currentScore;
        
        [Header("Scene Reference")]
        public GameStateMachine stateMachine;
        public Controllable control;
        public Spawner spawner;
        public GameObject blockPool;

        public List<Block> blocks = new List<Block>();

        private void Awake() {
            control.Setup(blocks);
        }

        public override void Enter() {
            base.Enter();

            OnControlState?.Invoke();

            scoreMultiplier.SetValue(multipliers.baseMultipier);
            currentScore.SetValue(0);

            if (IsGameOver() == true)
            {
                OnLose?.Invoke();
            } else
            {
                //Set controller to spawner location
                control.transform.localPosition = spawner.transform.localPosition;

                //Spawn blocks
                SpawnBlocks();

                control.StopMovement();
                //Give control to player
                control.StartMovement();
            }
        }

        public override void Exit() {
            base.Exit();
            StopMovement();
        }

        public override void OnAfterTransition() {
            if (GameState.GetValue() == false) {
                ToGameEndState();
            }
        }

        public void StopMovement() {
            //Move parent from controller to block pool
            blocks.ForEach(block => {
                block.transform.parent = blockPool.transform;
            });

            control.blocks.Clear();

            //Remove control from player
            control.StopMovement();
            
        }

        private bool IsGameOver()
        {
            if (board.IsOccupied(0, -1))
            {
                return true;
            }

            return false;
        }

        private void SpawnBlocks()
        {
            spawner.SpawnNext();

            activeBlocks.Items.ForEach(block =>
            {
                block.transform.parent = control.transform;
                block.gameObject.SetActive(true);
                blocks.Add(block);
            });

            spawner.SetNext();
        }

        public void ToNextState() {

            if(GameState.GetValue() == false) {
                ToGameEndState();
            } else {
                ToFallState();
            }
        }

        public void ToFallState() {
            stateMachine.ChangeState<FallState>();
        }

        public void ToGameEndState() {
            stateMachine.ChangeState<GameEndState>();
        }

        protected override void AddListeners() {
            control.OnInactive += ToNextState;
        }

        protected override void RemoveListeners() {
            control.OnInactive -= ToNextState;
        }
    }

}
