using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Core;

namespace Game
{
    public class GameEndManager : MonoBehaviour
    {
        public delegate void GameEndDelegate();
        public static event GameEndDelegate OnGameRestart;
        public static event GameEndDelegate OnGameEnd;
        
        [Header("Scriptable Reference")]
        public FloatVariable score;
        public BoolVariable GameState;

        private List<int> scores = new List<int>();
        private List<string> dateTimes = new List<string>();


        // Start is called before the first frame update
        void Start()
        {
            GetScores();
        }

        public void EndGame()
        {
            GameState.SetValue(false);

            CheckRecord();

            OnGameEnd?.Invoke();
        }

        public void ToMainMenu()
        {
            EndGame();

            SceneManager.LoadScene(0);
        }

        public void Restart()
        {
            OnGameRestart?.Invoke();
        }

        public void GetScores()
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

        public void CheckRecord()
        {
            int newScore = Mathf.RoundToInt(score.GetValue());

            //Iterate through all current scores
            for(int i = 0; i < scores.Count; i++) {
                if(newScore > scores[i]) {
                    SaveScores(i, newScore);
                    break;
                }
            }
        }

        /// <summary>
        /// Re-save all scores
        /// </summary>
        /// <param name="saveIndex"></param>
        public void SaveScores(int saveIndex, int newScore) {
            //Push all scores down
            PushScores(saveIndex);

            //Put in current score
            SaveScore(saveIndex, newScore, System.DateTime.Now.ToString());
        }

        //Pushes all scores at index down
        public void PushScores(int index) {
            for (int i = index + 1; i < scores.Count; i++) {
                SaveScore(i, scores[i - 1], dateTimes[i - 1]);
            }
        }


        /// <summary>
        /// Saves the score in specific index (list not ordered)
        /// </summary>
        /// <param name="index"></param>
        /// <param name="score"></param>
        public void SaveScore(int index, int score, string time) {
            switch (index) {
                case 0:
                PlayerPrefs.SetInt("score_1", score);
                
                PlayerPrefs.SetString("score_dt_1", time);
                break;
                case 1:
                PlayerPrefs.SetInt("score_2", score);
                PlayerPrefs.SetString("score_dt_2", time);
                break;
                case 2:
                PlayerPrefs.SetInt("score_3", score);
                PlayerPrefs.SetString("score_dt_3", time);
                break;
                case 3:
                PlayerPrefs.SetInt("score_4", score);
                PlayerPrefs.SetString("score_dt_4", time);
                break;
                case 4:
                PlayerPrefs.SetInt("score_5", score);
                PlayerPrefs.SetString("score_dt_5", time);
                break;
                case 5:
                PlayerPrefs.SetInt("score_6", score);
                PlayerPrefs.SetString("score_dt_6", time);
                break;
                case 6:
                PlayerPrefs.SetInt("score_7", score);
                PlayerPrefs.SetString("score_dt_7", time);
                break;
                case 7:
                PlayerPrefs.SetInt("score_8", score);
                PlayerPrefs.SetString("score_dt_8", time);
                break;
                case 8:
                PlayerPrefs.SetInt("score_9", score);
                PlayerPrefs.SetString("score_dt_9", time);
                break;
                case 9:
                PlayerPrefs.SetInt("score_10", score);
                PlayerPrefs.SetString("score_dt_10", time);
                break;
                default:

                break;
            }
        }

        

        private void OnEnable()
        {
            ControlState.OnLose += EndGame;
            GameEndUI.OnRestart += Restart;
            GameEndUI.OnMainMenu += ToMainMenu;
            PauseUIManager.OnRestart += EndGame;
            PauseUIManager.OnQuit += ToMainMenu;
        }

        private void OnDisable()
        {
            ControlState.OnLose -= EndGame;
            GameEndUI.OnRestart -= Restart;
            GameEndUI.OnMainMenu -= ToMainMenu;
            PauseUIManager.OnRestart -= EndGame;
            PauseUIManager.OnQuit -= ToMainMenu;
        }
    }

}
