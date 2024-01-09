using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

[Serializable]
public class SerializableARPlane
{
    public Vector3 center;
    public Quaternion rotation;
    // Add any other necessary properties

    public SerializableARPlane(ARPlane arPlane)
    {
        center = arPlane.center;
        // Populate other properties as needed
    }
}

public class PlaneDetectionHolder : MonoBehaviour
{
    public static PlaneDetectionHolder Instance { get; private set; }

    // List to store detected AR planes
    public List<SerializableARPlane> detectedPlanes = new List<SerializableARPlane>();

    private ARPlaneManager planeManager;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // Get the ARPlaneManager from the AR Session
        planeManager = FindObjectOfType<ARPlaneManager>();
    }

    void OnEnable()
    {
        // Subscribe to plane detection events
        if (planeManager != null)
        {
            planeManager.planesChanged += OnPlanesChanged;
        }
    }

    void OnDisable()
    {
        // Unsubscribe from plane detection events
        if (planeManager != null)
        {
            planeManager.planesChanged -= OnPlanesChanged;
        }
    }

    void OnPlanesChanged(ARPlanesChangedEventArgs eventArgs)
    {
        // Update the list of detected planes
        foreach (var addedPlane in eventArgs.added)
        {
            detectedPlanes.Add(new SerializableARPlane(addedPlane));
        }

        foreach (var removedPlane in eventArgs.removed)
        {
            // Optionally remove the plane from the list if it's removed
            // detectedPlanes.RemoveAll(plane => plane.center == removedPlane.center);
        }
    }
}