using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game {
    public class GroupSpawner : MonoBehaviour {
        [Header("Asset Reference")]
        public Group prefab;

        public List<Group> pool = new List<Group>();

        public Group GetGroup() {
            Group group = GetPool();

            return group;
        }

        public Group GetPool() {
            for(int i = 0; i < pool.Count; i++) {
                if(pool[i].isActive == false) {
                    return pool[i];
                } 
            }

            Group group = Instantiate(prefab);
            group.transform.parent = this.transform;
            group.transform.localPosition = Vector3.zero;
            pool.Add(group);

            return group;
        }
    }

}
