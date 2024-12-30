using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiHitState : AiState
{
    public AiHitState(Creature creature, Controller controller) : base(creature, controller)
    {
    }

    public override void EnterState()
    {
        creature.GetAnimator.SetTrigger("HitTrigger");
        creature.GetAnimator.SetBool("IsHit", true);

    }

    public override void ExitState()
    {
        creature.GetAnimator.SetBool("IsHit", false);
    }

    public override void FixedState()
    {
        
    }

    public override void UpdateState()
    {
        
    }
}
