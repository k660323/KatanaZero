using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class AiToBattleState : AiState
{
    public AiToBattleState(Creature creature, Controller controller) : base(creature, controller)
    {
    }

    public override void EnterState()
    {
        creature.GetAnimator.SetTrigger("ToBattleTrigger");
    }

    public override void ExitState()
    {
        
    }

    public override void FixedState()
    {
        Vector2 dir = (controller.Target.transform.position - creature.transform.position).normalized;

        if (dir.x > 0)
            controller.LookDir = Vector2.right;
        else
            controller.LookDir = Vector2.left;
    }

    public override void UpdateState()
    {
        
    }
}
