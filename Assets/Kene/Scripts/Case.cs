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

    // Start is called before the first frame update
    private void Start()
    {
        circleCollider2D = GetComponent<CircleCollider2D>();
    }

    void Awake()
    {
        cooldownTime = setCooltime;
    }

    // Update is called once per frame
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
}
