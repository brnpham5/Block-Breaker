using Core;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Game
{
    public class ScorableUI : MonoBehaviour
    {
        public TMP_Text scoreText;
        public FloatVariable score;

        private void Start()
        {
            this.scoreText.text = score.GetValue().ToString();
        }

        public void UpdateText()
        {
            this.scoreText.text = score.GetValue().ToString();
        }

        private void OnEnable()
        {
            score.OnChange += UpdateText;
        }

        private void OnDisable()
        {
            score.OnChange -= UpdateText;
        }
    }

}
