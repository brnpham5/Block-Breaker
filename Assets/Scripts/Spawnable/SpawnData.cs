using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game {
    [CreateAssetMenu(fileName = "Spawn Data", menuName = "Config/Spawn Data")]
    public class SpawnData : ScriptableObject {
        public List<PlaceableData> blocks;
        public PlaceableData diamond;

        [Header("Weight Modifiers")]
        public float normalWeightMod;
        public float breakerWeightMod;

        [Header("Diamond Counter")]
        public int diamondCounter = 25;
    }

}
