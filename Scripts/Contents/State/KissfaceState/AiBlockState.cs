using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiBlockState : AiState
{
    public AiBlockState(Creature creature, Controller controller) : base(creature, controller)
    {
    }

    public override bool CheckCondition()
    {
        return controller.Target;
    }

    public override void EnterState()
    {
        Managers.Sound.Play2D($"Sounds/clash_01");

        Vector2 dir = (controller.Target.transform.position - creature.transform.position).normalized;

        if (dir.x > 0)
            creature.GetController.LookDir = Vector2.right;
        else
            creature.GetController.LookDir = Vector2.left;

        Rigidbody2D rigidbody2D = controller.Target.GetComponent<Rigidbody2D>();
        if (rigidbody2D)
        {
            rigidbody2D.AddForce(creature.GetController.LookDir * 10000.0f, ForceMode2D.Impulse);
        }
        creature.GetRigidbody.AddForce(creature.GetController.LookDir * -15.0f, ForceMode2D.Impulse);

        creature.GetAnimator.SetTrigger("BlockTrigger");
        creature.GetAnimator.SetBool("IsBlock", true);
    }

    public override void ExitState()
    {
        creature.GetAnimator.SetBool("IsBlock", false);
    }

    public override void FixedState()
    {
        
    }

    public override void UpdateState()
    {
        
    }
}
