using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour {

    public GameObject selected, locked;
  

    //Upgrade Selected
    public void SelectUpgrade()
    {
        selected.SetActive(true);
    }

    public void DeSelectUpgrade()
    {
        selected.SetActive(false);
    }

    public void UnlockUpgrade()
    {
        locked.SetActive(false);
    }
}
