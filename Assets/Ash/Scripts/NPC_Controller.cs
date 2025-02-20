using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC_Controller : MonoBehaviour
{
    NavMeshAgent agent;
    Spawner spawner;
    public event Action OnDestroyEvent;

    private List<List<Transform>> npcLocations = new List<List<Transform>>();
    public HashSet<int> visitedLists = new HashSet<int>();
    private int currentListIndex;
    private Transform currentTarget;
    private int spawnTimer;

    public bool grabbing = false;

    public GameObject grabMarker;

    Animator animator;
    public Vector2 podiumDir;
    private Vector2 direction;
    [SerializeField] private SpriteRenderer spriteRenderer;
    public GameObject[] extraGameObjects; 

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        spawner = GameObject.FindWithTag("GameController").GetComponent<Spawner>();
        npcLocations = new List<List<Transform>>(spawner.GetAllLocations());

        grabMarker = gameObject.transform.Find("Grab").gameObject;

        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        MoveToPoint();
    }

    private void Update()
    {
        UpdateAnimator();
        SortLayers();
    }

    private void MoveToPoint()
    {
        if (visitedLists.Count < 4)
        {
            do
            {
                currentListIndex = UnityEngine.Random.Range(0, npcLocations.Count);
            } while (visitedLists.Contains(currentListIndex));

            if (npcLocations[currentListIndex].Count > 0)
            {
                int randomLocationIndex = UnityEngine.Random.Range(0, npcLocations[currentListIndex].Count);
                currentTarget = npcLocations[currentListIndex][randomLocationIndex];

                agent.SetDestination(currentTarget.position);

                npcLocations[currentListIndex].Remove(currentTarget);
                spawner.RemoveLocation(currentTarget, currentListIndex);

                StartCoroutine(CheckIfReachedDestination());
            }
            else
            {
                StartCoroutine(WaitAndRetry());
            }
        }
        else
        {
            currentTarget = spawner.exitLocation;
            agent.SetDestination(currentTarget.position);
        }
    }

    private IEnumerator WaitAndRetry()
    {
        float waitTime = UnityEngine.Random.Range(2f, 8f);
        yield return new WaitForSeconds(waitTime);
        MoveToPoint();
    }

    private IEnumerator CheckIfReachedDestination()
    {
        while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance)
        {
            yield return null;
        }

        visitedLists.Add(currentListIndex);
        StartCoroutine(WaitAtLocation());
    }

    private IEnumerator WaitAtLocation()
    {
        animator.SetBool("IsWalking", false);
        animator.SetFloat("PodHor", podiumDir.x);
        animator.SetFloat("PodVer", podiumDir.y);

        spawnTimer = UnityEngine.Random.Range(2, 8);
        yield return new WaitForSeconds(spawnTimer);

        if (UnityEngine.Random.value > 0.7f && UnityEngine.Random.value <= 1.0f)
        {
            StartCoroutine(Grabbing());
        }
        else
        {
            animator.SetBool("IsWalking", true);
            spawner.AddLocation(currentTarget, currentListIndex);
            MoveToPoint();
        }
    }

    private IEnumerator Grabbing()
    {
        grabbing = true;
        grabMarker.SetActive(true);
        agent.isStopped = true;
        yield return new WaitForSeconds(3f);

        //damage here

        grabbing = false;
        grabMarker.SetActive(false);
        agent.isStopped = false;
        animator.SetBool("IsWalking", true);

        spawner.AddLocation(currentTarget, currentListIndex);
        MoveToPoint();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Exit"))
        {
            Destroy(gameObject);
            spawner.npcCount--;
        }

        if (other.CompareTag("Up"))
        {
            podiumDir.y = 1;
            podiumDir.x = 0;
        }

        if (other.CompareTag("Down"))
        {
            podiumDir.y = -1;
            podiumDir.x = 0;
        }

        if (other.CompareTag("Left"))
        {
            podiumDir.y = 0;
            podiumDir.x = -1;
        }

        if (other.CompareTag("Right"))
        {
            podiumDir.y = 0;
            podiumDir.x = 1;
        }

        if (other.CompareTag("Weapon"))
        {
            spawner.AddLocation(currentTarget, currentListIndex);

            Destroy(gameObject);
            spawner.npcCount--;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Up"))
        {
            podiumDir.y = 0;
            podiumDir.x = 0;
        }

        if (other.CompareTag("Down"))
        {
            podiumDir.y = 0;
            podiumDir.x = 0;
        }

        if (other.CompareTag("Left"))
        {
            podiumDir.y = 0;
            podiumDir.x = 0;
        }

        if (other.CompareTag("Right"))
        {
            podiumDir.y = 0;
            podiumDir.x = 0;
        }
    }

    void UpdateAnimator()
    {
        Vector3 velocity = agent.velocity;
        direction = new Vector2(velocity.x, velocity.y).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        if (angle > -45 && angle <= 45)
        {
            direction = Vector2.right;
            spriteRenderer.flipX = false;
        }
        else if (angle > 45 && angle <= 135)
        {
            direction = Vector2.up;
            spriteRenderer.flipX = false;
        }
        else if (angle > 135 || angle <= -135)
        {
            direction = Vector2.left;
            spriteRenderer.flipX = true;
        }
        else
        {
            direction = Vector2.down;
            spriteRenderer.flipX = false;
        }

        animator.SetFloat("Horizontal", direction.x);
        animator.SetFloat("Vertical", direction.y);
    }

    void SortLayers()
    {
        float minY = -16f;
        float maxY = 10f;

        int sortingOrder = (int)Mathf.Lerp(20, 3, Mathf.InverseLerp(minY, maxY, transform.position.y));
        spriteRenderer.sortingOrder = sortingOrder;

        // Update sorting order for extra game objects
        foreach (GameObject obj in extraGameObjects)
        {
            SpriteRenderer objSpriteRenderer = obj.GetComponent<SpriteRenderer>();
            if (objSpriteRenderer != null)
            {
                objSpriteRenderer.sortingOrder = sortingOrder;
            }
        }
    }
    
    private void OnDestroy()
    {
        OnDestroyEvent?.Invoke();
    }
}
