using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Case : MonoBehaviour
{

    public Case leftCase;
    public Case rightCase;
    public Case bottomCase;
    public Case topCase;

    [Header("Weapon")]
    [SerializeField] private GameObject weaponInCase;

    [Header("Settings")]
    public float setCooltime;
    [SerializeField] private float cooldownTime;
    public bool canAttack = true;

    [SerializeField] private GameObject coolingClock;
    [SerializeField] private Image coolingCircle;

    // Start is called before the first frame update
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
