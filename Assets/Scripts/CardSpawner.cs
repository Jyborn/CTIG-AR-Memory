﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using GameManagers;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using UI;

public class CardSpawner: MonoBehaviour {
	public GameObject cardPrefab;
	public int numPairs;
	//private string folderName = "ChristmasModels";
	public SettingsUI settingsUI;
	private string folderName;
	private ARAnchorManager anchorManager;
    private ARPlaneManager planeManager;
    private GameManager gameManager;
    [SerializeField] private PlaneDetectionUI _planeDetectionUI;
    private static ILogger logger = Debug.unityLogger;

    public void Start()
    {
	    numPairs = PlayerPrefs.GetInt("NumPairs", 2);
	    logger.Log("NUMBER OF PAIRS: " + numPairs);

		string KeyName = "Models";
		if (PlayerPrefs.HasKey(KeyName))
        {
            Debug.Log("The key " + KeyName + " exists");
        }
        else
            Debug.Log("The key " + KeyName + " does not exist");
		folderName = PlayerPrefs.GetString("Models", "ChristmasModels");
		logger.Log("FOLDER NAME: " + folderName);
		// settingsUI = FindObjectOfType<SettingsUI>();
        // if (settingsUI == null)
        // {
        //     Debug.LogError("SettingsUI script not found in the scene.");
        //     return;
        // }
        // // Access the dropdown value
        // string selectedValue = settingsUI.GetSelectedValue();
        // Debug.Log("Dropdown value in AnotherScript: " + selectedValue);
		// folderName = selectedValue;
		// Debug.Log("FOLDER NAME: " + folderName);

    }

    public void SpawnCardsOnPlanes() {
	    anchorManager = gameObject.GetComponent<ARAnchorManager>();
        planeManager = gameObject.GetComponent<ARPlaneManager>();
        gameManager = gameObject.GetComponent<GameManager>();

        if (planeManager == null)
        {
	        Debug.LogError("ARPlaneManager not found on this GameObject");
	        return;
        }
        if (numPairs > planeManager.trackables.count/2)
        {
	        return;
        }

        _planeDetectionUI.ChangeUI();
        planeManager.enabled = false;
        List<GameObject> flippedModels = GetRandomPrefabsFromFolder(folderName, numPairs);
        
        var i = 0;
        foreach (var plane in planeManager.trackables)
        {
	        if (i < numPairs * 2)
	        {
		        var model = flippedModels[i];
		        SpawnCard(plane, model);   
	        }
	        plane.gameObject.SetActive(false);
	        i++;
        }
    }

    private void SpawnCard(ARPlane targetPlane, GameObject flippedModel)
    {
	    GameObject card;
	    ARAnchor cardAnchor;
	    
	    if (targetPlane == null)
	    {
		    logger.Log("Target plane is null");
		    return;
	    }
	    
	    Vector3 spawnPosition = targetPlane.center + targetPlane.normal * 0.1f;
	    
	    card = Instantiate(cardPrefab, spawnPosition, Quaternion.identity);
	    card.tag = "SpawnedObject";
	    
	    FlippableCard fcard = card.GetComponent<FlippableCard>();
	    GameObject model = Instantiate(flippedModel);
	    fcard.modelToShowWhenFlipped = model;
	    //fcard.modelToShowWhenFlipped.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
	    logger.Log("Spawned card at " + card.transform.position);
	    
	    anchorManager = gameObject.GetComponent<ARAnchorManager>();
	    if (anchorManager == null) { return; }
	    
	    cardAnchor = anchorManager.AttachAnchor(targetPlane, new Pose(spawnPosition, Quaternion.identity));
	    
	    card.transform.parent = cardAnchor.transform;
		gameManager.MemoryCards.Add(fcard);
	    logger.Log("Added an anchor to the plane " + targetPlane);
    }
    
    /**
     * Returns random pairs of prefabs from path in a shuffled list
     */
    private List<GameObject> GetRandomPrefabsFromFolder(string path, int nPairs)
    {
	    // Load all prefabs from the specified folder
	    Object[] prefabs = Resources.LoadAll(path, typeof(GameObject));

	    if (prefabs.Length == 0)
	    {
		    Debug.LogError("No prefabs found in the folder: " + path);
		    return null;
	    }
	    ShuffleArray(prefabs);
	    List<GameObject> randomPrefabs = new List<GameObject>();
	    for (int i = 0; i < Mathf.Min(nPairs, prefabs.Length); i++)
	    {
		    randomPrefabs.Add((GameObject)prefabs[i]);
		    randomPrefabs.Add((GameObject)prefabs[i]);
	    }
		ShuffleList(randomPrefabs);
	    return randomPrefabs;
    }
    
    private void ShuffleArray(Object[] array)
    {
	    for (int i = array.Length - 1; i > 0; i--)
	    {
		    int randIndex = Random.Range(0, i + 1);
		    Object temp = array[i];
		    array[i] = array[randIndex];
		    array[randIndex] = temp;
	    }
    }
    
    private void ShuffleList<T>(List<T> list)
    {
	    int n = list.Count;
	    System.Random rng = new System.Random();

	    while (n > 1)
	    {
		    n--;
		    int k = rng.Next(n + 1);
		    T value = list[k];
		    list[k] = list[n];
		    list[n] = value;
	    }
    }

}