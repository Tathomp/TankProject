using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Profile : MonoBehaviour {

    private readonly string URLHIGHSCORE = "action_gethighscore.php";

    // State references, initialized in Start()
    PlayerState ps;
    Link l;

    /***************************************
                   Game Objects
    **************************************/
    // Panels
    public GameObject profileUI;
    // Text
    public Text txtChangePassword, txtCredits, txtEmail, txtHighScore, txtUpdatePicture, txtUserName;
    // Image
    public Image profileImage, profileImage2;

    void Start()
    {
        ps = PlayerState.GetCurrentPlayerState();
        l = Link.GetLinkState();
    }

    // Display the player's profile
    public void DisplayProfilePanel()
    {
        // Activate profile panel
        profileUI.SetActive(true);
        
        // Set the profile text to reflect state attributes
        txtCredits.text = ps.GetCredits().ToString();
        txtEmail.text = ps.GetEmail();
        txtUserName.text = ps.GetUserName();
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

    // Update picture
    public void UpdatePictureButtonTapped()
    {
        int id = ps.GetID();
        string img = ps.GetUserImage();
        string url = "https://www.ninjalive.com/tanks/avatars/upload.html?id=" + id.ToString() + "&img=" + img;
        l.OpenLinkJSPlugin(url);
    }


    /***************************************
                Coroutines
     **************************************/
    IEnumerator Avatar()
    {
        WWW www = new WWW(ps.GetAvatar());
        yield return www;
        profileImage.sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));
        profileImage2.sprite = profileImage.sprite;
    }

    private IEnumerator HighScore()
    {
        // Build the form for submission
        WWWForm form = new WWWForm();
        form.AddField("id", ps.GetID());

        WWW www = new WWW(ps.URL(URLHIGHSCORE), form);
        yield return www;

        // Set the highscore text
        txtHighScore.text = www.text;
    }
}
