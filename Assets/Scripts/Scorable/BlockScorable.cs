using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Core;

namespace Game {
    public class BlockScorable : MonoBehaviour, IScorable
    {
        [Header("Editor Reference")]
        public BlockBreakable breakable;
        public Block block;
        public Groupable groupable;

        [Header("Scriptable Reference")]
        public FloatVariable totalScore;
        public FloatVariable scoreMultiplier;
        public FloatVariable currentScore;

        public void AddScore()
        {
            float score = 100 * scoreMultiplier.GetValue();
            totalScore.ApplyChange(score);
            currentScore.ApplyChange(score);
        }

        private void OnEnable()
        {
            breakable.BeforeBreak += AddScore;
        }

        private void OnDisable()
        {
            breakable.BeforeBreak -= AddScore;
        }
    }

}

