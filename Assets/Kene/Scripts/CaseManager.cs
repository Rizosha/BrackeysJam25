using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CaseManager : MonoBehaviour
{
    public static CaseManager Instance;

    [SerializeField] private Case _activeCase;

    public Case ActiveCase
    {
        get { return _activeCase; }
        set { _activeCase = value; }
    }

    public List<Case> cases;
    public GameObject tranistionEffect;
    public GameObject bloodEffect;

    [NonSerialized] public bool hasAttacked;
 
    private void Awake()
    {
        _activeCase = cases[0];
    }

    private void Start()
    {
        Instance = this;
        StartCoroutine(InitializeCase(_activeCase));
    }

    private void Update()
    {
        InputCaseSwitch(ActiveCase);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!hasAttacked)
            {
                print(_activeCase);
                StartCoroutine(_activeCase.Attack());
                hasAttacked = true;
            }
        }
    }
    public IEnumerator InitializeCase(Case currentCase)
    {
        if (currentCase == null || !currentCase.canAttack)
            yield return null;

        float time = 0;
        float duration = 0.1f;
        tranistionEffect.SetActive(true);
        while (time < duration)
        {
            tranistionEffect.transform.position = Vector3.Lerp(tranistionEffect.transform.position,
                currentCase.transform.position, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        tranistionEffect.SetActive(false);

        for (int i = 0; i < cases.Count; i++)
        {
            if (currentCase == cases[i])
                continue;

            DeinitializeCase(cases[i]);
        }

        currentCase.spotLight.enabled = true;
        ActiveCase = currentCase;
    }

    public void CreateBloodSpatter(Transform transform)
    {
        GameObject bloodVFX = Instantiate(bloodEffect);
        bloodVFX.transform.position = transform.position;
    }
    private void DeinitializeCase(Case currentCase) 
    {
        currentCase.spotLight.enabled = false;
    }

    private void InputCaseSwitch(Case caseSwitch)
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (caseSwitch.topCase == null)
                return;

            if (!caseSwitch.topCase.canAttack || !caseSwitch.topCase.gameObject.activeSelf)
                return;

            StartCoroutine(InitializeCase(caseSwitch.topCase));
            return;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            if (caseSwitch.leftCase == null)
                return;

            if (!caseSwitch.leftCase.canAttack || !caseSwitch.leftCase.gameObject.activeSelf)
                return;

            StartCoroutine(InitializeCase(caseSwitch.leftCase));
            return;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            if (caseSwitch.bottomCase == null)
                return;

            if (!caseSwitch.bottomCase.canAttack || !caseSwitch.bottomCase.gameObject.activeSelf)
                return;

            StartCoroutine(InitializeCase(caseSwitch.bottomCase));
            return;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            if (caseSwitch.rightCase == null)
                return;

            if (!caseSwitch.rightCase.canAttack || !caseSwitch.rightCase.gameObject.activeSelf)
                return;

            StartCoroutine(InitializeCase(caseSwitch.rightCase));
            return;
        }
    }
    
}
