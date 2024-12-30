using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiKissfaceResistanceState : AiState
{
    float _resistanceTime = 3.0f;
    float _curResistanceTime = 0.0f;
    float _maxStateGauge = 0.2f;
    float _stateGauge = 0.0f;
    float _maxGauge = 1.0f;
    float _curGauge = 0.0f;
    float _speed = 10.0f;

    public AiKissfaceResistanceState(Creature creature, Controller controller) : base(creature, controller)
    {
    }

    public override bool CheckCondition()
    {
        return creature.GetStateMachine.EState == Define.State.Hit;
    }

    public override void EnterState()
    {
        if (PlayerMove.player)
            PlayerMove.player.gameObject.SetActive(false);

        if (creature is KissfaceEnemy kissfaceEnemy)
            kissfaceEnemy.ShowGaugeUI(true);

        _curResistanceTime = 0.0f;
        _stateGauge = 0.0f;
        creature.GetAnimator.SetBool("IsResistance", true);
    }

    public override void ExitState()
    {
        if (PlayerMove.player)
            PlayerMove.player.gameObject.SetActive(true);

        if (creature is KissfaceEnemy kissfaceEnemy)
            kissfaceEnemy.ShowGaugeUI(false);

        creature.GetAnimator.SetBool("IsResistance", false);
    }

    public override void FixedState()
    {
      
       
    }

    public override void UpdateState()
    {
        if(Input.GetKeyDown(KeyCode.X))
        {
            float gauge = _speed * Time.deltaTime;
            _stateGauge += gauge;
            _curGauge += gauge;

            if (creature is KissfaceEnemy kissfaceEnemy)
                kissfaceEnemy.SetPercent(_curGauge, _maxGauge);

            if (_curGauge >= _maxGauge)
                if (creature.GetStateMachine.ChangeState(Define.State.GroundDead))
                    return;

            if(_stateGauge >= _maxStateGauge)
                if (creature.GetStateMachine.ChangeState(Define.State.WakeUp))
                    return;
        }

        _curResistanceTime += Time.deltaTime;
        if (_curResistanceTime >= _resistanceTime)
            if (creature.GetStateMachine.ChangeState(Define.State.WakeUp))
                return;
    }
}
