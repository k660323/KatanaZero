using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiWakeUpState : AiState
{
    int myLayer;

    public AiWakeUpState(Creature creature, Controller controller) : base(creature, controller)
    {
    }

    public override void EnterState()
    {
        creature.GetAnimator.SetBool("IsWakeUp", true);
        Managers.Sound.Play2D($"Sounds/slide");
        myLayer = creature.gameObject.layer;
        creature.gameObject.layer = LayerMask.NameToLayer("Invincibility");
    }

    public override void ExitState()
    {
        creature.gameObject.layer = myLayer;
        creature.GetAnimator.SetBool("IsWakeUp", false);
    }

    public override void FixedState()
    {
        
    }

    public override void UpdateState()
    {
       
    }
}
