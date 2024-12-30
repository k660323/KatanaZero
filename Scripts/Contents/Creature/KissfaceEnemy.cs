using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KissfaceEnemy : Creature
{
    GameObject resistanceGauge;
    Image gauge;

    GameObject buttonPush;

    protected override void Awake()
    {
        base.Awake();
        resistanceGauge = transform.Find("ResistanceGauge").gameObject;
        gauge = resistanceGauge.transform.Find("Gauge").GetComponent<Image>();
        buttonPush = transform.Find("PushKeyBoard_X").gameObject;
        ShowGaugeUI(false);
        ShowInterectUI(false);
    }

    public override void InitStateMachine()
    {
        AiThrowWeaponState throwWeaponState = new AiThrowWeaponState(this, GetController);
        GetStateMachine.AddState(Define.State.Skill0, throwWeaponState);

        AiJumpSwingState jumpSwingState = new AiJumpSwingState(this, GetController);
        GetStateMachine.AddState(Define.State.Skill1, jumpSwingState);

        AiAttackState atkState = new AiAttackState(this, GetController);
        GetStateMachine.AddState(Define.State.Skill2, atkState);

        AiJumpAttackState jumpAtkState = new AiJumpAttackState(this, GetController, new Vector2(2, 2));
        GetStateMachine.AddState(Define.State.Skill3, jumpAtkState);

        AiKissfaceIdleState idleState = new AiKissfaceIdleState(this, GetController, 0.0f, 1.5f);      
        GetStateMachine.AddState(Define.State.Idle, idleState);

        AiKissfaceHitState hitState = new AiKissfaceHitState(this, GetController);
        GetStateMachine.AddState(Define.State.Hit, hitState);

        AiBlockState blockState = new AiBlockState(this, GetController);
        GetStateMachine.AddState(Define.State.Block, blockState);

        AiWakeUpState wakeUpState = new AiWakeUpState(this, GetController);
        GetStateMachine.AddState(Define.State.WakeUp, wakeUpState);

        AiKissfaceResistanceState KissfaceResistanceState = new AiKissfaceResistanceState(this, GetController);
        GetStateMachine.AddState(Define.State.Resistance, KissfaceResistanceState);

        AiKissfaceDeadState kissfaceDeadState = new AiKissfaceDeadState(this, GetController);  
        GetStateMachine.AddState(Define.State.GroundDead, kissfaceDeadState);

        AiToBattleState toBattleState = new AiToBattleState(this, GetController);
        GetStateMachine.DefaultState(Define.State.ToBattle, toBattleState);
    }

    public override void OnHit(GameObject owner, bool isGround = true)
    {
        if (GetStateMachine.EState == Define.State.Hit || GetStateMachine.EState == Define.State.WakeUp || GetStateMachine.EState == Define.State.Block)
            return;

        if (GetStateMachine.EState == Define.State.Idle)
            if (GetStateMachine.ChangeState(Define.State.Block))
                return;

        if (GetStateMachine.EState == Define.State.GroundDead)
        {
            GetStateMachine.ChangeState(Define.State.GroundDead);
            return;
        }

        GetStateMachine.ChangeState(Define.State.Hit);
    }

    public override void OnHit(GameObject owner, float damage, bool isGround = true)
    {
        if (GetStateMachine.EState == Define.State.Hit || GetStateMachine.EState == Define.State.WakeUp || GetStateMachine.EState == Define.State.Block)
            return;

        if (GetStateMachine.EState == Define.State.Idle)
            if (GetStateMachine.ChangeState(Define.State.Block))
                return;

        GetStateMachine.ChangeState(Define.State.Hit);
    }

    public void SetPercent(float curGauge, float maxGauge)
    {
        gauge.fillAmount = curGauge / maxGauge;
    }

    public void ShowGaugeUI(bool isActive)
    {
        if(isActive)
        {
            Vector3 localScalce = resistanceGauge.transform.localScale;
            if (transform.localScale.x >= 0)
                resistanceGauge.transform.localScale = new Vector3(Mathf.Abs(localScalce.x), localScalce.y, localScalce.z);
            else
                resistanceGauge.transform.localScale = new Vector3(-Mathf.Abs(localScalce.x), localScalce.y, localScalce.z);
        }

        resistanceGauge.SetActive(isActive);
    }

    public void ShowInterectUI(bool isActive)
    {
        if (isActive)
        {
            Vector3 localScalce = buttonPush.transform.localScale;
            if (transform.localScale.x >= 0)
                buttonPush.transform.localScale = new Vector3(Mathf.Abs(localScalce.x), localScalce.y, localScalce.z);
            else
                buttonPush.transform.localScale = new Vector3(-Mathf.Abs(localScalce.x), localScalce.y, localScalce.z);
        }

        buttonPush.SetActive(isActive);
    }
}
