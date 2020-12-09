using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class BGScroller : MonoBehaviour
    {
        public float scrollSpeed = 0.1f;

        public MeshRenderer mr;
        public Vector2 savedOffset;
        private Material mat;

        float yOffset = 0f;

        private void Start()
        {
            if(mr == null)
            {
                mr = GetComponent<MeshRenderer>();
                
            }
            mat = mr.material;
        }

        // Update is called once per frame
        void Update()
        {
            yOffset += Time.deltaTime * scrollSpeed;

            Vector2 offset = new Vector2(0, yOffset);
            mat.SetTextureOffset("_BaseMap", offset);

            if(yOffset > 1.0f)
            {
                yOffset = 0;
            }
        }

        private void OnDisable()
        {
            mat.SetTextureOffset("_BaseMap", savedOffset);
        }
    }

}
