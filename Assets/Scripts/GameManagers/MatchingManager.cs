using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;

namespace GameManagers
{

    public class MatchingManager : MonoBehaviour, GameManager
    {

        public List<FlippableCard> memoryCards;

        private int numOfValidPairs;
        private int pairs = 0;
        private static float score = 0f;
        public TextMeshProUGUI matchesText;
        public TextMeshProUGUI timeText;
 
        private AudioManager audioManager;
       
        private int pointsAddedForCorrectMatch = 10;
        private int pointsSubtractedForIncorrectMatch = 5;
        private float remainingTime;
        private CardSpawner cardSpawner;
        

        [SerializeField] private ARSession _arSession;
        public float Score
        {
            get { return ScoreManager.GlobalScore; }
            set { ScoreManager.GlobalScore = value; }
        }

        public List<FlippableCard> MemoryCards
        {
            get { return memoryCards; }
            set { memoryCards = value; }
        }

        public void GoToScoreScreen()
        {
            _arSession.Reset();
            SceneManager.LoadScene(2);
        }
        
        // Start is called before the first frame update
        void Start()
        {
            cardSpawner = gameObject.GetComponent<CardSpawner>();
            audioManager = gameObject.GetComponent<AudioManager>();
            remainingTime = DifficultyManager.Instance.TimeAvailable;
            numOfValidPairs = cardSpawner.numPairs;
           
        }

        // Update is called once per frame
        void Update()
        {
            if (pairs == numOfValidPairs)
            {
                ScoreManager.IsGameWon = true;
                Invoke("GoToScoreScreen", 5);
            }

            IList<FlippableCard> flippedCards = new List<FlippableCard>();
            foreach (var card in memoryCards)
            {
                if (card.isFlipped && !card.isFlipping)
                {
                    flippedCards.Add(card);
                }
            }

            if (flippedCards.Count >= 2)
            {
                StartCoroutine(CheckFlippedPairWithDelay(flippedCards));
            }

            if (cardSpawner.isGameStarted())
            {
                remainingTime -= Time.deltaTime;
                if (remainingTime <= 0)
                {
                    remainingTime = 0; // To avoid negative values, while loading next screen
                    Invoke("GoToScoreScreen", 5);
                }
            }
            timeText.text = "Time: " + (int) remainingTime;
        }

        private IEnumerator CheckFlippedPairWithDelay(IList<FlippableCard> cards)
        {
            bool isPair = cards[0].modelToShowWhenFlipped.name == cards[1].modelToShowWhenFlipped.name;

            if (isPair)
            {
                pairs++;
                score += pointsAddedForCorrectMatch;
                audioManager.MatchSound(true);
                Destroy(cards[0].gameObject, 1);
                Destroy(cards[0].modelToShowWhenFlipped, 1);
                Destroy(cards[1].gameObject, 1);
                Destroy(cards[1].modelToShowWhenFlipped, 1);
            }
            else
            {
                audioManager.MatchSound(false);
                score -= pointsSubtractedForIncorrectMatch;
            }
            Score = score;
            matchesText.text = "Score: " + score;
            List<Coroutine> flipCoroutines = new List<Coroutine>();

            foreach (var card in cards)
            {
                flipCoroutines.Add(StartCoroutine(card.FlipCard()));
            }

            foreach (var coroutine in flipCoroutines)
            {
                yield return coroutine;
            }

        }
    }
}
