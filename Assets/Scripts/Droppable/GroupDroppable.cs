using UnityEngine;
using System;
using System.Collections.Generic;


namespace Game {
    public class GroupDroppable : MonoBehaviour, IDroppable {
        public delegate void DropDelegate();
        public event DropDelegate OnDoneDropping;

        [Header("Scene Reference")]
        public Group group;

        private List<Block> blocks;

        public void Setup(List<Block> blocks) {
            this.blocks = blocks;
        }

        public bool CanDrop() {
            for(int i = 0; i < blocks.Count; i++)
            {
                if (blocks[i].droppable.CanDrop() == false)
                {
                    return false;
                }
            }

            return true;
        }

        public void Drop() {
            for (int i = 0; i < blocks.Count; i++) {
                blocks[i].droppable.Drop();
            }
        }
        public void DoneDropping()
        {
            OnDoneDropping?.Invoke();
        }

        public Transform GetTransform()
        {
            return group.data.bottomLeft.transform;
        }
    }

}
