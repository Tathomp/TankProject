using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    // Serverside script names referenced by WWWForms
    private readonly string URLUPDATEPASS = "action_updatepassword.php";

    // State references, initialized in Start()
    PlayerState ps;
    Leaderboard lb;
    LevelSelect ls;

    /***************************************
                   Game Objects
    **************************************/
    // Panels
    public GameObject changePassUI, difficultyUI, mainUI, pauseUI, profileUI, upgradeUI;
    public GameObject levelHUD, levelSelectUI, resetPasswordUI, resetConfUI, victoryDefeatUI;
    // Buttons
    public GameObject btnLvlSelect, btnLdrboard, btnProfile, btnUpgrades, btnLogOut;
    // Input Text
    public InputField txtConfirmResetPass, txtCurrentPass, txtResetPass;
    // Text
    public Text txtResetFeedback, msgSubtitle;

    // Use this for initialization
    void Start()
    {
        // Initialize player state reference
        ps = PlayerState.GetCurrentPlayerState();
        lb = Leaderboard.GetLeaderboardState();
        ls = LevelSelect.GetLevelSelectState();
    }

        // Use this for initialization
        void Awake()
    {
        GameState.GameIsPaused = true;
    }

    // MenuManager accessor
    public static MenuManager GetMenuManagerState()
    {
        return GameObject.Find("CanvasMenus").GetComponent<MenuManager>();
    }

    /***************************************
                Display Functions
    **************************************/
    // Disable all UI menus 
    public void CloseAllMenus()
    {
        GameObject[] UIarray = { changePassUI, difficultyUI, mainUI, pauseUI, profileUI, upgradeUI, levelSelectUI, victoryDefeatUI };
        foreach (GameObject ui in UIarray)
            ui.SetActive(false);
    }

    // Disable the reset password panel
    public void CloseResetPanel()
    {
        resetPasswordUI.SetActive(false);
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

    // Display reset password pannel
    public void DisplayResetConfPanel()
    {
        resetConfUI.SetActive(true);
    }

    // Display reset password pannel
    public void DisplayResetPasswordPanel()
    {
        resetPasswordUI.SetActive(true);
    }

    // Display the upgrades screen
    public void DisplayUpgradesPanel()
    {
        upgradeUI.GetComponent<SelectUpgradeManager>().StartUI();
    }


    /***************************************
                Button Actions
     **************************************/
    public void CloseUpgradeList()
    {
        PlayerState.GetCurrentPlayerState().SaveState();
        upgradeUI.SetActive(false);
    }

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
        // Set play difficulty based on attribute passed from button
        GameState.SetDifficulty((Difficulty)difficultySelected);

        // Hide all menus except HUD, start gameplay
        CloseAllMenus();

        // Start Level
        GameState.UnPauseLevel();
        levelHUD.SetActive(true); // this should be part of a function call to start the gameplay
    }

    // Enable the leaderboard panel
    public void LeaderboardButtonTapped()
    {
        lb.DisplayLeaderboardPanel();
    }

    // Redirect to level select panel
    public void LevelSelectButtonTapped()
    {
        ls.DisplayLevelSelectPanel();
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
        GameState.PauseLevel();

        // Diplay the pause menu panel
        pauseUI.SetActive(true);
    }

    // Redirect to player profile panel
    public void ProfileButtonTapped()
    {
        GameObject.Find("Manager").GetComponent<Profile>().DisplayProfilePanel();
    }

    // Redirect to LogIn panel
    public void QuitButtonTapped()
    {
        // Hide the pause menu and HUD
        pauseUI.SetActive(false);
        levelHUD.SetActive(false);

        // Display main menu
        DisplayMainMenuPanel();
    }

    // Display the password reset panel
    public void ResetPasswordButtontapped()
    {
        DisplayResetPasswordPanel();
    }

    // Submit password reset request
    public void ResetPasswordSubmitButtontapped()
    {
        StartCoroutine("UpdatePassword");
    }

    // Close the Reset password panel
    public void ResetPasswordCancelButtontapped()
    {
        CloseResetPanel();
    }

    // Reset password button
    public void ResetOKButtontapped()
    {
        // Close confirmation panel
        resetConfUI.SetActive(false);
        // Save config and log out user
        DisplayLoginPanel();
    }

    // Redirect to LogIn panel
    public void RestartLevelButtonTapped()
    {
        // Hide the pause menu and HUD
        pauseUI.SetActive(false);

        // Restart gameplay
        GameState.RestartLevel();
    }

    // Redirect to LogIn panel
    public void ResumeButtonTapped()
    {
        // Hide the pause menu and HUD
        pauseUI.SetActive(false);

        // Unpause the game
        GameState.UnPauseLevel();

     }

    // Redirect to level select panel
    public void SelectNewLevelButtonTapped()
    {
        // Hide the pause menu and HUD
        pauseUI.SetActive(false);
        levelHUD.SetActive(false);

        // Display main menu & level select over top
        DisplayMainMenuPanel();
        ls.DisplayLevelSelectPanel();
    }

    // Redirect to upgrades panel
    public void UpgradesButtonTapped()
    {
        DisplayUpgradesPanel();
    }

    /***************************************
                Coroutines
     **************************************/
    private IEnumerator UpdatePassword()
    {
        // Verify password length and matching
        if (txtCurrentPass.text.Length < 8 || txtResetPass.text.Length < 8)
        {
            txtResetFeedback.text = "Password neeeds to be at least 8 characters long";
            yield break;
        }
        if (txtResetPass.text != txtConfirmResetPass.text)
        {
            txtResetFeedback.text = "New passwords do not match";
            yield break;
        }

        // Build the form for submission
        WWWForm form = new WWWForm();
        form.AddField("id", ps.GetID());
        form.AddField("currentPass", txtCurrentPass.text);
        form.AddField("newPass", txtResetPass.text);

        WWW reset = new WWW(ps.URL(URLUPDATEPASS), form);
        yield return reset;

        // Check for successful web request
        if (string.IsNullOrEmpty(reset.error))
        {
            // Convert response to JSON
            User user = JsonUtility.FromJson<User>(reset.text);

            // Print the response message (error or success)
            txtResetFeedback.text = user.msg;

            // Only proceed if credentials are valid and account is active
            if (user.query == true && user.success == true)
            {
                // Clear registration input values
                txtCurrentPass.text = "";
                txtResetPass.text = "";
                txtConfirmResetPass.text = "";
                // Close reset panel
                CloseResetPanel();
                // Display reset confirmation panel
                DisplayResetConfPanel();
            }
        }
        else
        {
            // Connection failed
            txtResetFeedback.text = "An error occured talking to the server";
        }
    }

}
