using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiJumpSwingState : AiState
{
    public AiJumpSwingState(Creature creature, Controller controller) : base(creature, controller)
    {

    }

    public override void EnterState()
    {
        Managers.Sound.Play2D($"Sounds/jump_01");
        Managers.Sound.Play2D($"Sounds/throw_01");

        Vector2 dir = (controller.Target.transform.position - creature.transform.position).normalized;

        if (dir.x > 0)
            creature.GetController.LookDir = Vector2.right;
        else
            creature.GetController.LookDir = Vector2.left;

        creature.GetRigidbody.AddForce(Vector2.up * 10.0f, ForceMode2D.Impulse);
        creature.GetAnimator.SetBool("IsJumpSwing", true);
    }

    public override void ExitState()
    {
        creature.GetAnimator.SetBool("IsJumpSwing", false);
    }

    public override void FixedState()
    {
        if (Physics2D.Raycast(creature.transform.position, Vector2.down, 0.1f, 1 << 6))
        {
            creature.GetStateMachine.ChangeState(Define.State.Idle);
        }
    }

    public override void UpdateState()
    {
       
    }

    public void Fire()
    {
        GameObject go = Managers.Resource.Instantiate("Prefabs/Projectile/JumpSwingAxe");
        if (go.TryGetComponent(out JumpSwingAxe jumpSwingAxe))
            jumpSwingAxe.SetProjectile(creature.transform.position + new Vector3(0, 1.4f, 0), Vector2.zero, creature.GetStat.EnemyLayer, creature);

        creature.GetAnimator.SetTrigger("ThrowWeaponTrigger");
    }
}
