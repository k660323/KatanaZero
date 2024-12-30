using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour, IHitable
{
    public StateMachine GetStateMachine;

    public Animator GetAnimator;

    public CapsuleCollider2D GetCollider;

    public Rigidbody2D GetRigidbody;

    public Controller GetController;

    public SpriteRenderer GetSpriteRenderer;

    public Stat GetStat;

    public Transform AtkPos;

    public Weapon GetWeapon;

    public GameObject FollowMark;

    public string[] death_Sword = { "death_sword_01", "death_sword_02" };
    public string death_Bullet = "death_bullet";

    protected virtual void Awake()
    {
        TryGetComponent(out GetStateMachine);
        TryGetComponent(out GetAnimator);
        TryGetComponent(out GetCollider);
        TryGetComponent(out GetRigidbody);
        TryGetComponent(out GetController);
        TryGetComponent(out GetSpriteRenderer);
        TryGetComponent(out GetStat);
        TryGetComponent(out GetWeapon);
        AtkPos = transform.Find("AtkPos").transform;
        FollowMark = transform.Find("FollowMark").gameObject;
        FollowMark.SetActive(false);
        InitStateMachine();
    }

    public virtual void InitStateMachine()
    {
        AiIdleState idleState = new AiIdleState(this, GetController);
        GetStateMachine.DefaultState(Define.State.Idle, idleState);

        AiPatrolState patrolState = new AiPatrolState(this, GetController);
        GetStateMachine.AddState(Define.State.Patrol, patrolState);

        AiChaseState chaseState = new AiChaseState(this, GetController);
        GetStateMachine.AddState(Define.State.Chase, chaseState);

        AiAttackState atkState = new AiAttackState(this, GetController);
        GetStateMachine.AddState(Define.State.Attack, atkState);

        AiGroundDeadState deadState = new AiGroundDeadState(this, GetController);
        GetStateMachine.AddState(Define.State.GroundDead, deadState);
    }

    public float GetHeight()
    {
        return GetCollider.size.y;
    }

    public Vector2 GetSize()
    {
        return GetCollider.size;
    }

    public virtual void OnHit(GameObject owner, bool isGround = true)
    {
        if (isGround)
        {
            if (GetStateMachine.ChangeState(Define.State.GroundDead))
            {
                Managers.Sound.Play2D($"Sounds/{death_Sword[Random.Range(0, 2)]}");
                return;
            }
        }
        else
        {
            if (GetStateMachine.ChangeState(Define.State.FlyDead))
            {
                Managers.Sound.Play2D($"Sounds/{death_Sword[Random.Range(0, 2)]}");
                return;
            }
        }
    }

    public virtual void OnHit(GameObject owner, float damage, bool isGround = true)
    {
        if (isGround)
        {
            if (GetStateMachine.ChangeState(Define.State.GroundDead))
            {
                Managers.Sound.Play2D($"Sounds/{death_Bullet}");
                return;
            }
        }
        else
        {
            if (GetStateMachine.ChangeState(Define.State.FlyDead))
            {
                Managers.Sound.Play2D($"Sounds/{death_Bullet}");
                return;
            }
        }
    }

    private void FixedUpdate()
    {
        GetStateMachine.CurState?.FixedState();
    }

    private void Update()
    {
        GetStateMachine.CurState?.UpdateState();
    }

    public void OnChestGround()
    {
        Managers.Sound.Play2D("Sounds/chestground");
    }
}
