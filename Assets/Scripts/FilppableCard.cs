using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FlippableCard : MonoBehaviour
{

    public bool isFlipped = false;
    public bool isFlipping = false;
    public GameObject modelToShowWhenFlipped;
    
    private static ILogger logger = Debug.unityLogger;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        modelToShowWhenFlipped.SetActive(isFlipped);
    }

    public IEnumerator FlipCard()
    {   
        if (isFlipping)
        {
            yield break;
        }
        isFlipping = true;
        
        float duration = 1.0f; // Set the duration of the flip
        float elapsedTime = 0f;

        Quaternion startRotation = transform.rotation;
        Quaternion endRotation;

        if (isFlipped)
        {
            endRotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else
        {
            endRotation = Quaternion.Euler(180f, 0f, 0f);
        }
        
        // Animation of flip
        while (elapsedTime < duration )
        {
            transform.rotation = Quaternion.Lerp(startRotation, endRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensures the card is exactly at the end rotation in case of division inaccuracies
        transform.rotation = endRotation;
        isFlipped = !isFlipped;
        
        isFlipping = false;
    }
}


