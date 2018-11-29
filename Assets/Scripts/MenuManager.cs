﻿using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    /***************************************
                   Game Objects
    **************************************/
    // Panels
    public GameObject changePassUI, difficultyUI, mainUI, pauseUI, profileUI, upgradeUI;
    public GameObject levelHUD, levelSelectUI;
    // Buttons
    public GameObject btnLvlSelect, btnLdrboard, btnProfile, btnUpgrades, btnLogOut;
    // Text
    public Text msgSubtitle;

    // Use this for initialization
    void Awake()
    {
        GameState.GameIsPaused = true;
    }

    // Leaderboard accessor
    public Leaderboard LB()
    {
        return GameObject.Find("CanvasMenus").GetComponent<Leaderboard>();
    }

    // LevelSelect accessor
    public LevelSelect LS()
    {
        return GameObject.Find("CanvasMenus").GetComponent<LevelSelect>();
    }

    /***************************************
                Display Functions
    **************************************/
    // Disable all UI menus 
    public void CloseAllMenus()
    {
        GameObject[] UIarray = { changePassUI, difficultyUI, mainUI, pauseUI, profileUI, upgradeUI, levelSelectUI };
        foreach (GameObject ui in UIarray)
            ui.SetActive(false);
    }

    // Display the Login Screen
    // Saves and Ends user session
    public void DisplayLoginPanel()
    {
        PlayerState ps = PlayerState.GetCurrentPlayerState();
        // Save the current player state to the database
        ps.SaveState();
        // Clear the current player state
        ps.StartState();
        // Disable possible origin panels 
        profileUI.SetActive(false);
        pauseUI.SetActive(false);
        mainUI.SetActive(false);
        // Switch back to the Authentication Manager
        ps.DisplayLoginCanvas();
    }

    // Display the Main Menu
    public void DisplayMainMenuPanel()
    {
        // Enable main menu panel
        mainUI.SetActive(true);
        // Set the subtitle message with player's name
        PlayerState ps = PlayerState.GetCurrentPlayerState();
        msgSubtitle.text = "Greetings, " + ps.GetUserName() + "!";
    }

    // Display the player's profile
    public void DisplayProfilePanel()
    {
        /// Todo - Link to profile
    }

    // Display the upgrades screen
    public void DisplayUpgradesPanel()
    {
        /// Todo - Link to upgrades
        DisableMenu();
        upgradeUI.SetActive(true);
    }


    /***************************************
                Button Actions
     **************************************/
    void DisableMenu()
    {
        mainUI.SetActive(false);
    }

    public void CloseUpgradeList()
    {
        PlayerState.GetCurrentPlayerState().SaveState();
        DisplayMainMenuPanel();
        upgradeUI.SetActive(false);
    }

    /***************************************
                Button Actions
     **************************************/
    // Display difficulty menu as an overlay
    public void DifficultyBackButtonTapped()
    {
        difficultyUI.SetActive(false);
    }

    // Display difficulty menu as an overlay
    public void DifficultyButtonTapped()
    {
        difficultyUI.SetActive(true);
    }

    // Set level selected and display difficulty menu
    public void DifficultyLevelTapped(int difficultySelected)
    {
        /// Set play difficulty based on attribute passed from button
        GameState.SetDifficulty((Difficulty)difficultySelected);

        // Hide all menus except HUD, start gameplay
        CloseAllMenus();

        // Start Level
        GameState.StartLevel();
        levelHUD.SetActive(true); // this should be part of a function call to start the gameplay
    }

    // Enable the leaderboard panel
    public void LeaderboardButtonTapped()
    {
        LB().DisplayLeaderboardPanel();
    }

    // Redirect to level select panel
    public void LevelSelectButtonTapped()
    {
        LS().DisplayLevelSelectPanel();
    }

    // Redirect to LogIn panel
    public void LogoutButtonTapped()
    {
        // Hide the pause menu and HUD
        pauseUI.SetActive(false);
        DisplayLoginPanel();
    }

    // Pause Game & redirect to pause menu panel
    public void PauseButtonTapped()
    {
        // Pause gameplay
        Awake();
        // Diplay the pause menu panel
        pauseUI.SetActive(true);
    }

    // Redirect to player profile panel
    public void ProfileButtonTapped()
    {
        DisplayProfilePanel();
    }

    // Redirect to LogIn panel
    public void QuitButtonTapped()
    {
        // Hide the pause menu and HUD
        pauseUI.SetActive(false);
        levelHUD.SetActive(false);

        // todo - reset gameplay??

        // Display main menu
        DisplayMainMenuPanel();
    }

    // Redirect to LogIn panel
    public void RestartLevelButtonTapped()
    {
        // Hide the pause menu and HUD
        pauseUI.SetActive(false);
        
        // todo - reset gameplay??
    }

    // Redirect to LogIn panel
    public void ResumeButtonTapped()
    {
        // Hide the pause menu and HUD
        pauseUI.SetActive(false);

        // Unpause the game
        // todo - unpause game

    }

    // Redirect to level select panel
    public void SelectNewLevelButtonTapped()
    {
        // Hide the pause menu and HUD
        pauseUI.SetActive(false);
        levelHUD.SetActive(false);

        // todo - reset gameplay??

        // Display level select
        LevelSelectButtonTapped();
    }

    // Redirect to upgrades panel
    public void UpgradesButtonTapped()
    {
        DisplayUpgradesPanel();
    }

    

}
