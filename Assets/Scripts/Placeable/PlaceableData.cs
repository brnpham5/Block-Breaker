using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game {
    public enum BlockType {
        normal,
        breaker,
        diamond
    }

    public enum BlockColor {
        red,
        yellow,
        green,
        blue,
        all
    }

    public enum BlockPosition
    {
        original,
        topLeft,
        top,
        topRight,
        left,
        middle,
        right,
        bottomLeft,
        bottom,
        bottomRight
    }

    [CreateAssetMenu(fileName = "Block Type", menuName = "Config/Block Type")]
    public class PlaceableData : ScriptableObject {
        [Header("Configuration")]
        public BlockType type;
        public BlockColor color;
        public float weight;

        [Header("Asset Reference")]
        public Sprite original;
        public Sprite topLeft;
        public Sprite top;
        public Sprite topRight;
        public Sprite left;
        public Sprite middle_bright;
        public Sprite middle_split;
        public Sprite middle_dark;
        public Sprite right;
        public Sprite bottomLeft;
        public Sprite bottom;
        public Sprite bottomRight;

        public Block prefab;
        public ParticleSystem particleOnBreak;
    }

}
