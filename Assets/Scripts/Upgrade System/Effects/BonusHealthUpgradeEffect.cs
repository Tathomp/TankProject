using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Health Upgrade Effect", menuName = "Upgrade/HealthUpgrade")]
public class BonusHealthUpgradeEffect : UpgradeEffect
{
    public int HealthIncrease;

    public override void AddEffect()
    {
        Debug.Log("Health Added: " + HealthIncrease);
    }

    public override void RemoveEffect()
    {
        Debug.Log("Health Removed: " + HealthIncrease);
    }
}
