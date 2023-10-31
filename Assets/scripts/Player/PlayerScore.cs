using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerScore : MonoBehaviour
{
    [SerializeField] public static int foodAte;
    [SerializeField] public static int friendlyCreaturesKilled;
    [SerializeField] public static int hostileCreaturesKilled;
    [SerializeField] public static int timeSurvived;

    void Start()
    {
        foodAte = 0;
        friendlyCreaturesKilled = 0;
        hostileCreaturesKilled = 0;
        timeSurvived = 0;
    }

    private void OnEnable() 
    {
        Food.AteFood += UpdateScore;
    }

    private void OnDisable() 
    {
        Food.AteFood -= UpdateScore;
    }

    private void UpdateScore(int val)
    {
        switch(val)
        {
            case 1:
                foodAte++;
                break;
            case 2:
                friendlyCreaturesKilled++;
                break;
            case 3:
                hostileCreaturesKilled++;
                break;
            case 4:
                timeSurvived++; 
                break;
        }
    }
}
