using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game {
    public class Rotatable : MonoBehaviour {
        [Header("Scriptable Reference")]
        public BoundaryData boundary;
        public BoardSpace board;

        public List<Block> blocks;
        private Vector3 nextPos;
        public Direction direction;

        public void Setup(List<Block> blocks) {
            this.blocks = blocks;
        }

        public void RotateBlocks() {
            for (int i = 1; i < blocks.Count; i++) {
                Vector3 pos = blocks[i].transform.localPosition;
                blocks[i].transform.localPosition = new Vector3(-pos.y, pos.x, 0);
            }

            this.direction = Direction.none;
        }

        public bool CanRotate() {
            float xOffset = transform.localPosition.x;
            float yOffset = transform.localPosition.y;
            for (int i = 1; i < blocks.Count; i++) {
                Vector3 pos = blocks[i].transform.localPosition;
                nextPos = new Vector3(-pos.y, pos.x, 0);

                //Check boundary
                if (nextPos.x + xOffset < boundary.leftBound) {
                    direction = Direction.right;
                    return false;
                } else if(nextPos.x + xOffset > boundary.rightBound) {
                    direction = Direction.left;
                    return false;
                } else if(nextPos.y + yOffset < boundary.bottomBound) {
                    direction = Direction.up;
                    return false;
                }

                //Check blocks
                if (board.IsOccupied((int)(nextPos.x + xOffset), (int)(nextPos.y + yOffset))){
                    if(nextPos.x < 0) {
                        direction = Direction.right;
                    } else if(nextPos.x > 0) {
                        direction = Direction.left;
                    } else if(nextPos.y < 0) {
                        direction = Direction.up;
                    }
                    return false;
                }

            }
            return true;
        }

        public Direction GetDirection() {
            return this.direction;
        }
    }
}
