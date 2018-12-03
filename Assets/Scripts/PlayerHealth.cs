using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {

    public RectTransform MaxLength, CurrentLength;
    public PlayerController PlayerTank;

	// Update is called once per frame
	void Update ()
    {
        float healthpercent = (float)PlayerTank.CurrentHealth / (float)PlayerTank.MaxHealth;
        CurrentLength.sizeDelta = new Vector2(MaxLength.rect.width * healthpercent, CurrentLength.rect.height);
	}
}
