using System.Collections.Generic;
using UnityEngine;

public class AiPatrolState : AiState
{
    RaycastHit2D hitInfo;
    Vector3Int destCellpos;

    List<Vector3Int> pathRoad;

    int index;

    public AiPatrolState(Creature creature, Controller controller) : base(creature, controller)
    {

    }

    public override bool CheckCondition()
    {
        controller.LookDir = (Random.Range(0, 2) == 0 ? Vector2.right : Vector2.left);

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
        float randomRange = Random.Range(0, creature.GetStat.PatrolDistance);


        hitInfo = Physics2D.Raycast(startPos, controller.LookDir, randomRange, 1 | 1 << 6 | 1 << 9);

        if (hitInfo.collider == null)
        {
            startPos += controller.LookDir * randomRange;
            hitInfo = Physics2D.Raycast(startPos, Vector2.down, 2.0f, 1 << 6 | 1 << 9);
            
            if (hitInfo.collider)
            {
                Vector3Int myCellPos = Managers.Map.CurrentGrid.WorldToCell(creature.transform.position);
                destCellpos = Managers.Map.CurrentGrid.WorldToCell(hitInfo.point);
                pathRoad = Managers.Map.FindPath(myCellPos, destCellpos);

                if (pathRoad.Count > 1)
                {
                    Vector3Int nextPos = pathRoad[1];
                    return Managers.Map.CanGo(nextPos);
                }
            }
        }

        return false;
    }

    public override void EnterState()
    {
        index = 1;
        creature.GetAnimator.SetFloat("MoveSpeed", 0.5f);
    }

    public override void ExitState()
    {
        creature.GetAnimator.SetFloat("MoveSpeed", 0.0f);
        creature.GetRigidbody.gravityScale = 1.0f;
    }

    public override void FixedState()
    {
        if (creature.GetStateMachine.ChangeState(Define.State.Chase))
            return;

        if (index < pathRoad.Count)
        {
            Vector3Int nextPos = pathRoad[index];
            if (Managers.Map.CanGo(nextPos))
            {
                Vector3 worldPos = Managers.Map.CurrentGrid.CellToWorld(nextPos) + new Vector3(0.5f, 0.0f);
                Vector2 dir = (worldPos - creature.transform.position).normalized;
                
                float dist = (worldPos - creature.transform.position).magnitude;


                if (dir.x > 0)
                    controller.LookDir = Vector2.right;
                else
                    controller.LookDir = Vector2.left;

                creature.GetRigidbody.gravityScale = 0.0f;
                
                if ((creature.GetStat.MoveSpeed + 5) * Time.fixedDeltaTime >= dist)
                {
                    creature.GetRigidbody.position = worldPos;
                    index++;
                }
                else
                {
                    Vector2 moveDir = (worldPos - creature.transform.position).normalized;
                    creature.GetRigidbody.position += moveDir * (creature.GetStat.MoveSpeed + 5) * Time.fixedDeltaTime;
                }

                return;
            }
        }

        creature.GetStateMachine.ChangeState(Define.State.Idle); 
    }

    public override void UpdateState()
    {
       
    }
}
