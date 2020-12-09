using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using Core;
using System;

namespace Game {
    public class FallState : MonoState {
        [Header("Scriptable Reference")]
        public BlockSet activeBlocks;
        public FloatReference autoDropDelay;
        public BoolVariable GameState;

        [Header("Scene Reference")]
        public GameStateMachine stateMachine;
        
        //Private
        private WaitForSeconds autoDelay;
        private List<IDroppable> sortedDroppables = new List<IDroppable>();

        private void Start() {
            autoDelay = new WaitForSeconds(autoDropDelay.Value);
        }

        #region
        public override void Enter() {
            base.Enter();

            SortBlocks();
        }

        public override void OnAfterTransition()
        {
            base.OnAfterTransition();

            StartCoroutine(FallCoroutine());
        }

        public override void Exit() {
            base.Exit();
            sortedDroppables.Clear();
        }

        protected override void AddListeners()
        {

        }

        protected override void RemoveListeners()
        {

        }

        #endregion

        /// <summary>
        /// Iterate through all active blocks and sort them into lowest to highest
        /// </summary>
        private void SortBlocks()
        {
            //Remove control of blocks
            Block block;
            IDroppable droppable;
            IPlaceable placeable;
            bool isInserted;

            //Check if there are any active blocks
            if (activeBlocks.Count > 0)
            {
                //Iterate through all active blocks
                for (int i = 0; i < activeBlocks.Count; i++)
                {
                    isInserted = false;
                    block = activeBlocks.Items[i];
                    
                    if(block.IsGrouped() == true)
                    {
                        droppable = block.GetGroup().droppable;
                        placeable = block.GetGroup().placeable;
                    } else
                    {
                        droppable = block.droppable;
                        placeable = block.placeable;
                    }
                    
                    //Iterate through currently sorted blocks and insert according to localPosition.y value
                    for (int j = 0; j < sortedDroppables.Count; j++)
                    {
                        if (sortedDroppables.Contains(droppable) == false && block.transform.localPosition.y > sortedDroppables[j].GetTransform().localPosition.y)
                        {
                            sortedDroppables.Insert(j, droppable);
                            
                            isInserted = true;
                            break;
                        }
                    }

                    //If y value is current lowest, then just add to end
                    if (!isInserted && sortedDroppables.Contains(droppable) == false)
                    {
                        sortedDroppables.Add(droppable);
                    }

                    placeable.Unplace();

                }
            }
        }

        public IEnumerator FallCoroutine() {
            IDroppable droppable;

            while (sortedDroppables.Count > 0) {
                for (int i = sortedDroppables.Count - 1; i >= 0; i--) {
                    droppable = sortedDroppables[i];

                    if (droppable.CanDrop()) {
                        droppable.Drop();
                    } else {
                        droppable.DoneDropping();
                        sortedDroppables.Remove(droppable);
                    }
                }

                yield return autoDelay;
            }

            ToNextState();
        }

        public void ToNextState() {
            if(GameState.GetValue() == false) {
                stateMachine.ChangeState<GameEndState>();
            } else {
                stateMachine.ChangeState<GroupState>();
            }
        }

    }
}
