using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game {
    public enum BlockSpawnPosition {
        center,
        top
    }

    public class Spawner : MonoBehaviour {
        public delegate void SpawnDelegate(PlaceableData block1, PlaceableData block2);
        public static event SpawnDelegate OnSetNext;

        [Header("Scriptable Reference")]
        public SpawnData data;
        public BlockSet activeBlocks;

        [Header("Scene Reference")]
        public GameObject blockPool;

        private Dictionary<PlaceableData, List<Block>> pools = new Dictionary<PlaceableData, List<Block>>();
        public Dictionary<PlaceableData, float> weights = new Dictionary<PlaceableData, float>();

        private Vector3 secondPos = new Vector3(0, 1);
        private float totalWeight = 0f;
        public int blockCount;
        private PlaceableData nextBlock1;
        private PlaceableData nextBlock2;

        public List<float> visibleWeights = new List<float>();

        private void Awake() {
            for (int i = 0; i < data.blocks.Count; i++) {
                pools.Add(data.blocks[i], new List<Block>());
                weights.Add(data.blocks[i], data.blocks[i].weight);
                visibleWeights.Add(data.blocks[i].weight);
                totalWeight += data.blocks[i].weight;
            }

            //Add in pool for diamond, if I ever decide to improve this game, fix this
            pools.Add(data.diamond, new List<Block>());
        }

        /// <summary>
        /// Spawn next blocks, (Spawn blocks and put them into nextBlock Set)
        /// </summary>
        public void SpawnNext()
        {
            Block block = Spawn(nextBlock1);
            SetupBlock(block, BlockSpawnPosition.center);
            activeBlocks.Add(block);
            
            Block block2 = Spawn(nextBlock2);
            SetupBlock(block2, BlockSpawnPosition.top);
            activeBlocks.Add(block2);

            nextBlock1 = null;
            nextBlock2 = null;

        }

        /// <summary>
        /// Move Next Blocks to activeBlocks
        /// </summary>
        public void SetNext()
        {
            PlaceableData block1;
            if (blockCount > 0 && blockCount % data.diamondCounter == 0) {
                block1 = data.diamond;
            } else {
                block1 = RollForBlockWeighted();
            }
            blockCount++;

            PlaceableData block2;
            if (blockCount > 0 && blockCount % data.diamondCounter == 0) {
                block2 = data.diamond;
            }
            else {
                block2 = RollForBlockWeighted();
            }
            blockCount++;

            this.nextBlock1 = block1;
            this.nextBlock2 = block2;

            OnSetNext?.Invoke(block1, block2);
        }

        public Block Spawn(PlaceableData pData) {
            Block block = GetPool(pData);
            return block;
        }

        /// <summary>
        /// Roll for a block using only the base weights
        /// </summary>
        /// <returns></returns>
        public PlaceableData RollForBlock() {
            float roll = Random.Range(0f, totalWeight);
            float total = 0f;

            for (int i = 0; i < data.blocks.Count; i++) {
                total += data.blocks[i].weight;
                if(total >= roll) {
                    return data.blocks[i];
                }
            }

            return data.blocks[data.blocks.Count - 1];
        }

        /// <summary>
        /// Roll for block using weightged weights. 
        /// Returns weight to base of block that was rolled.
        /// Increases weight of blocks that were not rolled.
        /// </summary>
        /// <returns></returns>
        public PlaceableData RollForBlockWeighted()
        {
            PlaceableData block;
            PlaceableData otherBlock;

            //Calculate total weight
            totalWeight = 0;
            for (int i = 0; i < data.blocks.Count; i++)
            {
                totalWeight += weights[data.blocks[i]];
            }

            //Roll a value between 0 and total weight
            float roll = Random.Range(0f, totalWeight);
            float total = 0f;

            //Calculate index by adding weights of each item until it's greater than the roll
            for (int i = 0; i < data.blocks.Count; i++)
            {
                block = data.blocks[i];
                total += weights[block];
                if (total >= roll)
                {
                    //Return weight to normal
                    weights[block] = block.weight;

                    //Increase weights of blocks not spawned
                    for(int j = 0; j < data.blocks.Count; j++)
                    {
                        otherBlock = data.blocks[j];
                        if(block != otherBlock)
                        {
                            if (otherBlock.type.Equals(BlockType.normal))
                            {
                                weights[otherBlock] += data.normalWeightMod;
                            } else if (otherBlock.type.Equals(BlockType.breaker))
                            {
                                weights[otherBlock] += data.breakerWeightMod;
                            }
                        }

                        visibleWeights[j] = weights[otherBlock];
                    }
                    return data.blocks[i];
                }
            }

            //Return last block 
            return data.blocks[data.blocks.Count - 1];
        }

        /// <summary>
        /// Set position of block by position
        /// </summary>
        /// <param name="block"></param>
        /// <param name="pos"></param>
        public void SetupBlock(Block block, BlockSpawnPosition pos) {
            block.transform.parent = this.transform;
            switch (pos) {
                case BlockSpawnPosition.center:
                block.transform.localPosition = Vector3.zero;
                break;
                case BlockSpawnPosition.top:
                block.transform.localPosition = secondPos;
                break;
                default:

                break;
            }

            block.gameObject.SetActive(true);
        }

        /// <summary>
        /// Get block from pool
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Block GetPool(PlaceableData pData) {
            List<Block> pool = pools[pData];

            for(int i = 0; i< pool.Count; i++) {
                if (pool[i].isActiveAndEnabled == false) {
                    return pool[i];
                }
            }

            Block block = Instantiate(pData.prefab);
            pool.Add(block);

            return block;
        }

        /// <summary>
        /// Clean up spawner by removing all blocks
        /// </summary>
        public void Cleanup() {
            //Reset weights
            for(int i = 0; i < data.blocks.Count; i++)
            {
                weights[data.blocks[i]] = data.blocks[i].weight;
            }

            //Reset block count
            blockCount = 0;
        }
    }

}
