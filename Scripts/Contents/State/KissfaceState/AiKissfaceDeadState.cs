using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiKissfaceDeadState : DeadState
{
    public AiKissfaceDeadState(Creature creature, Controller controller) : base(creature, controller)
    {
    }

    public override bool CheckCondition()
    {
        return !creature.GetAnimator.GetBool("IsDie") || !creature.GetAnimator.GetBool("IsNoHead");
    }

    public override void EnterState()
    {
        if (creature.GetAnimator.GetBool("IsDie") == false)
        {
            if (PlayerMove.player)
                PlayerMove.player.gameObject.SetActive(false);

            creature.GetAnimator.SetBool("IsDie", true);
            Managers.Sound.Play2D("Sounds/death_01");

            if (creature.TryGetComponent(out BoxCollider2D boxCollider2D))
            {
                boxCollider2D.enabled = true;
                creature.GetCollider.enabled = false;
            }
        }
        else if(creature.GetAnimator.GetBool("IsNoHead") == false)
        {
            creature.GetAnimator.SetBool("IsNoHead", true);
            Managers.Sound.Play2D("Sounds/free_01");
            creature.transform.gameObject.tag = "Dead";
            creature.transform.gameObject.layer = LayerMask.NameToLayer("Dead");
        }
    }

    public void GroundDead()
    {
        if (PlayerMove.player)
            PlayerMove.player.gameObject.SetActive(true);

        creature.GetAnimator.SetBool("IsGroundDead", true);
    }

    public void CreateObject()
    {
        GameObject headObject = Managers.Resource.Instantiate("Prefabs/Object/HeadLand");
        headObject.transform.position = creature.transform.position + (Vector3)(-controller.LookDir * 3);

        Vector3 localScale = creature.transform.localScale;
        headObject.transform.localScale = new Vector3(-localScale.x, localScale.y, localScale.z);

    }

    public override void ExitState()
    {
        if (creature.GetStateMachine.CurState is AiKissfaceDeadState)
            return;

        creature.GetAnimator.SetBool("IsDie", false);
        creature.GetAnimator.SetBool("IsGroundDead", false);
        creature.GetAnimator.SetBool("IsNoHead", false);
        creature.transform.gameObject.tag = "Enemy";
        creature.transform.gameObject.layer = LayerMask.NameToLayer("Monster");

        if (creature.TryGetComponent(out BoxCollider2D boxCollider2D))
        {
            boxCollider2D.enabled = false;
            creature.GetCollider.enabled = true;
        }
    }

    public override void FixedState()
    {
        
    }

    public override void UpdateState()
    {
        
    }
}
