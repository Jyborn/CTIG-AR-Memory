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
        private AudioSource audioSource;

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
            var cardSpawner = gameObject.GetComponent<CardSpawner>();
            numOfValidPairs = cardSpawner.numPairs;
            audioSource = gameObject.GetComponent<AudioSource>();
        }

        // Update is called once per frame
        void Update()
        {
            if (pairs == numOfValidPairs)
            {   
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
        }

        private IEnumerator CheckFlippedPairWithDelay(IList<FlippableCard> cards)
        {
            bool isPair = cards[0].modelToShowWhenFlipped.name == cards[1].modelToShowWhenFlipped.name;

            if (isPair)
            {
                pairs++;
                score += Mathf.Abs(cards[0].timeOfFlip - cards[1].timeOfFlip);
                matchesText.text = "Score: " + score;
                Score = score;
                PlaySound();
                Destroy(cards[0].gameObject, 1);
                Destroy(cards[0].modelToShowWhenFlipped, 1);
                Destroy(cards[1].gameObject, 1);
                Destroy(cards[1].modelToShowWhenFlipped, 1);
            }

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
        
        void PlaySound()
        {
            if (audioSource.clip != null)
            {
                audioSource.Play();
            }
            else
            {
                Debug.LogError("AudioClip not assigned. Please assign an AudioClip to the AudioSource component.");
            }
        }
    }
}
