using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Game
{
    [System.Serializable]
    public class GroupMultipliers
    {
        public float baseMult; //Base multiplier
        public float incrMult; //Incremental multiplier for when height increases (more points per block the bigger the group is)
    }

    
    [CreateAssetMenu(fileName = "Group Multipliers", menuName = "Config/Group Multipliers")]
    public class GroupMultiplierData : ScriptableObject
    {
        public float baseMultipier = 1.0f;
        public float comboMultiplier = 0.1f;
        public List<GroupMultipliers> multipliers;
    }

}
