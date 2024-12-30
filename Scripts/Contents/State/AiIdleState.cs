using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiIdleState : AiState
{
    protected float _maxThinkTime;
    protected float _minThinkTime;
    protected float curTime;

    public AiIdleState(Creature creature, Controller controller, float minThinkTime = 1.0f, float maxThinkTime = 3.0f) : base(creature, controller)
    {
        _maxThinkTime = maxThinkTime;
        _minThinkTime = minThinkTime;
    }

    public override void EnterState()
    {
        creature.GetAnimator.SetFloat("MoveSpeed", 0.0f);
        curTime = Random.Range(_minThinkTime, _maxThinkTime);
    }

    public override void ExitState()
    {
        
    }

    public override void FixedState()
    {
        if (creature.GetStateMachine.ChangeState(Define.State.Attack))
            return;

        if (creature.GetStateMachine.ChangeState(Define.State.Chase))
            return;

        if(controller.Target == null)
        {
            curTime -= Time.fixedDeltaTime;
            if (curTime <= 0.0f)
            {
                if (creature.GetStateMachine.ChangeState(Define.State.Patrol))
                    return;

                curTime = Random.Range(_minThinkTime, _maxThinkTime);
            }
        }
    }

    public override void UpdateState()
    {
        
    }
}
