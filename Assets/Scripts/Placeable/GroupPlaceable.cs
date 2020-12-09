using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class GroupPlaceable : MonoBehaviour, IPlaceable
    {
        [Header("Editor Reference")]

        private List<Block> blocks;

        public void Setup(List<Block> blocks)
        {
            this.blocks = blocks;
        }
        
        public void Place()
        {
            blocks.ForEach(block =>
            {
                block.placeable.Place();
            });
        }

        public void Unplace()
        {
            blocks.ForEach(block =>
            {
                block.placeable.Unplace();
            });
        }
    }

}
