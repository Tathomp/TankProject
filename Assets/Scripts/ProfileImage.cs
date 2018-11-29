using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ProfileImage : MonoBehaviour {

    // The output of the image
    public Image profileImage;

    // PlayerState accessor
    public PlayerState PS()
    {
        return GameObject.Find("Manager").GetComponent<PlayerState>();
    }

    IEnumerator Start()
    {
        WWW www = new WWW(PS().GetAvatar());
        yield return www;
        profileImage.sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));
    }
}
