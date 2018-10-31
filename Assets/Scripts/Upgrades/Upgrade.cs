using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Upgrade", menuName = "Upgrade/TestUpgrade")]
public class Upgrade : ScriptableObject
{
    public List<UpgradeEffect> UpgradeEffects;

    public void Test()
    {
        foreach (UpgradeEffect upgradeEffect in UpgradeEffects)
        {
            upgradeEffect.Test();
        }
    }
}
