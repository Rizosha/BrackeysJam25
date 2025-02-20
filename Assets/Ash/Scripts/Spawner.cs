using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Spawner : MonoBehaviour
{
    //todo
    
    // ***Get prefab
    // ***Get spawn point
    // ***Instantiate prefab at spawn point
    // ***Spawn at set/random intervals
    // ***Have a list of locations npc can walk to
    // ***Randomly select a location from the list
    // ***Set the target location of npc to the selected location
    // ***npc walks to the location
    // ***When the npc reaches location, remove location from list so another npc cant walk to the same location
    // ***Set a timer for the npc to wait at the location
    // ***When timer is up, add location back to list
    // ***When npc has visited all 4 locations, leave the scene
    
    //todo
    // ***fix the npc walking to the same location
    // ***some locations are not being added to the list because they have been removed already
    
    
    [SerializeField] GameObject npcPrefab;
    public Transform spawnPoint;
    public float spawnInterval;
    public int npcMax = 10;
    public Transform exitLocation;

    public List<Transform> location1 = new List<Transform>();
    public List<Transform> location2 = new List<Transform>();
    public List<Transform> location3 = new List<Transform>();
    public List<Transform> location4 = new List<Transform>();

    private List<List<Transform>> allLocations = new List<List<Transform>>();
    public int npcCount = 0;
    
    public GameObject[] extraGameObjects;
    private List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>();

    private void Start()
    {
        allLocations.Add(location1);
        allLocations.Add(location2);
        allLocations.Add(location3);
        allLocations.Add(location4);

        // Add extra game objects' SpriteRenderers to the list
        foreach (GameObject obj in extraGameObjects)
        {
            SpriteRenderer objSpriteRenderer = obj.GetComponent<SpriteRenderer>();
            if (objSpriteRenderer != null)
            {
                spriteRenderers.Add(objSpriteRenderer);
            }
        }
        
        StartCoroutine(SpawnNPC());
    }

    private void Update()
    {
        SortLayers();
    }

    private IEnumerator SpawnNPC()
    {
        while (npcCount < npcMax)
        {
            Instantiate(npcPrefab, spawnPoint.position, Quaternion.identity);
            npcCount++;
            spawnInterval = UnityEngine.Random.Range(2f, 6f);
            yield return new WaitForSeconds(spawnInterval);
        }
        
        spawnInterval = UnityEngine.Random.Range(2f, 3f);
        yield return new WaitForSeconds(spawnInterval);
        StartCoroutine(SpawnNPC());
    }

    public List<List<Transform>> GetAllLocations()
    {
        return allLocations;
    }

    public void AddLocation(Transform location, int listIndex)
    {
        allLocations[listIndex].Add(location);
    }

    public void RemoveLocation(Transform location, int listIndex)
    {
        allLocations[listIndex].Remove(location);
    }
    
    void SortLayers()
    {
        float minY = -16f;
        float maxY = 10f;

        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            int sortingOrder = (int)Mathf.Lerp(20, 3, Mathf.InverseLerp(minY, maxY, spriteRenderer.transform.position.y));
            spriteRenderer.sortingOrder = sortingOrder;
        }
    }
    
    
}
