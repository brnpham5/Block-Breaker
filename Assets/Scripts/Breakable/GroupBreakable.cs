using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game {
    public class GroupBreakable : MonoBehaviour, IBreakable {
        public delegate void BreakDelegate();
        public event BreakDelegate OnBreak;
        public event BreakDelegate BeforeBreak;

        public List<Block> blocks;

        public void Setup(List<Block> blocks) {
            this.blocks = blocks;
        }

        public void Break() {
            BeforeBreak?.Invoke();

            IBreakable breakable;
            for (int i = 0; i < blocks.Count; i++) {
                breakable = blocks[i].breakable;

                breakable.Break();
            }

            OnBreak?.Invoke();
        }

        public void CleanBreak()
        {
            IBreakable breakable;
            for (int i = 0; i < blocks.Count; i++)
            {
                breakable = blocks[i].breakable;

                breakable.CleanBreak();
            }

            OnBreak?.Invoke();
        }
    }

}
