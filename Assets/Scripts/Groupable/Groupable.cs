using Core;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;


namespace Game {
    public class Groupable: MonoBehaviour {
        [Header("Scene Reference")]
        public Block block;

        public bool isGrouped;
        public Group group;

        public void SetGroup(Group group)
        {
            this.group = group;
            isGrouped = true;
        }

        public void UnsetGroup()
        {
            this.group = null;
            isGrouped = false;
        }
    }
}
