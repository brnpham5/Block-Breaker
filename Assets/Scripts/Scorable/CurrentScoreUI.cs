using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using Core;


namespace Game
{
    public class CurrentScoreUI : MonoBehaviour
    {
        [Header("Scriptable Reference")]
        public FloatVariable currentScore;

        [Header("Gameobject Reference")]
        public TMP_Text scoreText;
        public Animator anim;

        public void UpdateUI()
        {
            float sizeMultiplier = 0.0f;
            this.scoreText.text = currentScore.GetValue().ToString();

            sizeMultiplier = currentScore.GetValue() / 5000;
            if(sizeMultiplier > 1.0f)
            {
                sizeMultiplier = 1.0f;
            }

            anim.SetFloat("SizeMultiplier", sizeMultiplier);
            
            anim.Play("Change Blend Tree");
            anim.SetBool("HasChanged", true);
        }

        public void HideAnim()
        {
            anim.SetBool("HasChanged", false);
        }


        private void OnEnable()
        {
            BreakState.OnBreak += UpdateUI;
            ControlState.OnControlState += HideAnim;
        }

        private void OnDisable()
        {
            BreakState.OnBreak -= UpdateUI;
            ControlState.OnControlState -= HideAnim;
        }
    }

}
