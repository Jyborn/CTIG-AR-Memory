using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using GameManagers;

public class CardSpawner: MonoBehaviour {
	public GameObject cardPrefab;
	public int numPairs= 2;
	private string folderName = "ChristmasModels";
	private ARAnchorManager anchorManager;
    private ARPlaneManager planeManager;
    private GameManager gameManager;

    private static ILogger logger = Debug.unityLogger;

    public void SpawnCardsOnPlanes() {
	    anchorManager = gameObject.GetComponent<ARAnchorManager>();
        planeManager = gameObject.GetComponent<ARPlaneManager>();
        gameManager = gameObject.GetComponent<GameManager>();
        
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
	    GameObject model = Instantiate(flippedModel);
	    fcard.modelToShowWhenFlipped = model;
	    Bounds prefabBounds = GetPrefabBounds(flippedModel);
	    Vector3 scale = new Vector3(0.5f / prefabBounds.size.x, 0.5f / prefabBounds.size.y, 0.5f / prefabBounds.size.z);
	    fcard.modelToShowWhenFlipped.transform.localScale = scale;
	    logger.Log("Spawned card at " + card.transform.position);
	    
	    anchorManager = gameObject.GetComponent<ARAnchorManager>();
	    if (anchorManager == null) { return; }

	    // Attach an anchor to the specified plane
	    cardAnchor = anchorManager.AttachAnchor(targetPlane, new Pose(spawnPosition, Quaternion.identity));

	    // Set the card's parent to the anchor
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
    
    Bounds GetPrefabBounds(GameObject prefab)
    {
	    // Instantiate the prefab temporarily to calculate its bounds
	    GameObject tempPrefab = Instantiate(prefab);
	    Bounds bounds = GetPrefabRendererBounds(tempPrefab);

	    // Destroy the temporary instance
	    Destroy(tempPrefab);

	    return bounds;
    }

    Bounds GetPrefabRendererBounds(GameObject prefab)
    {
	    // Retrieve the bounds from the first renderer in the prefab
	    Renderer[] renderers = prefab.GetComponentsInChildren<Renderer>();
	    if (renderers.Length > 0)
	    {
		    Bounds bounds = renderers[0].bounds;
		    foreach (Renderer renderer in renderers)
		    {
			    bounds.Encapsulate(renderer.bounds);
		    }
		    return bounds;
	    }
	    else
	    {
		    Debug.LogError("No renderer found in the prefab.");
		    return new Bounds(Vector3.zero, Vector3.zero);
	    }
    }
}