using System;
using UnityEngine;
using DefaultNamespace;
using TMPro;
using UnityEngine.SceneManagement;

namespace UI
{
    public class ScoreScreenUI : MonoBehaviour
    {
        public TextMeshProUGUI matchesText;
        public TextMeshProUGUI congratsText;
        private void Start()
        {
            float scoreFromPreviousGame = ScoreManager.GlobalScore;
            matchesText.text = "Score: " + scoreFromPreviousGame;

            if (ScoreManager.IsGameWon)
            {
                congratsText.text = "CONGRATULATIONS!!! \n \n YOU COMPLETED THE GAME. PLAY AGAIN AND TRY TO GET A HIGHER SCORE!";
            }
            else
            {
                congratsText.text = "Unfortunately you ran out of time. \n \n Better luck next time! ";
            }
            

        }

        public void OnNextButton()
        {
            SceneManager.LoadScene(0);
        }
    }
}