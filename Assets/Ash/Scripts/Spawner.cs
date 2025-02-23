using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Settings")] public float elapsedTime;
    public float currentLevel = 1;
    public int npcMax = 10;
    public int npcCount = 0;
    public int towers = 4;

    [Header("Level 1 Settings")] public float spawnMinL1;
    public float spawnMaxL1;
    public float level1Length;

    [Header("Level 2 Settings")] public float spawnMinL2;
    public float spawnMaxL2;
    public float level2Length;

    [Header("Level 3 Settings")] public float spawnMinL3;
    public float spawnMaxL3;
    public float level3Length;

    [Header("References")] [SerializeField]
    GameObject[] npcPrefabs;

    public Transform[] spawnPoints;
    public Transform[] exitLocation;

    private float spawnInterval;
    public List<Transform> mainLocations = new List<Transform>();

    public GameObject[] extraGameObjects;
    private List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>();

    public GameObject scoreUi;
    public GameObject gameOverUi;
    public TextMeshProUGUI scoreText;
    Score score;
    
    private void Start()
    {
        AudioManager.instance.PlayMusic(GetComponent<AudioSource>(), "Cool Background Music");
        foreach (GameObject obj in extraGameObjects)
        {
            SpriteRenderer objSpriteRenderer = obj.GetComponent<SpriteRenderer>();
            if (objSpriteRenderer != null)
            {
                spriteRenderers.Add(objSpriteRenderer);
            }
        }

        StartCoroutine(SpawnNPC());
        score = FindObjectOfType<Score>();
    }

    private void Update()
    {
        SortLayers();
        elapsedTime = Time.time;
        
    }

    private IEnumerator SpawnNPC()
    {
        while (true)
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
                    currentLevel = 3;
                }

                int randomSpawnIndex = UnityEngine.Random.Range(0, spawnPoints.Length);
                int randomNpcIndex = UnityEngine.Random.Range(0, npcPrefabs.Length); 
                Instantiate(npcPrefabs[randomNpcIndex], spawnPoints[randomSpawnIndex].position, Quaternion.identity);
                npcCount++;
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    public List<Transform> GetMainLocations()
    {
        return mainLocations;
    }

    void SortLayers()
    {
        float minY = -16f;
        float maxY = 10f;

        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            if (spriteRenderer != null)
            {
                int sortingOrder = (int)Mathf.Lerp(20, 3, Mathf.InverseLerp(minY, maxY, spriteRenderer.transform.position.y));
                spriteRenderer.sortingOrder = sortingOrder;
            }
        }
    }

    public void RemoveLocation(Transform location)
    {
        if (mainLocations.Contains(location))
        {
            mainLocations.Remove(location);
        }
    }
    
    public void GameOver()
    {
        Debug.Log("Game Over");
        Time.timeScale = 0;
        scoreText.text = score.score.ToString();
        scoreUi.SetActive(false);
        gameOverUi.SetActive(true);
        AudioManager.instance.StopMusic(GetComponent<AudioSource>(), "Cool Background Music");
    }
    
    public void Restart()
    {
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
    
 
    public void Quit()
    {
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
