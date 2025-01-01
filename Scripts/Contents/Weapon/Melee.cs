using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : Weapon
{
    // 근접 변수
    [SerializeField]
    public string swingEffect;

    public Vector2 _attackSize = Vector2.one;

    public override void Attack()
    {
        if (_isAttack)
        {
            RaycastHit2D hit = Physics2D.BoxCast(creature.AtkPos.position, _attackSize, 0.0f, controller.LookDir, 0.0f, creature.GetStat.EnemyLayer);
            if (hit == false)
                return;

            // 오브젝트 공격
            if (hit.collider)
            {
                if (hit.transform.TryGetComponent(out IHitable hitable))
                {
                    hitable.OnHit(gameObject, true);
                }
            }
        }
    }

    public void FixedUpdate()
    {
        Attack();
    }

    public override void PlayEffect()
    {
        if (swingEffect == "")
            return;

        GameObject effectObj = Managers.Resource.Instantiate($"Prefabs/Effect/{swingEffect}");
        if (effectObj != null && effectObj.TryGetComponent(out Effect efftect))
            efftect.InitEffect(creature.transform.position, controller.LookDir);

    }
}
