using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game {
    public class UISpawner : MonoBehaviour
    {
        public Image centerBlock;
        public Image topBlock;

        public void SetBlocks(PlaceableData block1, PlaceableData block2) {
            this.centerBlock.sprite = block1.original;
            this.topBlock.sprite = block2.original;
        }

        private void OnEnable()
        {
            Spawner.OnSetNext += SetBlocks;
        }

        private void OnDisable()
        {
            Spawner.OnSetNext -= SetBlocks;
        }
    }

}
