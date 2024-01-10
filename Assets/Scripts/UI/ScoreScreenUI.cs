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
        private void Start()
        {
            float scoreFromPreviousGame = ScoreManager.GlobalScore;
            matchesText.text = "Score: " + scoreFromPreviousGame;

        }

        public void OnNextButton()
        {
            SceneManager.LoadScene(0);
        }
    }
}