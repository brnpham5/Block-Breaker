using Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class GroupScorable : MonoBehaviour
    {
        [Header("Editor Reference")]
        public Group group;
        public GroupBreakable breakable;

        [Header("Scriptable Reference")]
        public FloatVariable totalScore;
        public FloatVariable scoreMultiplier;
        public FloatVariable currentScore;
        public GroupMultiplierData data;
        
        public void AddScore()
        {
            int width = group.width;
            int height = group.height;

            float scorePerBlock = 0;
            float score;
            
            if(width <= height)
            {
                scorePerBlock = WidthMultiplier();
            } else if(width > height)
            {
                scorePerBlock = HeightMultiplier();
            }

            score = (scorePerBlock * width * height) * scoreMultiplier.GetValue();

            this.totalScore.ApplyChange(score);
            this.currentScore.ApplyChange(score);
        }

        private float WidthMultiplier()
        {
            int index = group.width - 2;

            GroupMultipliers multipliers = data.multipliers[index];

            float score;

            float multiplierModifier = multipliers.baseMult + (multipliers.incrMult * (group.height - group.width));
            score = 100 * multiplierModifier;

            return score;
        }

        private float HeightMultiplier()
        {
            int index = group.height - 2;

            GroupMultipliers multipliers = data.multipliers[index];

            float score = 0;

            float multiplierModifier = multipliers.baseMult + (multipliers.incrMult * (group.width - group.height));
            score = 100 * multiplierModifier;

            return score;
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
