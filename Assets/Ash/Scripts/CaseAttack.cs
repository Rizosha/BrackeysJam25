using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaseAttack : MonoBehaviour
{
    // Start is called before the first frame update
    public void TurnOnHitbox()
    {
        GetComponent<CircleCollider2D>().enabled = true;
    }
    
    public void TurnOffHitbox()
    {
        GetComponent<CircleCollider2D>().enabled = false;
    }
}

