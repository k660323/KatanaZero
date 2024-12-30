using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiGroundDeadState : DeadState
{
    public string[] bloodsplat = { "bloodsplat", "bloodsplat2", "bloodsplat3", "bloodsplat4" };
    public AiGroundDeadState(Creature creature, Controller controller) : base(creature, controller)
    {
    }

    public override bool CheckCondition()
    {
        return creature.GetStateMachine.EState != Define.State.GroundDead && creature.GetStateMachine.EState != Define.State.FlyDead;
    }

    public override void EnterState()
    {
        creature.transform.gameObject.tag = "Dead";
        creature.transform.gameObject.layer = LayerMask.NameToLayer("Dead");

        Managers.Sound.Play2D($"Sounds/{bloodsplat[Random.Range(0, 4)]}");

        GameObject go = Managers.Resource.Instantiate("Prefabs/Effect/GroundBlood");
        if(go.TryGetComponent(out Effect effect))
        {
            Vector3 pos = creature.transform.position;
            if(creature)
                pos = new Vector3(pos.x, pos.y + creature.GetHeight() * 0.5f, pos.z);

            effect.InitEffect(pos, controller.LookDir, creature.gameObject);
        }
        creature.GetAnimator.SetTrigger("DeadTrigger");
        creature.GetAnimator.SetBool("IsGroundDead", true);
    }

    public override void ExitState()
    {
        creature.GetAnimator.SetBool("IsFlyDead", false);
    }

    public override void FixedState()
    {
       
    }

    public override void UpdateState()
    {
        
    }
}
