﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EndGameManager {

    public static void HasPlayerWon()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        if(enemies.Length == 1)
        {
            Debug.Log("Victory Condition Reached");

            GameState.EndLevel(true);
        }
    }

    
}