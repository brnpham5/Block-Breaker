using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// All blocks that are added to the list will 
    /// </summary>
    public class StartingBlocks : MonoBehaviour
    {
        public BlockSet activeBlocks;

        public List<Block> blocks;

        public void Setup()
        {
            blocks.ForEach(block =>
            {
                activeBlocks.Add(block);
            });
        }
    }

}
