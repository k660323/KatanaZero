using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Range : Weapon
{
    // 원거리 변수
    [SerializeField]
    public string projectileName;

    [SerializeField]
    public string fireEffect;

    [SerializeField]
    public string smokeEffect;

    [SerializeField]
    public string[] reloadSound;

    public override void Attack()
    {
        if (_isAttack)
        {
            // 오브젝트 발사
            GameObject projectileObj = Managers.Resource.Instantiate($"Prefabs/Projectile/{projectileName}");
            if (projectileObj.TryGetComponent(out Bullet bullet))
            {
                bullet.SetProjectile(creature.AtkPos.position, controller.LookDir, creature.GetStat.EnemyLayer, creature);
            }
        }
    }

    public override void PlayEffect()
    {
        GameObject effectObj;
        Effect effect;
        effectObj = Managers.Resource.Instantiate($"Prefabs/Effect/{smokeEffect}");
        if (effectObj != null && effectObj.TryGetComponent(out effect))
            effect.InitEffect(creature.AtkPos.position, controller.LookDir);

        effectObj = Managers.Resource.Instantiate($"Prefabs/Effect/{fireEffect}");
        if (effectObj != null && effectObj.TryGetComponent(out effect))
            effect.InitEffect(creature.AtkPos.position, controller.LookDir);
    }

    public void PlayReloadSound()
    {
        if (reloadSound.Length > 0)
        {
            Managers.Sound.Play2D($"Sounds/{reloadSound[Random.Range(0, reloadSound.Length)]}");
        }
    }
}
