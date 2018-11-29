﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class PlayerState : MonoBehaviour
{
    /******************************************************************************************
     Central Reference Manager for components and attributes used in gameplay and user settings 
     *****************************************************************************************/

    // Host server public IP or url, change here to update all instances
    private readonly string SERVERADDRESS = "http://www.ninjalive.com/tanks/";
    private readonly string URLUPDATESTATE = "action_updatestate.php";
    private readonly string URLUPDATESCORE = "action_updatescore.php";
    private readonly string URLGETSTATE = "action_getstate.php";
    

    // Central repository of active gameplay and user data
    // Any persistant data to be saved to the db needs to exist here
    private static int userID;              // db primary key
    private static int maxLevel;            // highest level available: completed level + 1, default=1
    private static string userName;
    private static string userEmail;
    private static string userImage;        // filename of user image
    private static int purchasedUpgrades;   // 3 digit addative code: 111 = no selections (can't save 000)
    private static int activeUpgrades;      // 3 digit code: 111 = no selections (can't save 000)
    private static int userCredits;         // total user credits

    [SerializeField] private Upgrade GunUpgrade;
    [SerializeField] private Upgrade ArmorUpgrade;
    [SerializeField] private Upgrade TrackUpgrade;

    // Suggestions... remove if not needed
    private static int maxHP;           // This variable and the following are only examples,
    private static int maxArmor;        // team members woring on various game mechanics
    private static int hp;              // will need to add variables which make sense to their
    private static int armor;           // development logic.
    private static int speed;
    private static int damage;



    /***************************************
                General Functions
     **************************************/
    // Builds server url strings
    public string URL(string url)
    {
        return SERVERADDRESS + url;
    }

    // Deactivate the login panel and activate the main menu
    public void DisplayMenuCanvas()
    {
        /// todo - deactivate canvases too
        // Switching between manager scripts
        GameObject.Find("CanvasAuthentication").GetComponent<AuthenticationManager>().logInUI.SetActive(false);
        GameObject.Find("CanvasMenus").GetComponent<MenuManager>().DisplayMainMenuPanel();
    }

    // Deactivate the main menu and activate the login panel
    // Logs Out current player, resets state 
    public void DisplayLoginCanvas()
    {
        /// todo - deactivate canvases too
        // Switching between manager scripts
        GameObject.Find("CanvasMenus").GetComponent<MenuManager>().mainUI.SetActive(false);
        GameObject.Find("CanvasAuthentication").GetComponent<AuthenticationManager>().DisplayLoginPanel();
    }

    /***************************************
                State Modifiers
     **************************************/
    // Creates a new game state
    public void StartState()
    {
        // Set default properties
        userID = 0;
        maxLevel = 1;
        userName = "Player";
        userEmail = "";
        userImage = "default.png";
        purchasedUpgrades = 111;
        activeUpgrades = 111;
        userCredits = 0;
        GunUpgrade = null;
        ArmorUpgrade = null;
        TrackUpgrade = null;

        // Set the profile picture
        GameObject.Find("Manager").GetComponent<Profile>().SetAvatar();

        maxHP = 100;
        maxArmor = 100;
        hp = maxHP;
        armor = maxArmor;
        speed = 10;
        damage = 10;
    }

    // Restore player state upon login
    public void RestoreState(User u)
    {
        //Set all the player attributes
        userName = u.userName;
        userEmail = u.userEmail;
        userID = u.userID;
        userImage = u.userImage;
        maxLevel = u.maxLevel;
        purchasedUpgrades = u.purchasedUpgrades;
        activeUpgrades = u.activeUpgrades;
        userCredits = u.userCredits;

        // Set the profile picture
        GameObject.Find("Manager").GetComponent<Profile>().SetAvatar();
    }

    // Save persistant game state data to DB 
    // Should be called at key points (victory, defeat, upgrade selection, purchase, etc)
    public void SaveState()
    {
        // Build the form for submission
        WWWForm form = new WWWForm();

        form.AddField("id", userID);        // For authentication
        form.AddField("email", userEmail);      // For authentication
        form.AddField("level", maxLevel);
        form.AddField("profile", userImage);
        form.AddField("purchased", purchasedUpgrades);
        form.AddField("active", activeUpgrades);
        form.AddField("credit", userCredits);

        WWW push = new WWW(URL(URLUPDATESTATE), form);
    }

    // Insert or Update users high score and credits earned
    // Should be called after every victory and defeat
    public void UpdateScore(int levelPlayed, int score, int creditsEarned, bool victory)
    {
        // Update values before push to db
        if (victory)
        {
            // Add credits earned
            userCredits += creditsEarned;
            // Unlock next level
            if (levelPlayed == maxLevel)
                maxLevel++;
        }

        // Build the form for submission
        WWWForm form = new WWWForm();

        form.AddField("id", userID);            // For authentication
        form.AddField("credit", userCredits);   // user table
        form.AddField("level", maxLevel);       // user table
        form.AddField("score", score);          // levelscores table
        form.AddField("played", levelPlayed);   // levelscores table

        WWW push = new WWW(URL(URLUPDATESCORE), form);
    }


    /***************************************
                Setters
     **************************************/
    // Setter for private gun upgrade object
    public void SetGunUpgrade(Upgrade u)
    {
        GunUpgrade = u;
    }

    // Setter for private armor upgrade object
    public void SetArmorUpgrade(Upgrade u)
    {
        ArmorUpgrade = u;
    }

    // Setter for private track upgrade object
    public void SetTrackUpgrade(Upgrade u)
    {
        TrackUpgrade = u;
    }


    /***************************************
                Getters
     **************************************/
    public string GetAvatar()
    {
        return URL("avatars/" + userImage);
    }

    public int GetCredits()
    {
        return userCredits;
    }

    public string GetEmail()
    {
        return userEmail;
    }

    public int GetID()
    {
        return userID;
    }

    public string GetUserName()
    {
        return userName;
    }

    public int GetMaxLevel()
    {
        return maxLevel;
    }

    public static PlayerState GetCurrentPlayerState()
    {
        return GameObject.Find("Manager").GetComponent<PlayerState>();
    }

    public Upgrade GetGunUpgrade()
    {
        return GunUpgrade;
    }

    public Upgrade GetTreadUpgrade()
    {
        return TrackUpgrade;
    }

    public Upgrade GetArmorUpgrade()
    {
        return ArmorUpgrade;
    }


    /***************************************
                Coroutines
     **************************************/
    // SaveState Coroutine, updates player state db records
    private IEnumerator PushSaveState()
    {
        // Build the form for submission
        WWWForm form = new WWWForm();

        form.AddField("id", userID);        // For authentication
        form.AddField("email", userEmail);      // For authentication
        form.AddField("level", maxLevel);
        form.AddField("profile", userImage);
        form.AddField("purchased", purchasedUpgrades);
        form.AddField("active", activeUpgrades);
        form.AddField("credit", userCredits);

        WWW push = new WWW(URL(URLUPDATESTATE), form);
        yield return push;

        // Check for successful connection
        if (string.IsNullOrEmpty(push.error))
        {
            // Convert response to JSON
            User response = JsonUtility.FromJson<User>(push.text);

            // Check for failed update
            if (response.query == false || response.success == false)
            {
                // Log the error
                Debug.Log(response.msg);
            }
        }
        else
        {
            // Log the connection error
            Debug.Log(push.error);
        }
    }

    // SaveState Coroutine, updates player state db records
    private IEnumerator PushVictoryDefeat(WWWForm form)
    {
        WWW push = new WWW(URL(URLUPDATESTATE), form);
        yield return push;

        // Check for successful connection
        if (string.IsNullOrEmpty(push.error))
        {
            // Convert response to JSON
            User response = JsonUtility.FromJson<User>(push.text);

            // Check for failed update
            if (response.query == false || response.success == false)
            {
                // Log the error
                Debug.Log(response.msg);
            }
        }
        else
        {
            // Log the connection error
            Debug.Log(push.error);
        }
    }
}
