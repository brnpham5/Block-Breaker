using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Game
{
    [CreateAssetMenu(fileName = "Level Data", menuName = "Config/Level Data")]
    public class LevelData : ScriptableObject
    {
        public List<LevelDropData> levels;
    }

    [System.Serializable]
    public class LevelDropData
    {
        public float dropSpeed;
        public int breakCount;
    }

}
