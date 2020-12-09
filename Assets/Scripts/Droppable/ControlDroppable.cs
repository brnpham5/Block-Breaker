using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game {
    public class ControlDroppable : MonoBehaviour, IDroppable {
        public delegate void DropDelegate();
        public event DropDelegate OnDoneDropping;

        [Header("Scriptable Reference")]
        public BoundaryData boundary;
        public BoardSpace board;

        private List<Block> blocks;

        public void Setup(List<Block> blocks) {
            this.blocks = blocks;
        }

        public bool CanDrop() {
            int xOffset = (int)transform.localPosition.x;
            int yOffset = (int)transform.localPosition.y;

            int xPos;
            int yPos;

            Block block;
            for (int i = 0; i < blocks.Count; i++) {
                block = blocks[i];

                //Check if reached the bottom
                yPos = Mathf.RoundToInt(block.transform.localPosition.y + yOffset);
                if (yPos <= boundary.bottomBound) {
                    return false;
                }

                //Check for other blocks
                xPos = Mathf.RoundToInt(block.transform.localPosition.x + xOffset);
                if (board.IsOccupied(xPos, yPos - 1)) {
                    return false;
                }
            }

            return true;
        }

        public void Drop() {
            transform.localPosition += Vector3.down;
        }
        public void DoneDropping()
        {
            OnDoneDropping?.Invoke();
        }

        public Transform GetTransform()
        {
            return this.transform;
        }
    }

}
