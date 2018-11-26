using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour {

    public Upgrade upgrade;
    

    // Will pass the upgrade that is associated with this button
    // through this method
    public void InitButton(Upgrade upgrade)
    {
        this.upgrade = upgrade;

        this.transform.GetChild(0).GetComponent<Text>().text = upgrade.Upgradename;
    }

    private void Start()
    {
        transform.GetChild(0).GetComponent<Text>().text = upgrade.name;
    }

    // This method should be hooked up to the OnClick method through the editor
    public void ButtonClicked()
    {
        PlayerState ps = PlayerState.GetCurrentPlayerState();
        Debug.Log(upgrade.name + " butotn clicked");

        if(upgrade.upgradeType == UpgradeType.Armor)
        {
            //PlayerState.CurrentPlayerInstance.ArmorUpgrade = upgrade;
            ps.SetArmorUpgrade(upgrade);
        }
        else if(upgrade.upgradeType == UpgradeType.Gun)
        {
            //PlayerState.CurrentPlayerInstance.GunUpgrade = upgrade;
            ps.SetGunUpgrade(upgrade);
        }
        else if (upgrade.upgradeType == UpgradeType.Track)
        {
            //PlayerState.CurrentPlayerInstance.TrackUpgrade = upgrade;
            ps.SetTrackUpgrade(upgrade);
        }
    }
}
