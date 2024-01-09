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
        // Ensure the model is initially inactive
        modelToShowWhenFlipped.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // Use setActive in Update only if the state has changed
        if (modelToShowWhenFlipped.activeSelf != isFlipped)
        {
            modelToShowWhenFlipped.SetActive(isFlipped);
        }
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
        
        Vector3 startPosition = modelToShowWhenFlipped.transform.position;
        Vector3 endPosition;
        
        if (isFlipped)
        {
            endRotation = Quaternion.Euler(0f, 0f, 0f);
            endPosition = transform.position;
        }
        else
        {
            endRotation = Quaternion.Euler(180f, 0f, 0f);
            endPosition = transform.position + Vector3.up;
        }

        // Animation of flip
        while (elapsedTime < duration)
        {
            transform.rotation = Quaternion.Lerp(startRotation, endRotation, elapsedTime / duration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensures the card is exactly at the end rotation in case of division inaccuracies
        transform.rotation = endRotation;
        modelToShowWhenFlipped.transform.position = endPosition;
        
        isFlipped = !isFlipped;
        isFlipping = false;
    }
}


