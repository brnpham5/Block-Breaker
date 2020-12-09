using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Game 
{
    public class MainMenuManager : MonoBehaviour
    {
        public delegate void ButtonDelegate();
        public static event ButtonDelegate OnOption;
        public static event ButtonDelegate OnTutorial;
        public static event ButtonDelegate OnScore;

        [Header("Editor Reference")]
        public Button buttonStart;
        public Button buttonOption;
        public Button buttonTutorial;
        public Button buttonScore;

        private MainMenuManager instance;

        private void Awake()
        {
            if(instance == null)
            {
                instance = this;
            } else
            {
                Destroy(this.gameObject);
            }
        }

        private void Start()
        {
            buttonStart.onClick.AddListener(OnStartPress);
            buttonOption.onClick.AddListener(OnOptionPress);
            buttonTutorial.onClick.AddListener(OnTutorialPress);
            buttonScore.onClick.AddListener(OnScorePress);
        }

        public void StartGame()
        {
            buttonStart.interactable = false;
            buttonOption.interactable = false;
            buttonTutorial.interactable = false;
            buttonScore.interactable = false;

            SceneManager.LoadScene(1);

            
        }

        public void OnOptionPress()
        {
            OnOption?.Invoke();
        }

        public void OnStartPress()
        {
            StartGame();
        }

        public void OnTutorialPress()
        {
            OnTutorial?.Invoke();
        }

        public void OnScorePress()
        {
            OnScore?.Invoke();
        }
    }

}
