using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaseAnimator : MonoBehaviour
{
    public GameObject[] cases;
    public GameObject currentCase;
    public CaseManager caseScript;
    public CircleCollider2D hitbox;

    // Start is called before the first frame update
    void Start()
    {
        caseScript = gameObject.GetComponent<CaseManager>();
    }

    // Update is called once per frame
    void Update()
    {
        currentCase = caseScript.ActiveCase.gameObject;
        hitbox = currentCase.GetComponent<CircleCollider2D>();
    }

}
