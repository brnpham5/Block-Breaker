using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Core;

namespace Game
{
    public class Movable : MonoBehaviour
    {
        [Header("Scriptable Reference")]
        public BoundaryData boundary;
        public BoardSpace board;
        public List<Block> blocks;

        public void Move(Direction direction) {
            switch (direction) {
                case Direction.up:
                MoveUp();
                break;
                case Direction.right:
                MoveRight();
                break;

                case Direction.left:
                MoveLeft();
                break;
                default:

                break;
            }
        }

        public void Setup(List<Block> blocks)
        {
            this.blocks = blocks;
        }

        public void MoveUp() {
            transform.localPosition += Vector3.up;
        }

        public void MoveRight() {
            transform.localPosition += Vector3.right;

        }

        public void MoveLeft() {
            transform.localPosition += Vector3.left;
        }

        public void MoveDown() {
            transform.localPosition += Vector3.down;
        }

        public bool CanMove(Direction dir) {
            switch (dir) {
                case Direction.up:
                for (int i = 0; i < blocks.Count; i++) {
                    float pos = blocks[i].transform.localPosition.y + transform.localPosition.y;

                }
                break;

                case Direction.right:
                for (int i = 0; i < blocks.Count; i++) {
                    int xPos = (int)(blocks[i].transform.localPosition.x + transform.localPosition.x);
                    int yPos = (int)(blocks[i].transform.localPosition.y + transform.localPosition.y);
                    if (xPos >= boundary.rightBound) {
                        return false;
                    } else if(board.IsOccupied(xPos + 1, yPos)){
                        return false;
                    }
                }
                break;

                case Direction.left:
                for (int i = 0; i < blocks.Count; i++) {
                    int xPos = (int)(blocks[i].transform.localPosition.x + transform.localPosition.x);
                    int yPos = (int)(blocks[i].transform.localPosition.y + transform.localPosition.y);
                    if (xPos <= boundary.leftBound) {
                        return false;
                    } else if(board.IsOccupied(xPos - 1, yPos)){
                        return false;
                    }
                }
                break;

                default:

                break;
            }

            return true;
        }
    }
}
