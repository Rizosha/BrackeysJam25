using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;

public class Spawner : MonoBehaviour
{
    
    [Header("Settings")]
    public float elapsedTime;
    public float currentLevel = 1;
    public int npcMax = 10;
    public int npcCount = 0;
    
    [Header("Level 1 Settings")]
    public float spawnMinL1;
    public float spawnMaxL1;
    public float level1Length;
    
    [Header("Level 2 Settings")]
    public float spawnMinL2;
    public float spawnMaxL2;
    public float level2Length;
    
    [Header("Level 3 Settings")]
    public float spawnMinL3;
    public float spawnMaxL3;
    public float level3Length;
    
    
    
    
    [Header("References")]
    [SerializeField] GameObject npcPrefab;
    public Transform[] spawnPoints;
    public Transform[] exitLocation;
    
    private float spawnInterval;
    public List<Transform> location1 = new List<Transform>();
    public List<Transform> location2 = new List<Transform>();
    public List<Transform> location3 = new List<Transform>();
    public List<Transform> location4 = new List<Transform>();
    public List<Transform> location5 = new List<Transform>();
    public List<Transform> location6 = new List<Transform>();
    public List<Transform> location7 = new List<Transform>();
    public List<Transform> location8 = new List<Transform>();

    private List<List<Transform>> allLocations = new List<List<Transform>>();
   
    
    public GameObject[] extraGameObjects;
    private List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>();
    
   

    private void Start()
    {
        allLocations.Add(location1);
        allLocations.Add(location2);
        allLocations.Add(location3);
        allLocations.Add(location4);
        allLocations.Add(location5);
        allLocations.Add(location6);
        allLocations.Add(location7);
        allLocations.Add(location8);

        
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
        elapsedTime = Time.time;
    }

    private IEnumerator SpawnNPC()
    {
        while (true) // Loop indefinitely
        {
            if (npcCount < npcMax)
            {
                if (elapsedTime < level1Length)
                {
                    spawnInterval = UnityEngine.Random.Range(spawnMinL1, spawnMaxL1);
                    currentLevel = 1;
                }
                else if (elapsedTime < level1Length + level2Length)
                {
                    spawnInterval = UnityEngine.Random.Range(spawnMinL2, spawnMaxL2);
                    currentLevel = 2;
                }
                else if (elapsedTime < level1Length + level2Length + level3Length)
                {
                    spawnInterval = UnityEngine.Random.Range(spawnMinL3, spawnMaxL3);
                    currentLevel = 3;
                }
                else
                {
                    currentLevel = 4;
                }

                int randomSpawnIndex = UnityEngine.Random.Range(0, spawnPoints.Length);
                Instantiate(npcPrefab, spawnPoints[randomSpawnIndex].position, Quaternion.identity);
                npcCount++;
            }

            yield return new WaitForSeconds(spawnInterval);
        }
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
