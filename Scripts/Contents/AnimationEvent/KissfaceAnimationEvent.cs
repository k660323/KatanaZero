using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KissfaceAnimationEvent : MonoBehaviour
{
    public Creature creature;
    public Controller controller;

    private void Awake()
    {
        TryGetComponent(out creature);
        TryGetComponent(out controller);
    }
    public void OnAttackStart()
    {
        if (creature.GetStateMachine.EState == Define.State.Attack)
        {
            creature.GetWeapon._isAttack = true;
        }
    }

    public void OnAttackEnd()
    {
        if (creature.GetStateMachine.EState == Define.State.Attack)
        {
            creature.GetWeapon._isAttack = false;
        }
    }

    public void OnFire()
    {
        if (creature.GetStateMachine.EState == Define.State.Attack)
        {
            creature.GetWeapon._isAttack = true;
            creature.GetWeapon.Attack();
        }
    }

    public void OnAttackComplete()
    {
        StartCoroutine(creature.GetWeapon.StartAtkCoolTime());

        if (creature.GetStateMachine.ChangeState(Define.State.Attack))
            return;

        creature.GetStateMachine.ChangeState(Define.State.Idle);
    }

    public void OnGroundDeadComplete()
    {
       
    }

    public void OnFlyingDeadComplete()
    {
        
    }

    public void OnThrowWeapon()
    {
        // µµ≥¢ ≈ı√¥
        if(creature.GetStateMachine.CurState is AiThrowWeaponState throwWeaponState)
        {
            throwWeaponState.Fire();
        }
    }

    public void OnFinishedReturnWeapon()
    {
        creature.GetStateMachine.ChangeState(Define.State.Idle);
    }

    public void OnSwingWeapon()
    {
        if (creature.GetStateMachine.CurState is AiJumpSwingState JumpSwingState)
        {
            JumpSwingState.Fire();
        }
    }

    public void OnPreLunge()
    {
        if(creature.GetStateMachine.CurState is AiJumpAttackState jumpAttackState)
        {
            jumpAttackState.GetSetjumpAttackState = AiJumpAttackState.JumpAttackState.Lunge;
        }
    }

    public void OnLangeAttackStart()
    {
        if (creature.GetStateMachine.CurState is AiJumpAttackState jumpAttackState)
        {
            jumpAttackState._isAttack = true;
        }
    }

    public void OnLangeAttackEnd()
    {
        if (creature.GetStateMachine.CurState is AiJumpAttackState jumpAttackState)
        {
            jumpAttackState._isAttack = true;
        }
    }

    public void OnFinisihedLandAttack()
    {
        creature.GetStateMachine.ChangeState(Define.State.Idle);
    }

    public void OnFinishedBlock()
    {
        creature.GetStateMachine.ChangeState(Define.State.Idle);
    }

    public void OnFinishedHit()
    {
        if (creature.GetStateMachine.CurState is AiKissfaceHitState kissfaceHitState)
        {
            kissfaceHitState.WakeUp();
        }
    }

    public void OnFinishedWakeUp()
    {
        creature.GetStateMachine.ChangeState(Define.State.Idle);
    }

    public void OnFinishedDie()
    {
        if (creature.GetStateMachine.CurState is AiKissfaceDeadState kissfaceDeadState)
        {
            kissfaceDeadState.GroundDead();
            return;
        }

        creature.GetStateMachine.ChangeState(Define.State.Idle);
    }

    public void OnCreateObject()
    {
        if (creature.GetStateMachine.CurState is AiKissfaceDeadState kissfaceDeadState)
        {
            kissfaceDeadState.CreateObject();
        }
    }

    public void OnCatchWeaponSound()
    {
        Managers.Sound.Play2D($"Sounds/axecatch_01");
    }

    public void OnFinishiedToBattle()
    {
        creature.GetStateMachine.ChangeState(Define.State.Idle);
    }
}
