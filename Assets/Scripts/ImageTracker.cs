using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.XR.ARFoundation;

public class ImageTracker : MonoBehaviour
   
{
    ARTrackedImageManager imgTracker;

    void Awake()
    {
        imgTracker = GetComponent<ARTrackedImageManager>();
        Debug.Log("ImageTracker Awake()");
        if (imgTracker == null)
        {
            Debug.Log("imgtracker null");
        }
    }

    void OnEnable()
    {
        imgTracker.trackedImagesChanged += myEventHandler;
        Debug.Log("OnEnable");
    }

    void OnDisable()
    {
        imgTracker.trackedImagesChanged -= myEventHandler;
        Debug.Log("OnDisable");
    }

    void myEventHandler(ARTrackedImagesChangedEventArgs eventArgs)
    {
        Debug.Log("myEventHandler");
        foreach (ARTrackedImage img in eventArgs.added)
        {
            handleTracking(img);
        }
        foreach (ARTrackedImage img in eventArgs.updated)
        {
            handleTracking(img);
        }
    }

    void handleTracking (ARTrackedImage img)
    {
        if (img.trackingState == TrackingState.None)
        {
            Debug.Log("trackingState was none");
            return;
        }

        Debug.Log("Found an image: " + img.referenceImage.name + " (" + img.trackingState + ")");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
