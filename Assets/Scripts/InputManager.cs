using System;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class InputManager : MonoBehaviour
{
    public Camera myCamera;
    private AudioManager audioManager;

    private void Start()
    {
        myCamera = gameObject.transform.Find("Camera Offset/Main Camera").gameObject.GetComponent<Camera>();
        audioManager = gameObject.GetComponent<AudioManager>();
    }

    void FixedUpdate()
    {

        if (Input.touchCount == 1)
        {

            RaycastHit[] myHits;
            Ray r;
		
            r = myCamera.ScreenPointToRay(Input.GetTouch(0).position);

            myHits = Physics.RaycastAll(r);

            foreach (RaycastHit hit in myHits) {

                FlippableCard flippableCard = hit.transform.GetComponent<FlippableCard>();

                if (flippableCard != null)
                {
                    audioManager.ClickCardSound();
                    Debug.Log("Hit a FlippableCard with model " + flippableCard.modelToShowWhenFlipped.name);
                    StartCoroutine(flippableCard.FlipCard());
                }

            }
        }
    }
}
