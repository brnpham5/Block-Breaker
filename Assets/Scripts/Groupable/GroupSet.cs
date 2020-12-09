using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Core;
using System.Text.RegularExpressions;

namespace Game {
    [CreateAssetMenu(fileName = "Group Set", menuName = "Runtime/Group Set")]
    public class GroupSet : RuntimeSet<Group> {

        public Group GetGroup(Block block)
        {
            return null;
        }
    }

}
