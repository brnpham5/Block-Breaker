using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using Core;


namespace Game
{
    public class ScoreBreakUI : MonoBehaviour
    {
        public TMP_Text scoreText;
        public IntVariable score;

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
