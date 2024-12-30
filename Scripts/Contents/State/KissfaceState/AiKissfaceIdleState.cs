using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiKissfaceIdleState : AiIdleState
{

    Define.State nextState;

    public AiKissfaceIdleState(Creature creature, Controller controller, float minThinkTime, float maxThinkTime) : base(creature, controller, minThinkTime, maxThinkTime)
    {

    }

    public override void EnterState()
    {
        creature.GetRigidbody.velocity = Vector3.zero;
        creature.GetAnimator.SetFloat("MoveSpeed", 0.0f);
        curTime = Random.Range(_minThinkTime, _maxThinkTime);

        nextState = creature.GetStateMachine.RandomSelectSkill(creature.GetStateMachine.UseableSkill());
    }

    public override void ExitState()
    {
       
    }

    public override void FixedState()
    {
        if (controller.Target)
        {
            Vector2 dir = (controller.Target.transform.position - creature.transform.position).normalized;

            if (dir.x > 0)
                creature.GetController.LookDir = Vector2.right;
            else
                creature.GetController.LookDir = Vector2.left;
        }

        curTime -= Time.fixedDeltaTime;
        if (curTime <= 0.0f)
        {
            if (creature.GetStateMachine.ChangeState(nextState))
                return;

            nextState = creature.GetStateMachine.RandomSelectSkill(creature.GetStateMachine.UseableSkill());
        }
    }

    public override void UpdateState()
    {
      
    }
}
