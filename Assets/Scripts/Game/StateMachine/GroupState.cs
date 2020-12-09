using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Core;

namespace Game
{
    public class GroupState : MonoState
    {
        [Header("Scriptable Reference")]
        public BoardSpace board;
        public BlockSet activeBlocks;
        public GroupSet activeGroups;
        public BoolVariable GameState;
        

        [Header("Scene Reference")]
        public GameStateMachine stateMachine;
        public GroupSpawner groupSpawner;

        public override void Enter()
        {
            Group group;
            //TODO: For more efficiency, only activate groups adjacent to an active block;
            for(int j = 0; j < activeGroups.Items.Count; j++) {
                group = activeGroups.Items[j];
                if(group.isActive == true)
                {
                    ExpandGroup(group);
                }
                
            }

            base.Enter();
        }

        public override void OnAfterTransition()
        {
            ToNextState();
        }

        /// <summary>
        /// Expand the group by combining blocks that are adjacent and the correct dimensions
        /// Prioritize expanding towards the largest area
        /// </summary>
        public void ExpandGroup(Group group)
        {
            int counter = 0;
            int max = board.boundary.height >= board.boundary.width ? board.boundary.height : board.boundary.width;
            bool expanded = false;

            if (group.height > group.width)
            {
                while (group.CanExpand() && counter < max + 2)
                {
                    //Expand horizontally first
                    if (group.CanExpand(Direction.right))
                    {
                        group.Expand(Direction.right);
                        expanded = true;
                    }

                    if (group.CanExpand(Direction.left))
                    {
                        group.Expand(Direction.left);
                        expanded = true;
                    }

                    if (group.CanExpand(Direction.up))
                    {
                        group.Expand(Direction.up);
                        expanded = true;
                    }

                    if (group.CanExpand(Direction.down))
                    {
                        group.Expand(Direction.down);
                        expanded = true;
                    }

                    counter++;
                }

                while (group.CanCombine())
                {
                    if (group.CanCombine(Direction.right))
                    {
                        group.Combine(Direction.right);
                        expanded = true;
                    }

                    if (group.CanCombine(Direction.left))
                    {
                        group.Combine(Direction.left);
                        expanded = true;
                    }

                    if (group.CanCombine(Direction.up))
                    {
                        group.Combine(Direction.up);
                        expanded = true;
                    }

                    if (group.CanCombine(Direction.down))
                    {
                        group.Combine(Direction.down);
                        expanded = true;
                    }
                }

            }
            else if (group.height <= group.width)
            {
                while (group.CanExpand() && counter < max + 2)
                {
                    //Expand vertically first
                    if (group.CanExpand(Direction.up))
                    {
                        group.Expand(Direction.up);
                        expanded = true;
                    }

                    if (group.CanExpand(Direction.down))
                    {
                        group.Expand(Direction.down);
                        expanded = true;
                    }

                    if (group.CanExpand(Direction.left))
                    {
                        group.Expand(Direction.left);
                        expanded = true;
                    }

                    if (group.CanExpand(Direction.right))
                    {
                        group.Expand(Direction.right);
                        expanded = true;
                    }

                    counter++;
                }

                while (group.CanCombine())
                {
                    if (group.CanCombine(Direction.up))
                    {
                        group.Combine(Direction.up);
                        expanded = true;
                    }

                    if (group.CanCombine(Direction.down))
                    {
                        group.Combine(Direction.down);
                        expanded = true;
                    }

                    if (group.CanCombine(Direction.left))
                    {
                        group.Combine(Direction.left);
                        expanded = true;
                    }

                    if (group.CanCombine(Direction.right))
                    {
                        group.Combine(Direction.right);
                        expanded = true;
                    }
                }
            }

            if (expanded == true)
            {
                group.ChangeSprites();
            }

        }

        public override void Exit()
        {
            base.Exit();
        }

        public void ToNextState() {
            if (GameState.GetValue() == false) {
                stateMachine.ChangeState<GameEndState>();
            }
            else {
                stateMachine.ChangeState<SettleState>();
            }
        }

        protected override void AddListeners()
        {

        }

        protected override void RemoveListeners()
        {

        }
    }

}
