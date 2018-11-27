using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameState
{
    
	public static bool GameIsPaused { get; set; }

    // The LevelStarted Evenet will fire whenever the level starts
    // The tanks will subscribe to these events to initialize their upgrades
    public delegate void OnLevelStart();
    public static event OnLevelStart LevelStarted;

    public static Difficulty CurrentDifficulty = Difficulty.Easy;

    //Call when we need to fire the LevelStartedEvents
    public static void StartLevel()
    {
        Debug.Log(LevelStarted.Method);

        LevelStarted();
    }
}
