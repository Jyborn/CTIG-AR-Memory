using System;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class InputManager : MonoBehaviour
{
    private ARRaycastManager rays;
    public Camera myCamera;
    private static ILogger logger = Debug.unityLogger;
    
    private void Start()
    {
        myCamera = gameObject.transform.Find("AR Camera").gameObject.GetComponent<Camera>();
        rays = gameObject.GetComponent<ARRaycastManager>();
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
                logger.Log ("Detected " + hit.transform.gameObject.name);
			
                FlippableCard flippableCard = hit.transform.GetComponent<FlippableCard>();

                if (flippableCard != null)
                {
                    Debug.Log("Hit a FlippableCard!");
                    StartCoroutine(flippableCard.FlipCard());
                }
                else
                {
                    // It's not a FlippableCard
                    Debug.Log("Hit something else");
                }
            }
        }
    }
}
