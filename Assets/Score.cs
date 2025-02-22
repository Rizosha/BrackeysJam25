using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    public int score = 0;

    [SerializeField] private TextMeshProUGUI tmp;


    private void Update()
    {
        tmp.text = "Score: " + score;
    }

    public void AddScore(int amount)
    {
        score += amount;
    }

}
