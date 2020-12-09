using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Block))]
    public class BlockPlaceable : MonoBehaviour, IPlaceable
    {
        [Header("Editor Reference")]
        public Block block;

        [Header("Scriptable Reference")]
        public BoardSpace board;

        [Header("Runtime Set Reference")]
        public BlockSet allBlocks;
        public BlockSet normalBlocks;
        public BlockSet breakerBlocks;
        public BlockSet diamondBlocks;

        public BlockSet blueBlocks;
        public BlockSet greenBlocks;
        public BlockSet redBlocks;
        public BlockSet yellowBlocks;

        public bool placed = false;

        public void Place()
        {
            board.UpdateGrid(block.X, block.Y, block);
            placed = true;

            AddToSets();
        }

        public void Unplace()
        {
            board.UpdateGrid(block.X, block.Y, null);
            placed = false;

            RemoveFromSets();
        }


        private void AddToSets()
        {

            allBlocks.Add(block);
            switch (block.data.type)
            {
                case BlockType.normal:
                    normalBlocks.Add(block);
                    break;
                case BlockType.breaker:
                    breakerBlocks.Add(block);
                    break;
                case BlockType.diamond:
                    diamondBlocks.Add(block);
                    break;
            }

            switch (block.data.color)
            {
                case BlockColor.blue:
                    blueBlocks.Add(block);
                    break;
                case BlockColor.green:
                    greenBlocks.Add(block);
                    break;
                case BlockColor.red:
                    redBlocks.Add(block);
                    break;
                case BlockColor.yellow:
                    yellowBlocks.Add(block);
                    break;
            }

        }

        private void RemoveFromSets()
        {
            allBlocks.Remove(block);
            switch (block.data.type)
            {
                case BlockType.normal:
                    normalBlocks.Remove(block);
                    break;
                case BlockType.breaker:
                    breakerBlocks.Remove(block);
                    break;
                case BlockType.diamond:
                    diamondBlocks.Remove(block);
                    break;
            }

            switch (block.data.color)
            {
                case BlockColor.blue:
                    blueBlocks.Remove(block);
                    break;
                case BlockColor.green:
                    greenBlocks.Remove(block);
                    break;
                case BlockColor.red:
                    redBlocks.Remove(block);
                    break;
                case BlockColor.yellow:
                    yellowBlocks.Remove(block);
                    break;
            }
        }
    }

}
