using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// WIP until art assets are in!
/// </summary>
public class CaseManager : MonoBehaviour
{
    [SerializeField] private Case _activeCase;
    public Case ActiveCase { get { return _activeCase; } set { _activeCase = value; } }

    public Case[] cases;
    public Color activeCaseColor; //here until possible
    public GameObject tranistionEffect;
  
   

    private void Awake()
    {
        _activeCase = cases[0];
    }

    private void Start()
    {
        StartCoroutine(InitializeCase(_activeCase));
    }

    private void Update()
    {
        InputCaseSwitch(ActiveCase);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            print(_activeCase);
            _activeCase.Attack();

            List<Case> openCases = cases.Where(x => x.canAttack).ToList();

            StartCoroutine(InitializeCase(openCases[Random.Range(0, openCases.Count)]));
        }

    }

    private IEnumerator InitializeCase(Case currentCase)
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

        for (int i = 0; i < cases.Length; i++)
        {
            if (currentCase == cases[i])
                continue;

            DeinitializeCase(cases[i]);
        }

        currentCase.GetComponent<SpriteRenderer>().color = activeCaseColor;
        ActiveCase = currentCase;
    }

    private void DeinitializeCase(Case currentCase) 
    {
        currentCase.GetComponent<SpriteRenderer>().color = Color.white;
    }
    private void InputCaseSwitch(Case caseSwitch)
    {
        if (Input.GetKeyDown(KeyCode.W)) 
        {
            if (caseSwitch.topCase == null)
                return;

            if (!caseSwitch.topCase.canAttack)
                return;

            StartCoroutine(InitializeCase(caseSwitch.topCase));
            return;
        } 
        
        if (Input.GetKeyDown(KeyCode.A)) 
        {
            if (caseSwitch.leftCase == null)
                return;

            if (!caseSwitch.leftCase.canAttack)
                return;

            StartCoroutine(InitializeCase(caseSwitch.leftCase));
            return;
        }
        
        if (Input.GetKeyDown(KeyCode.S)) 
        {
            if (caseSwitch.bottomCase == null)
                return;

            if (!caseSwitch.bottomCase.canAttack)
                return;

            StartCoroutine(InitializeCase(caseSwitch.bottomCase));
            return;
        }
        
        if (Input.GetKeyDown(KeyCode.D)) 
        {
            if (caseSwitch.rightCase == null)
                return;

            if (!caseSwitch.rightCase.canAttack)
                return;

            StartCoroutine(InitializeCase(caseSwitch.rightCase));
            return;
        }

    }
}
