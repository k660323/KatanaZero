using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiThrowWeaponState : AiState
{
    GameObject axeObject;

    public AiThrowWeaponState(Creature creature, Controller controller) : base(creature, controller)
    {
    }

    public override bool CheckCondition()
    {
        return controller.Target;
    }

    public override void EnterState()
    {
        Vector2 dir = (controller.Target.transform.position - creature.transform.position).normalized;

        if (dir.x > 0)
            creature.GetController.LookDir = Vector2.right;
        else
            creature.GetController.LookDir = Vector2.left;

        creature.GetAnimator.SetBool("IsThrowWeapon", true);
      
    }

    public override void ExitState()
    {
        if (axeObject != null && axeObject.TryGetComponent(out Axe _throwWeaponObject))
            _throwWeaponObject.DestroyProjectile();

        creature.GetAnimator.SetBool("IsThrowWeapon", false);
    }

    public override void FixedState()
    {
        
    }

    public override void UpdateState()
    {
        
    }

    public void Fire()
    {
        Managers.Sound.Play2D($"Sounds/axethrow_01");

        axeObject = Managers.Resource.Instantiate("Prefabs/Projectile/ThrowAxe");
        if (axeObject.TryGetComponent(out Axe _throwWeaponObject))
        {
            _throwWeaponObject.SetProjectile(creature.AtkPos.position, controller.LookDir,creature.GetStat.EnemyLayer, creature);
        }
    }
}
