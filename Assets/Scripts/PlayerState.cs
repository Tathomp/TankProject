using System.Collections;
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
    private readonly string URLGETSTATE = "action_getstate.php";

    // Central repository of active gameplay and user data
    // Any persistant data to be saved to the db needs to exist here
    private static int userID;
    private static int maxLevel;
    private static string userName;
    private static string userEmail;
    private static string userImage;

    // Suggestions...
    private static int maxHP;           // This variable and the following are only examples,
    private static int maxArmor;        // team members woring on various game mechanics
    private static int hp;              // will need to add variables which make sense to their
    private static int armor;           // development logic.
    private static int speed;
    private static int damage;
    private static string purchasedUpgrades;
    private static string activeUpgrades;


    [SerializeField] private Upgrade GunUpgrade;
    [SerializeField] private Upgrade ArmorUpgrade;
    [SerializeField] private Upgrade TrackUpgrade;


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
        maxHP = 100;
        maxArmor = 100;
        hp = maxHP;
        armor = maxArmor;
        speed = 10;
        damage = 10;
        purchasedUpgrades = ""; 
        activeUpgrades = "";
        GunUpgrade = null;
        ArmorUpgrade = null;
        TrackUpgrade = null;
    }

    // Restore player state upon login
    public void RestoreState(User u)
    {
        userName = u.userName;
        userEmail = u.userEmail;
        userID = u.userID;
        userImage = u.userImage;
        maxLevel = u.maxLevel;
        purchasedUpgrades = u.purchasedUpgrades;
        activeUpgrades = u.activeUpgrades;
        // SetGunUpgrade(gun);
        // SetArmorUpgrade(armor);
        // SetTrackUpgrade(track);
    }

    // Save persistant game state data to DB 
    // Should be called at key points (victory, defeat, upgrade selection, purchase, etc)
    public void SaveState()
    {
        StartCoroutine("PushSaveState");
    }

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
        //form.AddField("GunUpgrade", GunUpgrade);
        //form.AddField("ArmorUpgrade", ArmorUpgrade);
        //form.AddField("TrackUpgrade", TrackUpgrade);

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
    public string GetUserName()
    {
        return userName;
    }

    public static PlayerState GetCurrentPlayerState()
    {
        return GameObject.Find("Manager").GetComponent<PlayerState>();
    }
}
