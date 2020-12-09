using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Core;


namespace Game {
    public class GameEndState : MonoState {

        [Header("Scriptable Reference")]
        public GroupSet allGroups;
        public BlockSet allBlocks;
        public BlockSet activeBlocks;

        [Header("Editor Reference")]
        public Spawner spawner;
        public Controllable control;
        public GameStateMachine stateMachine;
        public GameObject blockPool;

        public override void Enter() {
            base.Enter();
            EndGame();
        }

        public override void Exit() {
            base.Exit();
        }

        /// <summary>
        /// 1) Remove all blocks on board
        /// 2) Remove blocks from controller
        /// 3) Remove next blocks
        /// </summary>
        public void EndGame() {
            ClearControl();
            ClearSpawn();
            ClearBoard();
        }

        public void ClearBoard() {
            for (int i = allGroups.Items.Count - 1; i >= 0; i--) {
                allGroups.Items[i].breakable.CleanBreak();
            }

            for (int i = allBlocks.Items.Count - 1; i >= 0; i--) {
                allBlocks.Items[i].breakable.CleanBreak();
            }

            allGroups.Items.Clear();
            allBlocks.Items.Clear();
        }

        public void ClearSpawn() {
            spawner.Cleanup();
        }

        public void ClearControl() {
            activeBlocks.Items.ForEach(block => {
                block.breakable.CleanBreak();
            });

            activeBlocks.Items.Clear();
        }

        protected override void AddListeners() {

        }

        protected override void RemoveListeners() {

        }
    }

}
