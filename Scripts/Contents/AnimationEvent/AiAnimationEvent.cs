 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiAnimationEvent : MonoBehaviour
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

    public void OnReloadSound()
    {
        if (creature.GetWeapon is Range range)
            range.PlayReloadSound();
    }
}