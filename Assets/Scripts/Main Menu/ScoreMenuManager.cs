using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

using Core;

namespace Game
{
    public class ScoreMenuManager : PanelController
    {

        private List<int> scores = new List<int>();
        private List<string> dateTimes = new List<string>();

        public List<TMP_Text> scoreTexts;
        public List<TMP_Text> dateTimeTexts;

        public Button buttonExit;


        private void Start()
        {
            Hide();
            GetScores();
            SetScores();
            buttonExit.onClick.AddListener(Hide);
        }

        private void GetScores()
        {
            scores.Add(PlayerPrefs.GetInt("score_1"));
            scores.Add(PlayerPrefs.GetInt("score_2"));
            scores.Add(PlayerPrefs.GetInt("score_3"));
            scores.Add(PlayerPrefs.GetInt("score_4"));
            scores.Add(PlayerPrefs.GetInt("score_5"));
            scores.Add(PlayerPrefs.GetInt("score_6"));
            scores.Add(PlayerPrefs.GetInt("score_7"));
            scores.Add(PlayerPrefs.GetInt("score_8"));
            scores.Add(PlayerPrefs.GetInt("score_9"));
            scores.Add(PlayerPrefs.GetInt("score_10"));

            dateTimes.Add(PlayerPrefs.GetString("score_dt_1"));
            dateTimes.Add(PlayerPrefs.GetString("score_dt_2"));
            dateTimes.Add(PlayerPrefs.GetString("score_dt_3"));
            dateTimes.Add(PlayerPrefs.GetString("score_dt_4"));
            dateTimes.Add(PlayerPrefs.GetString("score_dt_5"));
            dateTimes.Add(PlayerPrefs.GetString("score_dt_6"));
            dateTimes.Add(PlayerPrefs.GetString("score_dt_7"));
            dateTimes.Add(PlayerPrefs.GetString("score_dt_8"));
            dateTimes.Add(PlayerPrefs.GetString("score_dt_9"));
            dateTimes.Add(PlayerPrefs.GetString("score_dt_10"));
        }

        private void SetScores()
        {
            for(int i = 0; i < scoreTexts.Count; i++)
            {
                dateTimeTexts[i].text = dateTimes[i];
                scoreTexts[i].text = scores[i].ToString();
            }

        }


        private void OnEnable()
        {
            MainMenuManager.OnScore += Show;
        }

        private void OnDisable()
        {
            MainMenuManager.OnScore -= Show;
        }

    }

}
