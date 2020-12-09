using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



using Core;

namespace Game
{
    public class ScoreLvlUI : MonoBehaviour
    {
        public TMP_Text lvlText;
        public IntVariable lvl;
        public Animator anim;

        public StringBuilder sb = new StringBuilder();

        private WaitForSeconds animTime = new WaitForSeconds(3.0f);

        public void UpdateUI()
        {
            sb.Clear();
            sb.Append("Lvl ");
            sb.Append(lvl.GetValue().ToString());
            lvlText.text = sb.ToString();

            
        }

        public void LvlUpAnim() {
            StopAllCoroutines();
            StartCoroutine(AnimCoroutine());
        }

        public IEnumerator AnimCoroutine() {
            anim.SetBool("IsVisible", true);

            yield return animTime;

            anim.SetBool("IsVisible", false);
        }

        private void OnEnable()
        {
            lvl.OnChange += UpdateUI;
            DropSpeedManager.OnLvl += LvlUpAnim;
        }

        private void OnDisable()
        {
            lvl.OnChange -= UpdateUI;
            DropSpeedManager.OnLvl -= LvlUpAnim;
        }
    }

}
