using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource soundCorrectMatch;
    private AudioSource soundIncorrectMatch;
    private AudioSource soundClickCard;
    
    // Start is called before the first frame update
    void Start()
    {
        soundCorrectMatch = gameObject.AddComponent<AudioSource>();
        soundCorrectMatch.clip = Resources.Load("matchSound") as AudioClip;
        soundCorrectMatch.playOnAwake = false;

        soundIncorrectMatch = gameObject.AddComponent<AudioSource>();
        soundIncorrectMatch.clip = Resources.Load("failMatchSound") as AudioClip;
        soundIncorrectMatch.playOnAwake = false;
        soundIncorrectMatch.volume = 0.1f;

        soundClickCard = gameObject.AddComponent<AudioSource>();
        soundClickCard.clip = Resources.Load("clickCardSound") as AudioClip;
        soundClickCard.playOnAwake = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MatchSound(bool isPair)
    {
        if (isPair)
        {
            soundCorrectMatch.Play();
        }
        else
        {
            soundIncorrectMatch.Play();
        }
    }

    public void ClickCardSound()
    {
        soundClickCard.Play();
    }

}
