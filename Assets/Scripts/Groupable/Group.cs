using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Core;

namespace Game
{
    /// <summary>
    /// Group rectangular groups of blocks (Blocks)
    /// </summary>
    [RequireComponent(typeof(GroupBreakable))]
    [RequireComponent(typeof(GroupDroppable))]
    [RequireComponent(typeof(GroupPlaceable))]
    public class Group : MonoBehaviour
    {
        private delegate void DimensionDelegate();
        private DimensionDelegate OnExpand;

        private delegate Block IterateDelegate(Block block);
        private IterateDelegate OnIterate;

        [Header("Scriptable Reference")]
        public BoardSpace board;
        public BoolVariable DebugGroupExpand;

        [Header("Scene Reference")]
        public GroupBreakable breakable;
        public GroupDroppable droppable;
        public GroupPlaceable placeable;

        public List<Block> blocks = new List<Block>();
        public bool isActive;

        public GroupData data;

        public int width
        {
            get { return data.width; }
        }

        public int height
        {
            get { return data.height; }
        }

        public bool CanDrop {
            get { return droppable.CanDrop(); }
        }

        private void Awake() {
            if(breakable == null) {
                breakable = GetComponent<GroupBreakable>();
            }

            if(droppable == null) {
                droppable = GetComponent<GroupDroppable>();
            }

            if(placeable == null)
            {
                placeable = GetComponent<GroupPlaceable>();
            }
        }

        private void Start() {
            breakable.Setup(blocks);
            droppable.Setup(blocks);
            placeable.Setup(blocks);
        }

        public void Setup(GroupData data)
        {
            this.data = data;
            this.isActive = true;

            GroupBlocks();
            ChangeSprites();
        }

        public void Setdown()
        {
            data.width = 0;
            data.height = 0;
            data.topLeft = null;
            data.topRight = null;
            data.bottomLeft = null;
            data.bottomRight = null;

            blocks.Clear();
            isActive = false;
        }

        public bool Contains(Block block) {
            if (blocks.Contains(block)) {
                return true;
            }

            return false;
        }

        public bool CanExpand(Direction dir)
        {
            Block block;

            switch (dir)
            {
                case Direction.up:
                    block = data.topRight.SameAdjacent(Direction.up);

                    //Iterate left
                    if (block != null && SameColorLine(block, Direction.left, (data.width - 1)))
                    {
                        return true;
                    }
                    break;

                case Direction.right:
                    block = data.topRight.SameAdjacent(Direction.right);
                    //Iterate down
                    if (block != null && SameColorLine(block, Direction.down, (data.height - 1)))
                    {
                        return true;
                    }
                    break;

                case Direction.down:
                    block = data.bottomLeft.SameAdjacent(Direction.down);
                    //Iterate right
                    if (block != null && SameColorLine(block, Direction.right, (data.width - 1)))
                    {
                        return true;
                    }
                    break;

                case Direction.left:
                    block = data.bottomLeft.SameAdjacent(Direction.left);
                    //Iterate up
                    if (block != null && SameColorLine(block, Direction.up, (data.height - 1)))
                    {
                        return true;
                    }
                    break;
            }

            return false;
        }

        public bool CanExpand()
        {
            if (CanExpand(Direction.up)){
                return true;
            }
            else if (CanExpand(Direction.right))
            {
                return true;
            }
            else if (CanExpand(Direction.down))
            {
                return true;
            }
            else if (CanExpand(Direction.left))
            {
                return true;
            }

            return false;
        }

        public void Expand(Direction dir)
        {
            Block other1 = null;
            Block other2 = null;

            switch (dir)
            {
                case Direction.up:
                    other1 = data.topLeft.SameAdjacent(Direction.up);
                    other2 = data.topRight.SameAdjacent(Direction.up);

                    if (other1 == null || other2 == null)
                    {
                        return;
                    }

                    data.topLeft = other1;
                    data.topRight = other2;

                    //Iterate from topLeft to topRight (right) 
                    OnIterate = IterateRight;
                    OnExpand = IncrementHeight;
                    break;
                case Direction.right:
                    other1 = data.topRight.SameAdjacent(Direction.right);
                    other2 = data.bottomRight.SameAdjacent(Direction.right);

                    if (other1 == null || other2 == null)
                    {
                        return;
                    }

                    data.topRight = other1;
                    data.bottomRight = other2;

                    //Iterate from topRight to bottomRight (down)
                    OnIterate = IterateDown;
                    OnExpand = IncrementWidth;
                    break;
                case Direction.down:
                    other1 = data.bottomLeft.SameAdjacent(Direction.down);
                    other2 = data.bottomRight.SameAdjacent(Direction.down);

                    if (other1 == null || other2 == null)
                    {
                        return;
                    }

                    data.bottomLeft = other1;
                    data.bottomRight = other2;

                    //Iterate from bottomLeft to bottomRight (right)
                    OnIterate = IterateRight;
                    OnExpand = IncrementHeight;
                    break;
                case Direction.left:
                    other1 = data.topLeft.SameAdjacent(Direction.left);
                    other2 = data.bottomLeft.SameAdjacent(Direction.left);

                    if (other1 == null || other2 == null)
                    {
                        return;
                    }

                    data.topLeft = other1;
                    data.bottomLeft = other2;

                    //Iterate from topLeft to bottomLeft (down)
                    OnIterate = IterateDown;
                    OnExpand = IncrementWidth;
                    break;
                default:
                    return;
            }

            //Get blocks adjacent to the corners
            other2.groupable.SetGroup(this);

            blocks.Add(other2);
            while (!other1.Equals(other2) && other1 != null)
            {
                blocks.Add(other1);
                other1.groupable.SetGroup(this);
                other1 = OnIterate(other1);
            }

            OnExpand();

            OnIterate = null;
            OnExpand = null;
        }


        public bool CanCombine()
        {
            if (CanCombine(Direction.up))
            {
                return true;
            }
            else if (CanCombine(Direction.right))
            {
                return true;
            }
            else if (CanCombine(Direction.down))
            {
                return true;
            }
            else if (CanCombine(Direction.left))
            {
                return true;
            }

            return false;
        }


        /// <summary>
        /// Check if both corners are touching in another group, if so, combine groups
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public bool CanCombine(Direction dir)
        {
            Block other1 = null;
            Block other2 = null;

            Group otherGroup = null;

            switch (dir)
            {
                case Direction.up:
                    other1 = data.topLeft.SameAdjacent(Direction.up);
                    other2 = data.topRight.SameAdjacent(Direction.up);
                    break;
                case Direction.right:
                    other1 = data.topRight.SameAdjacent(Direction.right);
                    other2 = data.bottomRight.SameAdjacent(Direction.right);
                    break;
                case Direction.down:
                    other1 = data.bottomLeft.SameAdjacent(Direction.down);
                    other2 = data.bottomRight.SameAdjacent(Direction.down);
                    break;
                case Direction.left:
                    other1 = data.topLeft.SameAdjacent(Direction.left);
                    other2 = data.bottomLeft.SameAdjacent(Direction.left);
                    break;
                default:
                    return false;
            }

            if (other1 == null || other2 == null)
            {
                return false;
            }

            //If other1 and other 2 are grouped and if they're part of the same group
            if (other1.IsGrouped() == true && other2.IsGrouped() == true && other1.GetGroup().Equals(other2.GetGroup()))
            {
                //If other blocks are corner blocks, then it can expand
                otherGroup = other1.GetGroup();

                switch (dir)
                {
                    case Direction.up:
                        if (other1.Equals(otherGroup.data.bottomLeft) && other2.Equals(otherGroup.data.bottomRight))
                        {
                            return true;
                        }
                        break;
                    case Direction.right:
                        if (other1.Equals(otherGroup.data.topLeft) && other2.Equals(otherGroup.data.bottomLeft))
                        {
                            return true;
                        }
                        break;

                    case Direction.down:
                        if (other1.Equals(otherGroup.data.topLeft) && other2.Equals(otherGroup.data.topRight))
                        {
                            return true;
                        }
                        break;
                    case Direction.left:
                        if (other1.Equals(otherGroup.data.topRight) && other2.Equals(otherGroup.data.bottomRight))

                        {
                            return true;
                        }
                        break;
                    default:

                        break;
                }

            }

            return false;
        }

        /// <summary>
        /// Combine this group with another in (dir)ection
        /// </summary>
        /// <param name="other"></param>
        /// <param name="dir"></param>
        public void Combine(Direction dir)
        {
            Group other;

            switch (dir)
            {
                case Direction.up:
                    other = data.topLeft.GetTopAdjacent().GetGroup();
                    data.topLeft = other.data.topLeft;
                    data.topRight = other.data.topRight;
                    data.height += other.data.height;
                    other.Setdown();
                    break;
                case Direction.right:
                    other = data.topRight.GetRightAdjacent().GetGroup();
                    data.topRight = other.data.topRight;
                    data.bottomRight = other.data.bottomRight;
                    data.width += other.data.width;
                    other.Setdown();
                    break;
                case Direction.down:
                    other = data.bottomRight.GetBottomAdjacent().GetGroup();
                    data.bottomLeft = other.data.bottomLeft;
                    data.bottomRight = other.data.bottomRight;
                    data.height += other.data.height;
                    other.Setdown();
                    break;
                case Direction.left:
                    other = data.bottomLeft.GetLeftAdjacent().GetGroup();
                    data.topLeft = other.data.topLeft;
                    data.bottomLeft = other.data.bottomLeft;
                    data.width += other.data.width;
                    other.Setdown();
                    break;
            }

            GroupBlocks();
        }

        private void GroupBlocks()
        {
            Block place = data.bottomLeft;
            Block current;

            for (int currWidth = 0; currWidth < data.width; currWidth++)
            {
                current = place;

                for (int currHeight = 0; currHeight < data.height; currHeight++)
                {
                    blocks.Add(current);
                    current.groupable.SetGroup(this);
                    current = current.GetTopAdjacent();
                }

                place = place.GetRightAdjacent();
            }
        }

        public void ChangeSprites()
        {
            FormTopLeft();
            FormTop();
            FormTopRight();
            FormLeft();
            FormMiddle();
            FormRight();
            FormBottomLeft();
            FormBottom();
            FormBottomRight();
        }

        /// <summary>
        /// 1, height
        /// </summary>
        private void FormTopLeft()
        {
            
            data.topLeft.SetSprite(BlockPosition.topLeft);
        }

        /// <summary>
        /// 1 < x < width - 1, height
        /// </summary>
        private void FormTop()
        {
            int currentWidth = (data.width - 1);

            Block current = data.topRight;

            while (currentWidth > 1)
            {
                current = current.GetLeftAdjacent();
                current.SetSprite(BlockPosition.top);
                currentWidth--;
            }
        }

        /// <summary>
        /// width, height
        /// </summary>
        private void FormTopRight()
        {
            data.topRight.SetSprite(BlockPosition.topRight);
        }

        /// <summary>
        /// 1, 1 < x < height
        /// </summary>
        private void FormLeft()
        {
            int currentHeight = 1;

            Block current = data.bottomLeft;

            while (currentHeight < (data.height - 1))
            {
                current = current.GetTopAdjacent();
                current.SetSprite(BlockPosition.left);
                currentHeight++;
            }
        }

        /// <summary>
        /// 1 < x < width - 1, 1 < y < height - 1
        /// </summary>
        private void FormMiddle()
        {
            int currentWidth = 1;
            int currentHeight = 1;

            Block current;

            while (currentWidth < (data.width - 1))
            {
                while (currentHeight < (data.height - 1))
                {
                    current = board.GetBlock(data.bottomLeft.X + currentWidth, data.bottomLeft.Y + currentHeight);
                    current.SetSprite(BlockPosition.middle);
                    currentHeight++;
                }

                currentHeight = 1;
                currentWidth++;
            }
        }

        /// <summary>
        /// width, 1 < y < height - 1
        /// </summary>
        private void FormRight()
        {
            int currentHeight = (data.height - 1);

            Block current = data.topRight;

            while (currentHeight > 1)
            {
                current = current.GetBottomAdjacent();
                current.SetSprite(BlockPosition.right);
                currentHeight--;
            }
        }

        /// <summary>
        /// 1, 1
        /// </summary>
        private void FormBottomLeft()
        {
            data.bottomLeft.SetSprite(BlockPosition.bottomLeft);
        }

        /// <summary>
        /// 1 < x < width - 1, 1
        /// </summary>
        private void FormBottom()
        {
            int currentWidth = 1;

            Block current = data.bottomLeft;

            while (currentWidth < (data.width - 1))
            {
                current = current.GetRightAdjacent();
                current.SetSprite(BlockPosition.bottom);
                currentWidth++;
            }
        }

        /// <summary>
        /// 1, width
        /// </summary>
        private void FormBottomRight()
        {
            data.bottomRight.SetSprite(BlockPosition.bottomRight);
        }

        private void IncrementWidth()
        {
            data.width++;
        }

        private void IncrementHeight()
        {
            data.height++;
        }

        private Block IterateUp(Block block)
        {
            return block.SameAdjacent(Direction.up);
        }

        private Block IterateRight(Block block)
        {
            return block.SameAdjacent(Direction.right);
        }
        private Block IterateDown(Block block)
        {
            return block.SameAdjacent(Direction.down);
        }

        private Block IterateLeft(Block block)
        {
            return block.SameAdjacent(Direction.left);
        }

        private bool SameColorLine(Block block, Direction dir, int length)
        {
            if (block == null || block.IsGrouped() == true)
            {
                return false;
            }

            switch (dir)
            {
                case Direction.up:
                    for (int i = 0; i < length; i++)
                    {
                        block = block.SameAdjacent(Direction.up);

                        if (block == null || block.IsGrouped() == true)
                        {

                            return false;
                        }
                    }

                    break;

                case Direction.right:
                    for (int i = 0; i < length; i++)
                    {
                        block = block.SameAdjacent(Direction.right);
                        if (block == null || block.IsGrouped() == true)
                        {
                            return false;
                        }
                    }
                    break;

                case Direction.down:
                    for (int i = 0; i < length; i++)
                    {
                        block = block.SameAdjacent(Direction.down);
                        if (block == null || block.IsGrouped() == true)
                        {
                            return false;
                        }
                    }
                    break;

                case Direction.left:
                    for (int i = 0; i < length; i++)
                    {
                        block = block.SameAdjacent(Direction.left);
                        if (block == null || block.IsGrouped() == true)
                        {
                            return false;
                        }
                    }
                    break;
                default:
                    return false;
            }

            return true;
        }


        private void OnEnable()
        {
            breakable.OnBreak += Setdown;
            droppable.OnDoneDropping += placeable.Place;
        }

        private void OnDisable()
        {
            breakable.OnBreak -= Setdown;
            droppable.OnDoneDropping -= placeable.Place;
        }
    }

}
