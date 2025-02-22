using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class Case : MonoBehaviour
{
    public Case leftCase;
    public Case rightCase;
    public Case bottomCase;
    public Case topCase;

    [Header("Weapon")]
    [SerializeField] public GameObject weaponInCase;

    [Header("Settings")]
    public float setCooltime;
    [SerializeField] private float cooldownTime;
    public bool canAttack = true;

    [SerializeField] private GameObject coolingClock;
    [SerializeField] private Image coolingCircle;

    public CircleCollider2D circleCollider2D;
    public Light2D spotLight;

    public int health = 100;
    public Image healthBar;
    public static event Action<Transform> OnCaseInactive;

    private void Start()
    {
        circleCollider2D = GetComponent<CircleCollider2D>();
       
    }

    void Awake()
    {
        cooldownTime = setCooltime;
    }

    void Update()
    {
        if (cooldownTime <= 0)
        {
            coolingClock.SetActive(false);
            canAttack = true;
            cooldownTime = setCooltime;
            coolingCircle.fillAmount = cooldownTime / setCooltime;
        }

        if (cooldownTime != 0 && !canAttack)
        {
            cooldownTime -= Time.deltaTime;
            coolingCircle.fillAmount = cooldownTime / setCooltime;
        }
    }

    public void Attack()
    {
        weaponInCase.GetComponent<Animator>().SetTrigger(Animator.StringToHash("Attack"));
        canAttack = false;
        coolingClock.SetActive(true);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (healthBar != null)
        {
            healthBar.fillAmount = health / 100f;
        }

        if (health <= 0)
        {
            OnCaseInactive?.Invoke(transform);
            Spawner spawner = GameObject.FindWithTag("GameController")?.GetComponent<Spawner>();
            if (spawner != null)
            {
                spawner.RemoveLocation(transform);
            }
            
            spawner.towers--;
            if (spawner.towers == 0)
            {
                spawner.GameOver();
            }
            gameObject.SetActive(false);

          
        }

        if (gameObject.activeInHierarchy && healthBar != null && healthBar.enabled)
        {
            StartCoroutine(FlashHealthBar());
        }
    }
    private IEnumerator FlashHealthBar()
    {
        for (int i = 0; i < 3; i++)
        {
            if (healthBar != null)
            {
                healthBar.enabled = false;
                yield return new WaitForSeconds(0.2f);
                healthBar.enabled = true;
                yield return new WaitForSeconds(0.2f);
            }
        }
    }
}
