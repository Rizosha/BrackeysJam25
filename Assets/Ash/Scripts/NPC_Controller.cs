using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC_Controller : MonoBehaviour
{
    NavMeshAgent agent;
    Spawner spawner;

    private List<Transform> npcLocations = new List<Transform>();
    private Transform currentTarget;
    private int spawnTimer;

    public bool grabbing = false;
    public GameObject grabMarker;

    Animator animator;
    public Vector2 podiumDir;
    private Vector2 direction;
    [SerializeField] private SpriteRenderer spriteRenderer;
    public GameObject[] extraGameObjects;

    public bool canGrab = true;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        spawner = GameObject.FindWithTag("GameController").GetComponent<Spawner>();
        npcLocations = new List<Transform>(spawner.GetMainLocations()); 

        grabMarker = gameObject.transform.Find("Grab").gameObject;

        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        MoveToPoint();
    }

    private void Update()
    {
        UpdateAnimator();
        SortLayers();

        if (currentTarget != null && !currentTarget.gameObject.activeInHierarchy)
        {
            HandleCaseInactive(currentTarget);
        }
    }

    private IEnumerator WaitAndRetry()
    {
        float waitTime = UnityEngine.Random.Range(2f, 8f);
        yield return new WaitForSeconds(waitTime);
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
            Destroy(gameObject);
            spawner.npcCount--;
            GameObject.FindWithTag("GameController").GetComponent<Score>().AddScore(100);
        }

        if (other.CompareTag("WaitPoint"))
        {
            canGrab = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("WaitPoint"))
        {
            canGrab = true;
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

        foreach (GameObject obj in extraGameObjects)
        {
            SpriteRenderer objSpriteRenderer = obj.GetComponent<SpriteRenderer>();
            if (objSpriteRenderer != null)
            {
                objSpriteRenderer.sortingOrder = sortingOrder;
            }
        }
    }

    private Case FindClosestCase()
    {
        Case[] cases = FindObjectsOfType<Case>();
        Case closestCase = null;
        float closestDistance = Mathf.Infinity;

        foreach (Case currentCase in cases)
        {
            float distance = Vector3.Distance(transform.position, currentCase.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestCase = currentCase;
            }
        }

        return closestCase;
    }

    private IEnumerator Grabbing()
    {
        grabbing = true;
        grabMarker.SetActive(true);
        agent.isStopped = true;
        yield return new WaitForSeconds(3f);

        Case closestCase = FindClosestCase();
        if (closestCase != null)
        {
            closestCase.TakeDamage(20);
        }

        grabbing = false;
        if (grabMarker != null)
        {
            grabMarker.SetActive(false);
        }

        agent.isStopped = false;
        animator.SetBool("IsWalking", true);

        MoveToPoint();
    }

    private Vector3 GetRandomNavMeshLocation(Vector3 center, float radius)
    {
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * radius;
        randomDirection += center;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, radius, 1);
        return hit.position;
    }

    private void MoveToPoint()
    {
        Transform closestLocation = null;
        float closestDistance = Mathf.Infinity;

        foreach (var location in npcLocations)
        {
            float distance = Vector3.Distance(transform.position, location.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestLocation = location;
            }
        }

        if (closestLocation != null)
        {
            currentTarget = closestLocation;
            npcLocations.Remove(closestLocation);
            Vector3 randomNavMeshLocation = GetRandomNavMeshLocation(currentTarget.position, 6f);
            agent.SetDestination(randomNavMeshLocation);

            StartCoroutine(CheckIfReachedRandomLocation(randomNavMeshLocation));
        }
        else
        {
            if (spawner.exitLocation != null && spawner.exitLocation.Length > 0)
            {
                int randomExitIndex = UnityEngine.Random.Range(0, spawner.exitLocation.Length);
                currentTarget = spawner.exitLocation[randomExitIndex];
                agent.SetDestination(currentTarget.position);
            }
        }
    }

    private IEnumerator WaitAtLocation()
    {
        animator.SetBool("IsWalking", false);
        animator.SetFloat("PodHor", podiumDir.x);
        animator.SetFloat("PodVer", podiumDir.y);

        spawnTimer = UnityEngine.Random.Range(2, 8);
        yield return new WaitForSeconds(spawnTimer);

        if (UnityEngine.Random.value > 0f && UnityEngine.Random.value <= 1.0f && canGrab)
        {
            StartCoroutine(Grabbing());
        }
        else
        {
            animator.SetBool("IsWalking", true);
            MoveToPoint();
        }
    }

    private IEnumerator CheckIfReachedRandomLocation(Vector3 randomLocation)
    {
        while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance)
        {
            yield return null;
        }

        StartCoroutine(WaitAtLocation());
    }

    private void OnEnable()
    {
        Case.OnCaseInactive += HandleCaseInactive;
    }

    private void OnDisable()
    {
        Case.OnCaseInactive -= HandleCaseInactive;
    }

    private void HandleCaseInactive(Transform inactiveCase)
    {
        if (npcLocations.Contains(inactiveCase))
        {
            npcLocations.Remove(inactiveCase);
        }

        if (currentTarget == inactiveCase)
        {
            if (grabbing)
            {
                StopCoroutine("Grabbing");
                grabbing = false;
                grabMarker.SetActive(false);
                agent.isStopped = false;
            }
            MoveToPoint();
        }
    }
}