using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    protected Creature creature;
    protected Controller controller;

    public bool _isAttack = false;
    [HideInInspector]
    public bool _isCoolTime = false;
    public float _coolTime = 0.0f;
    public float _curTime = 0.0f;

    [SerializeField]
    public string atkSound;

    private void Awake()
    {
        TryGetComponent(out creature);
        TryGetComponent(out  controller);
    }

    public virtual void StartAtk(BaseState baseState)
    {
        PlaySound();
        PlayEffect();

        creature.GetAnimator.SetBool("IsAttack", true);
        if (creature.GetStateMachine.TempState == baseState)
            creature.GetAnimator.SetTrigger("AttackTrigger");
    }

    public virtual void EndAtk(BaseState baseState)
    {
        creature.GetAnimator.SetBool("IsAttack", false);
    }

    public abstract void Attack();

    public IEnumerator StartAtkCoolTime()
    {
        _isAttack = false;
        _isCoolTime = true;
        _curTime = _coolTime;
        while (_curTime > 0.0f)
        {
            _curTime -= Time.deltaTime;
            yield return null;
        }
        _isCoolTime = false;
    }

    public void PlaySound()
    {
        if (atkSound != "")
            Managers.Sound.Play2D($"Sounds/{atkSound}");
    }

    public abstract void PlayEffect();
}
