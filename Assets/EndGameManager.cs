using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EndGameManager {

    public static GameObject[] enemies;

    public static void HasPlayerWon()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");

        for (int i = 0; i < enemies.Length; i++)
        {
            if(enemies[i].activeInHierarchy)
            {
                return;
            }
        }

        Debug.Log("Victory Condition Reached");

        GameState.EndLevel(true);
        
    }    
}
