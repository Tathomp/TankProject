using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour {

    Upgrade upgrade;

    // Will pass the upgrade that is associated with this button
    // through this method
	public void InitButton(Upgrade upgrade)
    {
        this.upgrade = upgrade;

        this.transform.GetChild(0).GetComponent<Text>().text = upgrade.Upgradename;
    }


    // This method should be hooked up to the OnClick method through the editor
    public void ButtonClicked()
    {
        Debug.Log(upgrade.name + " butotn clicked");

        if(upgrade.upgradeType == UpgradeType.Armor)
        {
            PlayerData.CurrentPlayerInstance.ArmorUpgrade = upgrade;
        }
        else if(upgrade.upgradeType == UpgradeType.Gun)
        {
            PlayerData.CurrentPlayerInstance.GunUpgrade = upgrade;
        }
        else if (upgrade.upgradeType == UpgradeType.Track)
        {
            PlayerData.CurrentPlayerInstance.TrackUpgrade = upgrade;
        }
    }
}
