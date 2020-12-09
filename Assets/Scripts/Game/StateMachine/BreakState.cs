using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Core;
using System.Net;

namespace Game {
    public class BreakState : MonoState {
        public delegate void BreakDelegate();
        public static event BreakDelegate OnBreak;

        [Header("Scriptable Reference")]
        public BlockSet activeBlocks;
        public BlockSet breakerBlocks;
        public BlockSet diamondBlocks;
        public BoolVariable GameState;

        public BlockSet blueBlocks;
        public BlockSet greenBlocks;
        public BlockSet redBlocks;
        public BlockSet yellowBlocks;

        public FloatVariable scoreMultiplier;
        public GroupMultiplierData multipliers;

        [Header("Scene Reference")]
        public GameStateMachine stateMachine;
        
        private WaitForSeconds delay = new WaitForSeconds(0.25f);
        private bool hasBroken = false;

        public List<IBreakable> breakableList = new List<IBreakable>();
        public List<Block> pendingBlocks = new List<Block>();

        #region StateMachine
        public override void Enter() {
            base.Enter();
            hasBroken = false;

            QueueBreak();

            QueueDrops();

            Break();
        }

        public override void OnAfterTransition()
        {
            StartCoroutine(BreakCoroutine());
        }

        public override void Exit()
        {
            base.Exit();
            if (hasBroken)
            {
                scoreMultiplier.ApplyChange(multipliers.comboMultiplier);
                OnBreak?.Invoke();
            }

            breakableList.Clear();
            pendingBlocks.Clear();
        }

        protected override void AddListeners()
        {

        }

        protected override void RemoveListeners()
        {

        }

        #endregion

        public void Break()
        {
            IBreakable breakable;

            for (int i = breakableList.Count - 1; i >= 0; i--)
            {
                breakable = breakableList[i];
                breakable.Break();
            }

            if(breakableList.Count > 0)
            {
                hasBroken = true;
            }
        }

        /// <summary>
        /// Check all break blocks if they're adjacent to any same color blocks
        /// </summary>
        public void QueueBreak()
        {
            for(int i = diamondBlocks.Count - 1; i >= 0; i--) {
                DiamondLogic(diamondBlocks.Items[i]);
            }

            //Iterate breaker blocks
            for (int i = breakerBlocks.Count - 1; i >= 0; i--)
            {
                QueueAdjacentBlocks(breakerBlocks.Items[i], Direction.all);
            }
        }

        public void DiamondLogic(Block block) {
            QueueBlockToBreak(block);
            Block otherBlock = block.GetBottomAdjacent();

            if(otherBlock != null) {
                QueueColor(otherBlock.data.color);
            }
        }

        public void QueueColor(BlockColor color) {
            switch (color) {
                case BlockColor.blue:
                blueBlocks.Items.ForEach(block => {
                    QueueBlockToBreak(block);
                });
                break;
                case BlockColor.green:
                greenBlocks.Items.ForEach(block => {
                    QueueBlockToBreak(block);
                });
                break;
                case BlockColor.red:
                redBlocks.Items.ForEach(block => {
                    QueueBlockToBreak(block);
                });
                break;
                case BlockColor.yellow:
                yellowBlocks.Items.ForEach(block => {
                    QueueBlockToBreak(block);
                });
                break;
            }
        }

        /// <summary>
        /// Check to break all blocks in a certain direction and perpendicular to direction
        /// Does not include opposite of direction because that is the block where the check is coming from (already checked)
        /// </summary>
        /// <param name="block"></param>
        /// <param name="dir"></param>
        public void QueueAdjacentBlocks(Block block, Direction dir)
        {
            switch (dir)
            {
                case Direction.up:
                    CheckBreak(block, Direction.up);
                    CheckBreak(block, Direction.right);
                    CheckBreak(block, Direction.left);
                    break;
                case Direction.right:
                    CheckBreak(block, Direction.up);
                    CheckBreak(block, Direction.right);
                    CheckBreak(block, Direction.down);
                    break;
                case Direction.down:
                    CheckBreak(block, Direction.right);
                    CheckBreak(block, Direction.down);
                    CheckBreak(block, Direction.left);
                    break;
                case Direction.left:
                    CheckBreak(block, Direction.up);
                    CheckBreak(block, Direction.down);
                    CheckBreak(block, Direction.left);
                    break;

                case Direction.all:
                    CheckBreak(block, Direction.up);
                    CheckBreak(block, Direction.right);
                    CheckBreak(block, Direction.down);
                    CheckBreak(block, Direction.left);
                    break;
                case Direction.none:
                default:

                    break;
            }
        }

        /// <summary>
        /// Check if block in direction is same color and break it
        /// </summary>
        /// <param name="block"></param>
        /// <param name="dir"></param>
        public void CheckBreak(Block block, Direction dir)
        {
            Block other;
            switch (dir)
            {
                case Direction.up:
                    other = block.GetTopAdjacent();
                    CompareAndBreak(block, other, dir);
                    break;
                case Direction.right:
                    other = block.GetRightAdjacent();
                    CompareAndBreak(block, other, dir);
                    break;
                case Direction.down:
                    other = block.GetBottomAdjacent();
                    CompareAndBreak(block, other, dir);
                    break;
                case Direction.left:
                    other = block.GetLeftAdjacent();
                    CompareAndBreak(block, other, dir);
                    break;
                default:

                    break;
            }
        }

        private void CompareAndBreak(Block block, Block other, Direction dir)
        {
            if (block.SameColor(other))
            {
                if (block.IsGrouped() == true)
                {
                    QueueGroup(block, Direction.none);
                }
                else
                {
                    //Queue original block
                    QueueBlock(block, Direction.none);
                }

                //Queue other block
                if(other.IsGrouped() == true)
                {
                    QueueGroup(other, dir);
                }
                else
                {
                    //Queue other
                    QueueBlock(other, dir);
                }
            }
        }

        /// <summary>
        /// Adds a block to breakable list and pending blocks
        /// </summary>
        public bool QueueBlockToBreak(Block block) {
            bool status = false;
            if(block.IsGrouped() == false) {
                if (!breakableList.Contains(block.breakable)) {
                    breakableList.Add(block.breakable);
                    status = true;
                }
            } else if(block.IsGrouped() == true) {
                QueueGroup(block, Direction.none);
                status = true;
            }
            
            if (!pendingBlocks.Contains(block)) {
                pendingBlocks.Add(block);
                status = true;
            }

            return status;
        }

        /// <summary>
        /// Add block for pending break, then check adjacent blocks for same color
        /// </summary>
        /// <param name="block"></param>
        public void QueueBlock(Block block, Direction dir)
        {
            IBreakable breakable = block.breakable;

            //Queue original block
            if (QueueBlockToBreak(block)) {
                //Break adjacent blocks
                switch (dir) {
                    case Direction.up:
                    QueueAdjacentBlocks(block, Direction.up);
                    break;
                    case Direction.right:
                    QueueAdjacentBlocks(block, Direction.right);
                    break;
                    case Direction.down:
                    QueueAdjacentBlocks(block, Direction.down);
                    break;
                    case Direction.left:
                    QueueAdjacentBlocks(block, Direction.left);
                    break;
                    case Direction.all:
                    QueueAdjacentBlocks(block, Direction.all);
                    break;
                    case Direction.none:
                    default:

                    break;
                }
            }
        }

        /// <summary>
        /// Queue grouped up blocks
        /// Remove all blocks in from from pending blocks
        /// Destroy all adjacent blocks that are the same color to the group
        /// </summary>
        /// <param name="block"></param>
        /// <param name="dir"></param>
        private void QueueGroup(Block block, Direction dir)
        {
            Group group = block.GetGroup();
            IBreakable breakable = group.breakable;

            if (group != null && !breakableList.Contains(breakable))
            {
                breakableList.Add(breakable);
                for (int i = 0; i < group.blocks.Count; i++)
                {
                    if (!pendingBlocks.Contains(group.blocks[i]))
                    {
                        pendingBlocks.Add(group.blocks[i]);

                        QueueAdjacentBlocks(group.blocks[i], dir);
                    }
                }
            }
        }

        /// <summary>
        /// Go through all blocks above pending blocks and set them up to fall
        /// </summary>
        private void QueueDrops() {
            Block block;
            Block current;
            Group group;

            for (int i = 0; i < pendingBlocks.Count; i++) {
                block = pendingBlocks[i].GetTopAdjacent();

                //Iterate up and add all blocks 
                while (block != null)
                {
                    //Check if block is grouped
                    if (block.IsGrouped())
                    {
                        group = block.GetGroup();
                        //Add all blocks in group to active blocks
                        for(int j = 0; j < group.blocks.Count; j++)
                        {
                            activeBlocks.Add(group.blocks[j]);
                        }

                        current = group.data.topLeft;

                        //Iterate through all blocks above the top row
                        while (current != null)
                        {
                            block = current;
                            block = block.GetTopAdjacent();
                            //Iterate up
                            while (block != null)
                            {
                                activeBlocks.Add(block);
                                block = block.GetTopAdjacent();
                            }
                            //Iterate right
                            current = current.GetRightAdjacent();
                        }
                    }
                    //Else if it's a normal block, just add it
                    else
                    {
                        activeBlocks.Add(block);
                        block = block.GetTopAdjacent();
                    }
                    
                }
                
            }
        }

        public void ToNextState() {
            if (GameState.GetValue() == false) {
                stateMachine.ChangeState<GameEndState>();
            }
            else if (activeBlocks.Count > 0) {
                stateMachine.ChangeState<FallState>();
            }
            else {
                stateMachine.ChangeState<ControlState>();
            }
        }

        public IEnumerator BreakCoroutine() {
            yield return delay;

            ToNextState();
        }

    }
}
