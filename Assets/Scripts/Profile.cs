using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Profile : MonoBehaviour {

    private readonly string URLHIGHSCORE = "action_gethighscore.php";

    /***************************************
                   Game Objects
    **************************************/
    // Panels
    public GameObject profileUI;
    // Text
    public Text txtChangePassword, txtCredits, txtEmail, txtHighScore, txtUpdatePicture, txtUserName;
    // Image
    public Image profileImage, profileImage2;

    // PlayerState accessor
    public PlayerState PS()
    {
        return GameObject.Find("Manager").GetComponent<PlayerState>();
    }

    // MenuManager accessor
    public MenuManager MM()
    {
        return GameObject.Find("CanvasMenus").GetComponent<MenuManager>();
    }

    // Display the player's profile
    public void DisplayProfilePanel()
    {
        // Activate profile panel
        profileUI.SetActive(true);
        
        // Set the profile text to reflect state attributes
        txtCredits.text = PS().GetCredits().ToString();
        txtEmail.text = PS().GetEmail();
        txtUserName.text = PS().GetUserName();
        StartCoroutine("HighScore");
    }


    // Disable profile menu
    public void MainMenuButtonTapped()
    {
        profileUI.SetActive(false);
    }

    // Set the profile picture sprites
    public void SetAvatar()
    {
        StartCoroutine("Avatar");
    }


    /***************************************
                Coroutines
     **************************************/
    IEnumerator Avatar()
    {
        WWW www = new WWW(PS().GetAvatar());
        yield return www;
        profileImage.sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));
        profileImage2.sprite = profileImage.sprite;
    }

    private IEnumerator HighScore()
    {
        // Build the form for submission
        WWWForm form = new WWWForm();
        form.AddField("id", PS().GetID());

        WWW www = new WWW(PS().URL(URLHIGHSCORE), form);
        yield return www;

        // Set the highscore text
        txtHighScore.text = www.text;


        //// Build the form for submission
        //form = new WWWForm();
        //form.AddField("mail", txtFPEmail.text);

        //WWW passResetReq = new WWW(ps.URL(URLREQUESTRESET), form);
        //yield return passResetReq;

        //// Check for successful web request
        //if (string.IsNullOrEmpty(passResetReq.error))
        //{
        //    // Convert response to JSON
        //    User user = JsonUtility.FromJson<User>(passResetReq.text);

        //    // Print the response message (error or success)
        //    txtFeedback.text = user.msg;

        //    // Only proceed if email exists and account is active
        //    if (user.query == true && user.success == true)
        //    {
        //        // Successful reset request, display the confirmation panel
        //        DisplayPassConfPanel();
        //    }
        //}
        //else
        //{
        //    // Connection failed
        //    txtFeedback.text = "An error occured talking to the server";
        //}
    }
}
