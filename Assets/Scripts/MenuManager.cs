using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    PlayerState ps;

    /***************************************
                Game Objects
     **************************************/
    // Panels
    public GameObject changePassUI, difficultyUI, mainUI, pauseUI, profileUI, levelHUD, upgradeUI;
    // Buttons
    public GameObject btnLvlSelect, btnLdrboard, btnProfile, btnUpgrades, btnLogOut;
    // Text
    public Text msgSubtitle;

    // Use this for initialization
    void Awake()
    {
        GameState.GameIsPaused = true;
    }

    /***************************************
               Display Functions
    **************************************/    
    // Display the level select panel
    public void DisplayLevelSelectPanel()
    {
        /// Todo - Link to level selector
        /// Currently, just disables the menu and hud and displays the level hud
        DisableMenu();
        levelHUD.SetActive(true);
        GameState.GameIsPaused = false;
        GameState.StartLevel();
    }

    // Display the Login Screen
    // Saves and Ends user session
    public void DisplayLoginPanel()
    {
        PlayerState ps = GameObject.Find("Manager").GetComponent<PlayerState>();
        // Save the current player state to the database
        ps.SaveState();
        // Clear the current player state
        ps.StartState();
        // Switch back to the Authentication Manager
        ps.DisplayLoginCanvas();
    }

    // Display the Main Menu
    public void DisplayMainMenuPanel()
    {
        // Enable main menu panel
        mainUI.SetActive(true);
        // Set the subtitle message with player's name
        PlayerState ps = GameObject.Find("Manager").GetComponent<PlayerState>();
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
    // Enable the leaderboard panel
    public void LeaderboardButtonTapped()
    {
        GameObject.Find("CanvasMenus").GetComponent<Leaderboard>().DisplayLeaderboardPanel();
    }

    // Redirect to level select panel
    public void LevelSelectButtonTapped()
    {
        DisplayLevelSelectPanel();
    }

    // Redirect to player profile panel
    public void ProfileButtonTapped()
    {
        DisplayProfilePanel();
    }

    // Redirect to upgrades panel
    public void UpgradesButtonTapped()
    {
        DisplayUpgradesPanel();
    }

    // Redirect to LogIn panel
    public void LogoutButtonTapped()
    {
        DisplayLoginPanel();
    }

}
