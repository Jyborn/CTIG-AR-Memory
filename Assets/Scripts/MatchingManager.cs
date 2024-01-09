using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MatchingManager : MonoBehaviour
{

    public List<FlippableCard> memoryCards;
    
    private int pairs = 0;

    public TextMeshProUGUI matchesText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        IList<FlippableCard> flippedCards = new List<FlippableCard>();
        foreach (var card in memoryCards)
        {
            if (card.isFlipped && !card.isFlipping) flippedCards.Add(card);
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
            matchesText.text = "Matches: " + pairs;
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
}
