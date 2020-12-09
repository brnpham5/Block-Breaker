using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game {
    [CreateAssetMenu(fileName = "Board Space", menuName ="Runtime/Board Space")]
    public class BoardSpace : ScriptableObject {
        [Header("Scriptable Reference")]
        public BoundaryData boundary;

        public Block[,] grid;

        public void UpdateGrid(int x, int y, Block block) {
            x -= boundary.leftBound;
            y += boundary.height;

            grid[x, y] = block;
        }

        public bool IsOccupied(int x, int y) {
            x -= boundary.leftBound;
            y += boundary.height;

            if (x > boundary.width || x < 0 || y > boundary.height + 3 || y < 0) {
                return false;
            }

            if (grid[x,y] != null) {
                return true;
            } else {
                return false;
            }
        }

        public Block GetBlock(int x, int y)
        {
            x -= boundary.leftBound;
            y += boundary.height;
            if (x >= boundary.width || x < 0 || y >= boundary.height + 3 || y < 0)
            {
                return null;
            }

            return grid[x, y];
        }

        public BlockPosition BlockPos(Block block)
        {
            bool up = false, right = false, down = false, left = false;
            bool upperLeft = false, upperRight = false, lowerLeft = false, lowerRight = false;

            int adjacentCount = 0;

            Block other = block.SameAdjacent(Direction.up);

            //Check directly adjacent blocks
            if (other != null && other.IsGrouped() == false)
            {
                adjacentCount++;
                up = true;
            }

            other = block.SameAdjacent(Direction.right);
            if (other != null && other.IsGrouped() == false)
            {
                adjacentCount++;
                right = true;
            }

            other = block.SameAdjacent(Direction.down);
            if (other != null && other.IsGrouped() == false)
            {
                adjacentCount++;
                down = true;
            }

            other = block.SameAdjacent(Direction.left);
            if (other != null && other.IsGrouped() == false)
            {
                adjacentCount++;
                left = true;
            }

            //If 3 or more adjacent, check diagonals too
            if(adjacentCount >= 3)
            {
                //check upper diagonals
                other = GetBlock(block.X - 1, block.Y + 1);
                if (block.SameColor(other) && other.IsGrouped() == false)
                {
                    upperLeft = true;
                }

                other = GetBlock(block.X + 1, block.Y + 1);
                if (block.SameColor(other) && other.IsGrouped() == false)
                {
                    upperRight = true;
                }

                //Check lower diagonals
                other = GetBlock(block.X - 1, block.Y - 1);
                if (block.SameColor(other) && other.IsGrouped() == false)
                {
                    lowerLeft = true;
                }

                other = GetBlock(block.X + 1, block.Y - 1);
                if (block.SameColor(other) && other.IsGrouped() == false)
                {
                    lowerRight = true;
                }

            }

            if (adjacentCount == 2)
            {
                //Corner block
                if (up && right)
                {
                    //bottom left
                    return BlockPosition.bottomLeft;

                }
                else if (right && down)
                {
                    //top left
                    return BlockPosition.topLeft;
                }
                else if (down && left)
                {
                    // top right
                    return BlockPosition.topRight;
                }
                else if (left && up)
                {
                    //bottom right
                    return BlockPosition.bottomRight;
                }

                //3 blocks in a row

            }
            else if (adjacentCount == 3)
            {
                //T shape

                //Edge of rect
                if (up && right && down)
                {
                    //left edge
                    if (upperRight && lowerRight)
                    {
                        return BlockPosition.left;
                    }

                    //bottom left
                    if (upperRight)
                    {
                        return BlockPosition.bottomLeft;
                    }

                    //top right
                    if (lowerRight)
                    {
                        return BlockPosition.topLeft;
                    }

                }
                else if (right && down && left)
                {
                    //top edge
                    if (lowerLeft && lowerRight)
                    {
                        return BlockPosition.top;
                    }

                    //top right
                    if (lowerLeft)
                    {
                        return BlockPosition.topRight;
                    }

                    //top left
                    if (lowerRight)
                    {
                        return BlockPosition.topLeft;
                    }

                }
                else if (down && left && up)
                {
                    //right edge
                    if (upperLeft && lowerLeft)
                    {
                        return BlockPosition.right;
                    }

                    //bottom right
                    if (upperLeft)
                    {
                        return BlockPosition.bottomRight;
                    }

                    //top right
                    if (lowerLeft)
                    {
                        return BlockPosition.topRight;
                    }
                }
                else if (left && up && right)
                {

                    //bottom edge
                    if (upperLeft && upperRight)
                    {
                        return BlockPosition.bottom;
                    }

                    //bottom left corner
                    if (upperRight)
                    {
                        return BlockPosition.bottomLeft;
                    }

                    //bottom right corner
                    if (upperLeft)
                    {
                        return BlockPosition.bottomRight;
                    }
                }
            }
            else if (adjacentCount == 4)
            {
                //+ shape
                //+ shape + 1 corner = group
                if(upperLeft || upperRight || lowerLeft || lowerRight)
                {
                    return BlockPosition.middle;
                }
                

                //Middle block of group
                if (upperLeft && upperRight && lowerLeft && lowerRight)
                {
                    return BlockPosition.middle;
                }

            }

            return BlockPosition.original;
        }

        private void CreateGrid() {
            grid = new Block[boundary.width, boundary.height + 3];
        }

        private void OnEnable() {
            CreateGrid();
        }

        private void OnValidate() {
            CreateGrid();
        }
    }

}
