using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bonus Speed Upgrade Effect", menuName = "Upgrade/SpeedUpgrade")]
public class BonusSpeedUpgradeEffect : UpgradeEffect
{
    [Tooltip("Speed Multiplier")]
    public float SpeedIncrease;


    public override void AddEffect(Tank tank)
    {
        Debug.Log("Speed Added: " + SpeedIncrease);
        if(tank is PlayerController)
        {
            PlayerController pc = (PlayerController)tank;
            pc.forwardSpeed = pc.forwardSpeed * SpeedIncrease;
        }
    }

    public override void RemoveEffect(Tank tank)
    {
        Debug.Log("Speed Removed: " + SpeedIncrease);

        if (tank is PlayerController)
        {
            PlayerController pc = (PlayerController)tank;
            pc.forwardSpeed = pc.forwardSpeed / SpeedIncrease;
        }
    }
}
