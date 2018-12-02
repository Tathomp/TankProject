using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Health Upgrade Effect", menuName = "Upgrade/HealthUpgrade")]
public class BonusHealthUpgradeEffect : UpgradeEffect
{
    public int HealthIncrease;

    public override void AddEffect(Tank tank)
    {
        Debug.Log("Health Added: " + HealthIncrease);
        tank.MaxHealth += HealthIncrease;
    }

    public override void RemoveEffect(Tank tank)
    {
        Debug.Log("Health Removed: " + HealthIncrease);
        tank.MaxHealth -= HealthIncrease;

    }
}
