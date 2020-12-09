using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Core;
using System.Text.RegularExpressions;

namespace Game {
    /// <summary>
    /// Settle all active blocks and form groups
    /// </summary>
    public class SettleState : MonoState {
        [Header("Scriptable Reference")]
        public BoundaryData boundary;
        public BlockSet activeBlocks;
        public GroupSet activeGroups;
        public BoardSpace board;
        public BoolVariable GameState;

        [Header("Debug Configuration")]
        public BoolVariable DebugBlockPosition;
        public BoolVariable DebugBlockGrouping;

        [Header("Scene Reference")]
        public GameStateMachine stateMachine;
        public GroupSpawner groupSpawner;

        private delegate Block IterateDelegate(Block block);
        private IterateDelegate IterateVert;
        private IterateDelegate IterateHori;

        private List<Block> blocks = new List<Block>();
        private WaitForSeconds delay = new WaitForSeconds(0.25f);



        public override void Enter() {
            base.Enter();

            //Check all active blocks to see if they form a rectangle
            CheckRect();
        }

        public override void OnAfterTransition()
        { 
            StartCoroutine(SettleCoroutine());
        }

        public override void Exit() {
            base.Exit();
            activeBlocks.Items.Clear();
        }

        public void CheckRect()
        {
            Block block;
            BlockPosition pos;

            GroupData data;
 
            //Check all active regular blocks to see whether or not they are grouped
            for (int i = 0; i < activeBlocks.Count; i++)
            {
                block = activeBlocks.Items[i];

                if (block.data.type.Equals(BlockType.breaker) || block.data.type.Equals(BlockType.diamond))
                {
                    continue;
                }

                pos = board.BlockPos(block);

                Block current;

                //Find a corner then calculate the area of the group
                switch (pos)
                {
                    case BlockPosition.topLeft:
                        if(DebugBlockPosition.GetValue() == true)
                        {
                            Debug.Log("Top Left", block);
                        }
                       
                        data = CalcArea(block, Direction.down, Direction.right);
                        FormGroup(data);
                        break;

                    case BlockPosition.top:
                        if (DebugBlockPosition.GetValue() == true)
                        {
                            Debug.Log("Top", block);
                        }

                        //Get block on far left, check down
                        current = GetEndBlock(block, Direction.left, Direction.down);

                        data = CalcArea(current, Direction.down, Direction.right);
                        FormGroup(data);

                        break;

                    case BlockPosition.topRight:
                        if (DebugBlockPosition.GetValue() == true)
                        {
                            Debug.Log("Top Right", block);
                        }
                        
                        data = CalcArea(block, Direction.down, Direction.left);
                        FormGroup(data);

                        break;

                    case BlockPosition.left:
                        if (DebugBlockPosition.GetValue() == true)
                        {
                            Debug.Log("Left", block);
                        }

                        //Get block on far down
                        current = GetEndBlock(block, Direction.down, Direction.right);

                        data = CalcArea(current, Direction.up, Direction.right);
                        FormGroup(data);

                        break;

                    case BlockPosition.middle:
                        if (DebugBlockPosition.GetValue() == true)
                        {
                            Debug.Log("Middle", block);
                        }

                        //Get bottom left corner block

                        //Get block on far left, check down
                        current = GetEndBlock(block, Direction.left, Direction.down);

                        //Get block on far down, check right
                        current = GetEndBlock(current, Direction.down, Direction.right);

                        //Calculate area
                        data = CalcArea(current, Direction.up, Direction.right);
                        FormGroup(data);

                        break;

                    case BlockPosition.right:
                        if (DebugBlockPosition.GetValue() == true)
                        {
                            Debug.Log("Right", block);
                        }

                        //Get block on far up, check left
                        current = GetEndBlock(block, Direction.up, Direction.left);

                        
                        data = CalcArea(current, Direction.down, Direction.left);
                        FormGroup(data);

                        break;

                    case BlockPosition.bottomLeft:
                        if (DebugBlockPosition.GetValue() == true)
                        {
                            Debug.Log("Bottom Left", block);
                        }
                        
                        data = CalcArea(block, Direction.up, Direction.right);
                        FormGroup(data);

                        break;

                    case BlockPosition.bottom:
                        if (DebugBlockPosition.GetValue() == true)
                        {
                            Debug.Log("Bottom", block);
                        }

                        //Get block on far right, check up
                        current = GetEndBlock(block, Direction.right, Direction.up);
                        

                        data = CalcArea(current, Direction.up, Direction.left);
                        FormGroup(data);

                        break;

                    case BlockPosition.bottomRight:
                        if (DebugBlockPosition.GetValue() == true)
                        {
                            Debug.Log("Bottom Right", block);
                        }
                        
                        data = CalcArea(block, Direction.up, Direction.left);
                        FormGroup(data);

                        break;

                    default:

                        break;
                }
            }
        }

        public void FormGroup(GroupData data) {
            if (DebugBlockGrouping.GetValue() == true) {
                Debug.Log(data.width * data.height);
            }
            if (data.width * data.height >= 4) {
                Group group = groupSpawner.GetGroup();
                group.Setup(data);
                activeGroups.Add(group);
            }
        }

        /// <summary>
        /// Finds the farthest block in dir direction, that has a same color block in check direction
        /// </summary>
        /// <param name="block"></param>
        /// <param name="dir"></param>
        /// <param name="check"></param>
        private Block GetEndBlock(Block block, Direction dir, Direction checkDir)
        {
            Block current = null;
            Block next;
            Block nextAdj;

            //Set current block
            current = block;

            //Iterate left 
            next = current.SameAdjacent(dir);
            nextAdj = next?.SameAdjacent(checkDir);

            while (next != null && nextAdj != null && next.IsGrouped() == false && nextAdj.IsGrouped() == false)
            {
                current = next;
                next = current.SameAdjacent(dir);
                nextAdj = next?.SameAdjacent(checkDir);
            }

            return current;
        }

        /// <summary>
        /// Calculate area, first horizontally, then vertically, returns the larger area.
        /// </summary>
        /// <param name="block"></param>
        /// <param name="vert"></param>
        /// <param name="hori"></param>
        public GroupData CalcArea(Block block, Direction vert, Direction hori) {

            GroupData horiData = CalcAreaHori(block, vert, hori);
            GroupData vertData = CalcAreaVert(block, vert, hori);

            int horiArea = horiData.width * horiData.height;
            int vertArea = vertData.width * vertData.height;

            if (horiArea >= 4 || vertArea >= 4) {
                if (horiArea > vertArea) {
                    return horiData;
                }
                else {
                    return vertData;
                }
            }

            return new GroupData();
        }

        /// <summary>
        /// Calc area prioritizing horizontal
        /// </summary>
        /// <param name="block"></param>
        /// <param name="vert"></param>
        /// <param name="hori"></param>
        public GroupData CalcAreaHori(Block block, Direction vert, Direction hori) {
            Block place = block;
            Block current = null;
            Block cornerBlock = null;
            GroupData data = new GroupData();

            int height = boundary.height + 3;
            int currentHeight = 0;
            int width = 0;
            int currentWidth = 0;

            switch (hori) {
                case Direction.left:
                IterateHori = SameLeft;
                break;
                case Direction.right:
                IterateHori = SameRight;
                break;
                default:
                return data;
            }

            switch (vert) {
                case Direction.up:
                IterateVert = SameUp;
                break;

                case Direction.down:
                IterateVert = SameDown;
                break;
                default:
                return data;
            }

            //Iterate prioritizing horizontal
            while (place != null && place.IsGrouped() == false) {
                currentHeight = 0;
                current = place;

                //Check if current block is null,.IsGrouped(), or passed the height
                while (current != null && current.IsGrouped() == false && currentHeight < height) {
                    //This currently priorites horizontal rectangles
                    if (currentHeight >= 2) {
                        cornerBlock = current;
                    }

                    //Iterate vertically to find height
                    current = IterateVert(current);

                    currentHeight++;
                }

                //Iterate horizontally
                place = IterateHori(place);

                if (currentHeight >= 2) {
                    width++;

                    //Set height to shortest height
                    if (height > currentHeight)
                    {
                        height = currentHeight;
                    }
                } else
                {
                    return data;
                }

                currentWidth++;
            }

            if (height == boundary.height + 3 || width <= 1 || height <= 1)
            {
                return data;
            }

            data.width = width;
            data.height = height;

            width = width - 1;
            height = height - 1;

            int x = block.X;
            int y = block.Y;

            //Corners will be opposite of the direction the data points (up and left means block is bottom right corner
            if (vert.Equals(Direction.up) && hori.Equals(Direction.left)) {
                data.topLeft = board.GetBlock(x - width, y + height);
                data.topRight = board.GetBlock(x, y + height);
                data.bottomLeft = board.GetBlock(x - width, y);
                data.bottomRight = block;
            }
            else if (vert.Equals(Direction.up) && hori.Equals(Direction.right)) {
                data.topLeft = board.GetBlock(x, y + height);
                data.topRight = board.GetBlock(x + width, y + height);
                data.bottomLeft = block;
                data.bottomRight = board.GetBlock(x + width, y);
            }
            else if (vert.Equals(Direction.down) && hori.Equals(Direction.left)) {
                data.topLeft = board.GetBlock(x - width, y);
                data.topRight = block;
                data.bottomLeft = board.GetBlock(x - width, y - height);
                data.bottomRight = board.GetBlock(x, y - height);
            }
            else if (vert.Equals(Direction.down) && hori.Equals(Direction.right)) {
                data.topLeft = block;
                data.topRight = board.GetBlock(x + width, y);
                data.bottomLeft = board.GetBlock(x, y - height);
                data.bottomRight = board.GetBlock(x + width, y - height);
            }

            return data;

        }

        /// <summary>
        /// Calculate area prioritizing vertically
        /// </summary>
        /// <param name="block"></param>
        /// <param name="vert"></param>
        /// <param name="hori"></param>
        public GroupData CalcAreaVert(Block block, Direction vert, Direction hori) {
            Block place = block;
            Block current = null;
            Block cornerBlock = null;
            GroupData data = new GroupData();

            int height = 0;
            int currentHeight = 0;
            int width = boundary.width + 1;
            int currentWidth = 0;

            switch (hori) {
                case Direction.left:
                IterateHori = SameLeft;
                break;
                case Direction.right:
                IterateHori = SameRight;
                break;
                default:
                return data;
            }

            switch (vert) {
                case Direction.up:
                IterateVert = SameUp;
                break;

                case Direction.down:
                IterateVert = SameDown;
                break;
                default:
                return data;
            }

            //Iterate prioritizing vertical
            while (place != null && place.IsGrouped() == false) {
                currentWidth = 0;
                current = place;

                //Check if current block is null,.IsGrouped(), or passed the width
                while (current != null && current.IsGrouped() == false && currentWidth < width) {
                    //This currently priorites vertical rectangles
                    if (currentWidth >= 2) {
                        cornerBlock = current;
                    }

                    current = IterateHori(current);

                    currentWidth++;
                }

                //Iterate vertically
                place = IterateVert(place);

                if (currentWidth >= 2) {
                    height++;
                    //Set height to shortest height
                    if (width > currentWidth)
                    {
                        width = currentWidth;
                    }
                }
                else
                {
                    return data;
                }


                currentHeight++;
            }

            if(width == boundary.width + 1 || width <= 1 || height <= 1)
            {
                return data;
            }

            data.width = width;
            data.height = height;

            width = width - 1;
            height = height - 1;

            int x = block.X;
            int y = block.Y;

            //Corners will be opposite of the direction the data points (up and left means block is bottom right corner
            if (vert.Equals(Direction.up) && hori.Equals(Direction.left)) {
                data.topLeft = board.GetBlock(x - width, y + height);
                data.topRight = board.GetBlock(x, y + height);
                data.bottomLeft = board.GetBlock(x - width, y);
                data.bottomRight = block;
            }
            else if (vert.Equals(Direction.up) && hori.Equals(Direction.right)) {
                data.topLeft = board.GetBlock(x, y + height);
                data.topRight = board.GetBlock(x + width, y + height);
                data.bottomLeft = block;
                data.bottomRight = board.GetBlock(x + width, y);
            }
            else if (vert.Equals(Direction.down) && hori.Equals(Direction.left)) {
                data.topLeft = board.GetBlock(x - width, y);
                data.topRight = block;
                data.bottomLeft = board.GetBlock(x - width, y - height);
                data.bottomRight = board.GetBlock(x, y - height);
            }
            else if (vert.Equals(Direction.down) && hori.Equals(Direction.right)) {
                data.topLeft = block;
                data.topRight = board.GetBlock(x + width, y);
                data.bottomLeft = board.GetBlock(x, y - height);
                data.bottomRight = board.GetBlock(x + width, y - height);
            }

            return data;
        }

        

        private Block SameUp(Block block) {
            return block.SameAdjacent(Direction.up);
        }

        private Block SameRight(Block block) {
            return block.SameAdjacent(Direction.right);
        }

        private Block SameDown(Block block) {
            return block.SameAdjacent(Direction.down);
        }

        private Block SameLeft(Block block) {
            return block.SameAdjacent(Direction.left);
        }

        public IEnumerator SettleCoroutine() {
            yield return delay;
            ToNextState();
        }

        public void ToNextState() {
            if (GameState.GetValue() == false) {
                stateMachine.ChangeState<GameEndState>();
            }
            else {
                stateMachine.ChangeState<BreakState>();
            }
        }

        protected override void AddListeners() {

        }

        protected override void RemoveListeners() {

        }
    }
}

