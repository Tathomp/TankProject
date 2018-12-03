using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Gun Upgrade Effect", menuName = "Upgrade/GunUpgrade")]
public class BonusGunUpgradeEffect : UpgradeEffect
{
    public int NumberOfShots;
    public BulletController player, enemy, bigEnemy, megaEnemy;
    public BulletController playerDefaullt, enemyDefault, bigEnemyDefault, megaEnemyDefault;

    public override void AddEffect(Tank tank)
    {
        if(tank is PlayerController)
        {
            PlayerController pc = (PlayerController)tank;

            pc.playerGun.projectile = player;
        }
        else
        {
            EnemyScript es = (EnemyScript)tank;

            if(es.enemyGun.projectile.tag == "GiantEnemyProjectile" )
            {
                es.enemyGun.projectile = bigEnemy;
            }
            else if(es.enemyGun.projectile.tag=="MegaProjectile")
            {
                es.enemyGun.projectile = megaEnemy;

            }
            else
            {
                es.enemyGun.projectile = enemy;
            }
        }

        Debug.Log("Gun Upgrade Applied.");
    }

    public override void RemoveEffect(Tank tank)
    {
        // throw new System.NotImplementedException();
        if (tank is PlayerController)
        {
            PlayerController pc = (PlayerController)tank;

            pc.playerGun.projectile = playerDefaullt;
        }
        else
        {
            EnemyScript es = (EnemyScript)tank;

            if (es.enemyGun.projectile.tag == "GiantEnemyProjectile")
            {
                es.enemyGun.projectile = bigEnemyDefault;
            }
            else if (es.enemyGun.projectile.tag == "MegaProjectile")
            {
                es.enemyGun.projectile = megaEnemyDefault;

            }
            else
            {
                es.enemyGun.projectile = enemyDefault;
            }
        }
    }
}
