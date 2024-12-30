using Cinemachine;
using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiJumpAttackState : AiState
{
    public enum JumpAttackState
    {
        None,
        PreLunge,
        Lunge,
        LungeAttack
    }

    JumpAttackState jumpAttackState;

    public JumpAttackState GetSetjumpAttackState
    {
        get 
        { 
            return jumpAttackState; 
        }
        set 
        { 
            jumpAttackState = value;

            switch (jumpAttackState)
            {
                case JumpAttackState.None:
                    creature.GetAnimator.SetBool("IsJumpAttack", false);
                    break;
                case JumpAttackState.PreLunge:
                    Managers.Sound.Play2D($"Sounds/axeprep_01");
                    creature.GetAnimator.SetBool("IsJumpAttack", true);
                    break;
                case JumpAttackState.Lunge:
                    Managers.Sound.Play2D($"Sounds/axelunge_01");
                    creature.GetAnimator.SetTrigger("LungeTrigger");
                    RaycastHit2D hit2D = Physics2D.Raycast(controller.Target.transform.position, Vector2.down, 100.0f, 1 << 6);
                    if (hit2D)
                        destPos = hit2D.point;
                    else
                        destPos = controller.Target.transform.position;

                    if (creature.transform.position.x <= controller.Target.transform.position.x)
                        destPos += new Vector3(-2.0f, 0, 0);
                    else
                        destPos += new Vector3(2.0f, 0, 0);

                    break;
                case JumpAttackState.LungeAttack:
                    creature.GetAnimator.SetTrigger("LugeAttackTrigger");
                    
                    if(CameraShakeCor != null)
                    {
                        creature.StopCoroutine(CameraShakeCor);
                        originPos = Camera.main.transform.position;
                    }
                    CameraShakeCor = CameraShake();
                    creature.StartCoroutine(CameraShakeCor);
                    break;
            }
        }
    }

    Vector3 destPos = Vector3.zero;

    public bool _isAttack;

    Vector2 _attackSize;

    Vector3 originPos;

    IEnumerator CameraShakeCor;

    public AiJumpAttackState(Creature creature, Controller controller, Vector2 attackSize) : base(creature, controller)
    {
        _attackSize = attackSize;
    }

    public override bool CheckCondition()
    {
        return controller.Target;
    }

    public override void EnterState()
    {
        Vector2 dir = (controller.Target.transform.position - creature.transform.position).normalized;

        if (dir.x > 0)
            creature.GetController.LookDir = Vector2.right;
        else
            creature.GetController.LookDir = Vector2.left;

        GetSetjumpAttackState = JumpAttackState.PreLunge;
    }

    public override void ExitState()
    {
        GetSetjumpAttackState = JumpAttackState.None;
        _isAttack = false;
    }

    public override void FixedState()
    {
        if(GetSetjumpAttackState == JumpAttackState.Lunge)
        {
            creature.transform.position = Vector3.MoveTowards(creature.transform.position, destPos, 30.0f * Time.fixedDeltaTime);
            float distance = (creature.transform.position - destPos).magnitude;
            if(distance <= 0.1f)
            {
                GetSetjumpAttackState = JumpAttackState.LungeAttack;
            }
        }
        else if(GetSetjumpAttackState == JumpAttackState.LungeAttack)
        {
            if (_isAttack)
            {
                RaycastHit2D hit = Physics2D.BoxCast(creature.AtkPos.position, _attackSize, 0.0f, controller.LookDir, 0.0f, creature.GetStat.EnemyLayer);
                if (hit == false)
                    return;

                // 오브젝트 공격
                if(hit.collider != null)
                {
                    if(hit.transform.TryGetComponent(out IHitable hitable))
                    {
                        hitable.OnHit(creature.gameObject);
                    }
                }

            }
        }
    }

    public override void UpdateState()
    {
       
    }

    IEnumerator CameraShake()
    {
        float duration = 1.0f;
        float roughness = 5.0f;
        float magnitude = 5.0f;

        float halfDuration = duration / 2.0f;
        float elapsed = 0.0f;
        float tick = Random.Range(-10.0f, 10.0f);

       

        while(elapsed < duration)
        {
            elapsed += Time.deltaTime / halfDuration;

            tick += Time.deltaTime * roughness;
            Camera.main.transform.position = new Vector3(
                Mathf.PerlinNoise(tick, 0) - 0.5f,
                Mathf.PerlinNoise(0, tick) - 0.5f,
                0.0f) * magnitude * Mathf.PingPong(elapsed, halfDuration);

            yield return null;
        }

        Camera.main.transform.position = originPos;
    }
}
