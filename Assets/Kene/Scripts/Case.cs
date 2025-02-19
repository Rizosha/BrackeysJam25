using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Case : MonoBehaviour
{

    public Case leftCase;
    public Case rightCase;
    public Case bottomCase;
    public Case topCase;

    [Header("Settings")]
    public float setCooltime;
    [SerializeField] private float cooldownTime;

    public bool canAttack = true;

    // Start is called before the first frame update
    void Start()
    {
        cooldownTime = setCooltime;
    }

    // Update is called once per frame
    void Update()
    {
        if (cooldownTime <= 0)
        {
            canAttack = true;
            cooldownTime = setCooltime;
        }

        if (cooldownTime != 0 && !canAttack) 
        { 
            cooldownTime -= Time.deltaTime;
        }
    }

    public virtual void Attack()
    {
        canAttack = false;
    }
}
