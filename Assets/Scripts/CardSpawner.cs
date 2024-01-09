using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class CardSpawner: MonoBehaviour {
	public GameObject cardPrefab;
	public int numPairs= 2;
	private string folderName = "ChristmasModels";
	private ARAnchorManager anchorManager;
    private ARPlaneManager planeManager;

    private static ILogger logger = Debug.unityLogger;

    public void SpawnCardsOnPlanes() {
	    anchorManager = gameObject.GetComponent<ARAnchorManager>();
        planeManager = gameObject.GetComponent<ARPlaneManager>();
        planeManager.enabled = false;
        
        if (planeManager == null)
        {
	        Debug.LogError("ARPlaneManager not found on this GameObject");
	        return;
        }
        if (numPairs > planeManager.trackables.count/2)
        {
	        return;
        }

        List<GameObject> flippedModels = GetRandomPrefabsFromFolder(folderName, numPairs);
        Debug.Log("fliped models length " + flippedModels.Count + " planes length " + planeManager.trackables.count);
        // Loop through all the planes
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

	    // Check if the ARPlane parameter is null
	    if (targetPlane == null)
	    {
		    logger.Log("Target plane is null");
		    return;
	    }

	    // Calculate the position for spawning the card
	    Vector3 spawnPosition = targetPlane.center + targetPlane.normal * 0.1f;
	    
	    card = Instantiate(cardPrefab, spawnPosition, Quaternion.identity);
	    card.tag = "SpawnedObject";
	    
	    FlippableCard fcard = card.GetComponent<FlippableCard>();
	    fcard.modelToShowWhenFlipped = flippedModel;
	    logger.Log("Spawned card at " + card.transform.position);
	    
	    anchorManager = gameObject.GetComponent<ARAnchorManager>();
	    if (anchorManager == null) { return; }

	    // Attach an anchor to the specified plane
	    cardAnchor = anchorManager.AttachAnchor(targetPlane, new Pose(spawnPosition, Quaternion.identity));

	    // Set the card's parent to the anchor
	    card.transform.parent = cardAnchor.transform;

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