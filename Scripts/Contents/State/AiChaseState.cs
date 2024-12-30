using System.Collections.Generic;
using UnityEngine;

public class AiChaseState : AiState
{
    RaycastHit2D hitInfo;

    public AiChaseState(Creature creature, Controller controller) : base(creature, controller)
    {
    }

    public override bool CheckCondition()
    {
        if (creature.GetWeapon._isCoolTime)
            return false;

        Vector2 size = Vector2.zero;
        if (creature)
        {
            Vector2 tmp = creature.GetSize() * 0.5f;

            if (creature.GetController.LookDir == Vector2.left)
            {
                size = new Vector2(-tmp.x, tmp.y);
            }
            else if (creature.GetController.LookDir == Vector2.right)
            {
                size = new Vector2(tmp.x, tmp.y);
            }
        }

        Vector2 startPos = (Vector2)creature.transform.position + size;

        if (controller.Target)
        {
            float distance = Vector2.Distance(startPos, controller.Target.transform.position);

            // 추적
            if (distance <= creature.GetStat.AtkRange)
                return false;
            else
                return true;
        }

        Vector2 lookDir = controller.LookDir;

        hitInfo = Physics2D.Raycast(startPos, lookDir, creature.GetStat.DetectRange, 1 | 1 << 6 | 1 << 9);
        if (hitInfo.collider != null)
            return false;

        hitInfo = Physics2D.Raycast(startPos, lookDir, creature.GetStat.DetectRange, 1 << 8);
        if(hitInfo.collider != null)
        {
            controller.Target = hitInfo.collider.gameObject;
            return true;
        }

        return false;
    }

    public override void EnterState()
    {
        creature.GetAnimator.SetFloat("MoveSpeed", 1.0f);
    }

    public override void ExitState()
    {
        creature.GetAnimator.SetFloat("MoveSpeed", 0.0f);
        creature.GetRigidbody.gravityScale = 1.0f;
    }

    public override void FixedState()
    {
        Vector2 size = Vector2.zero;
        if (creature)
        {
            Vector2 tmp = creature.GetSize() * 0.5f;

            if (creature.GetController.LookDir == Vector2.left)
            {
                size = new Vector2(-tmp.x, tmp.y);
            }
            else if (creature.GetController.LookDir == Vector2.right)
            {
                size = new Vector2(tmp.x, tmp.y);
            }
        }

        Vector2 startPos = (Vector2)creature.transform.position + size;
        float distance = Vector2.Distance(startPos, controller.Target.transform.position);
        Vector2 dir = (controller.Target.transform.position - creature.transform.position).normalized;

        if (dir.x > 0)
            controller.LookDir = Vector2.right;
        else
            controller.LookDir =  Vector2.left;

        int layer = ~(1 << 8 | 1 << 7 | 1 << 2);
        hitInfo = Physics2D.Raycast(startPos, dir, distance, layer);
        if (hitInfo.collider)
        {
            Vector3Int myCellPos = Managers.Map.CurrentGrid.WorldToCell(creature.transform.position);
            Vector3Int targetCellpos = Managers.Map.CurrentGrid.WorldToCell(controller.Target.transform.position);
            List<Vector3Int> list = Managers.Map.FindPath(myCellPos, targetCellpos);
            if (list.Count > 1)
            {
                Vector3Int nextPos = list[1];
                if (Managers.Map.CanGo(nextPos))
                {
                    Vector3 worldPos = Managers.Map.CurrentGrid.CellToWorld(nextPos) + new Vector3(0.5f, 0.0f);
                    float dist = (worldPos - creature.transform.position).magnitude;
                    creature.GetRigidbody.gravityScale = 0.0f;
                    // 남은 거리
                    if ((creature.GetStat.MoveSpeed + 5) * Time.fixedDeltaTime >= dist)
                    {
                        creature.GetRigidbody.position = worldPos;
                    }
                    else
                    {
                        Vector2 moveDir = (worldPos - creature.transform.position).normalized;
                        creature.GetRigidbody.position += moveDir * (creature.GetStat.MoveSpeed + 5) * Time.fixedDeltaTime;
                    }

                    return;
                }
            }
        }

        creature.GetRigidbody.gravityScale = 1.0f;
        if (distance > creature.GetStat.AtkRange)
        {
            creature.GetRigidbody.position += controller.LookDir * (creature.GetStat.MoveSpeed + 5) * Time.fixedDeltaTime;
        }
        else
        {
            if (creature.GetStateMachine.ChangeState(Define.State.Attack))
                return;
            if (creature.GetStateMachine.ChangeState(Define.State.Idle))
                return;
        }
    }

    public override void UpdateState()
    {
       
    }
}
