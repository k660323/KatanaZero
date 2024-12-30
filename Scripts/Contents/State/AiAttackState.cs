using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AiAttackState : AiState
{
    public AiAttackState(Creature creature, Controller controller) : base(creature, controller)
    {

    }

    public override bool CheckCondition()
    {
        if (controller.Target == null)
            return false;

        Vector2 dir = (controller.Target.transform.position - creature.transform.position).normalized;

        if (dir.x > 0)
            creature.GetController.LookDir = Vector2.right;
        else
            creature.GetController.LookDir = Vector2.left;

        if (creature.GetWeapon._isCoolTime)
            return false;


        Vector2 size = Vector2.zero;
        if (creature)
        {
            Vector2 tmp = creature.GetSize() * 0.5f;

            if (creature.GetController.LookDir == Vector2.left)
            {
                size =  new Vector2(-tmp.x, tmp.y);
            }
            else if (creature.GetController.LookDir == Vector2.right)
            {
                size =  new Vector2(tmp.x, tmp.y);
            }
        }

        Vector2 startPos = (Vector2)creature.transform.position + size;
        float distance = Vector2.Distance(startPos, controller.Target.transform.position);

        if (creature.GetStat.AtkRange < distance)
            return false;

        return true;
    }

    public override void EnterState()
    {
        creature.GetWeapon?.StartAtk(this);
    }

    public override void ExitState()
    {
        creature.GetWeapon?.EndAtk(this);
    }

    public override void FixedState()
    {
        
    }

    public override void UpdateState()
    {
        
    }
}
