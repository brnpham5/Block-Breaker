using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Groupable))]
    [RequireComponent(typeof(Droppable))]
    [RequireComponent(typeof(BlockBreakable))]
    [RequireComponent(typeof(BlockPlaceable))]
    public class Block : MonoBehaviour
    {
        [Header("Scriptable Reference")]
        public BoardSpace board;
        public BoundaryData boundary;
        public PlaceableData data;

        [Header("Editor Reference")]
        public Groupable groupable;
        public Droppable droppable;
        public BlockBreakable breakable;
        public BlockPlaceable placeable;
        public SpriteRenderer sr;

        public int X {
            get {
                float xOffset = transform.parent.localPosition.x;
                int x = (int)(transform.localPosition.x + xOffset);
                return x;
            }
        }

        public int Y {
            get {
                float yOffset = transform.parent.localPosition.y;
                int y = (int)(transform.localPosition.y + yOffset);
                return y;
            }
        }

        public BlockColor color
        {
            get
            {
                return data.color;
            }
        }

        private void Awake() {
            if (groupable == null) {
                groupable = GetComponent<Groupable>();
            }

            if (droppable == null) {
                droppable = GetComponent<Droppable>();
            }

            if(breakable == null) {
                breakable = GetComponent<BlockBreakable>();
            }

            if(placeable == null)
            {
                placeable = GetComponent<BlockPlaceable>();
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            breakable.Setup(data.particleOnBreak);
        }

        public bool IsGrouped()
        {
            return groupable.isGrouped;
        }

        public Group GetGroup() {
            if(groupable.group != null) {
                return groupable.group;
            }

            return null;
        }

        public void Break()
        {
            placeable.Unplace();
            SetSprite(BlockPosition.original);
            groupable.UnsetGroup();
            gameObject.SetActive(false);
        }

        public Block SameAdjacent(Direction dir)
        {
            Block block = null;

            switch (dir)
            {
                case Direction.up:
                    block = GetTopAdjacent();
                    if (SameColor(block) && SameType(block))
                    {
                        return block;
                    }
                    break;
                case Direction.right:
                    block = GetRightAdjacent();
                    if (SameColor(block) && SameType(block))
                    {
                        return block;
                    }
                    break;
                case Direction.down:
                    block = GetBottomAdjacent();
                    if (SameColor(block) && SameType(block))
                    {
                        return block;
                    }
                    break;
                case Direction.left:
                    block = GetLeftAdjacent();
                    if (SameColor(block) && SameType(block))
                    {
                        return block;
                    }
                    break;
                default:
                    return null;
            }

            return null;
        }

        public Block GetTopLeft()
        {
            return board.GetBlock(X - 1, Y + 1);
        }

        public Block GetTopAdjacent()
        {
            return board.GetBlock(X, Y + 1);
        }

        public Block GetTopRight()
        {
            return board.GetBlock(X + 1, Y + 1);
        }

        public Block GetLeftAdjacent()
        {
            return board.GetBlock(X - 1, Y);
        }

        public Block GetRightAdjacent()
        {
            return board.GetBlock(X + 1, Y);
        }

        public Block GetBottomLeft()
        {
            return board.GetBlock(X - 1, Y - 1);
        }


        public Block GetBottomAdjacent()
        {
            return board.GetBlock(X, Y - 1);
        }

        public Block GetBottomRight()
        {
            return board.GetBlock(X + 1, Y - 1);
        }

        public void SetSprite(BlockPosition pos) {
            switch (pos) {
                case BlockPosition.topLeft:
                this.sr.sprite = data.topLeft;
                break;
                case BlockPosition.top:
                this.sr.sprite = data.top;
                break;
                case BlockPosition.topRight:
                this.sr.sprite = data.topRight;
                break;
                case BlockPosition.left:
                this.sr.sprite = data.left;
                break;
                case BlockPosition.middle:
                this.sr.sprite = data.middle_bright;
                break;
                case BlockPosition.right:
                this.sr.sprite = data.right;
                break;
                case BlockPosition.bottomLeft:
                this.sr.sprite = data.bottomLeft;
                break;
                case BlockPosition.bottom:
                this.sr.sprite = data.bottom;
                break;
                case BlockPosition.bottomRight:
                this.sr.sprite = data.bottomRight;
                break;
                default:
                this.sr.sprite = data.original;
                break;
            }
        }

        public bool SameColor(Block block) {
            return block != null && data.color.Equals(block.data.color);
        }

        public bool SameType(Block block)
        {
            return block != null && data.type.Equals(block.data.type);
        }

        private void OnEnable()
        {
            breakable.OnBreak += Break;
            droppable.OnDoneDropping += placeable.Place;
        }

        private void OnDisable()
        {
            breakable.OnBreak -= Break;
            droppable.OnDoneDropping -= placeable.Place;
        }
    }

}
