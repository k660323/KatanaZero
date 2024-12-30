using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiKissfaceHitState : AiHitState
{
    IEnumerator wakeUpCor;

    WaitForSeconds waitForSeconds = new WaitForSeconds(1.5f);

    public AiKissfaceHitState(Creature creature, Controller controller) : base(creature, controller)
    {

    }

    public override void EnterState()
    {
        creature.GetAnimator.SetTrigger("HitTrigger");
        creature.GetAnimator.SetBool("IsHit", true);
        Managers.Sound.Play2D("Sounds/chestground");
        if (creature is KissfaceEnemy kissfaceEnemy)
        {
            kissfaceEnemy.ShowInterectUI(true);
        }

    }

    public void WakeUp()
    {
        if (wakeUpCor != null)
            creature.StopCoroutine(wakeUpCor);
        wakeUpCor = WakeUpCor();
        creature.StartCoroutine(wakeUpCor);
    }

    public IEnumerator WakeUpCor()
    {
        yield return waitForSeconds;
        if (creature.GetStateMachine.ChangeState(Define.State.WakeUp))
            yield break;

        creature.GetStateMachine.ChangeState(Define.State.Idle);
    }

    public override void ExitState()
    {
        creature.GetAnimator.SetBool("IsHit", false);
        if(wakeUpCor != null)
            creature.StopCoroutine(wakeUpCor);
        wakeUpCor = null;

        if (creature is KissfaceEnemy kissfaceEnemy)
        {
            kissfaceEnemy.ShowInterectUI(false);
        }
    }

    public override void FixedState()
    {

    }

    public override void UpdateState()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            float distance = (controller.Target.transform.position - creature.transform.position).magnitude;
            if (distance > 3.0f)
                return;

            if (creature.GetStateMachine.ChangeState(Define.State.Resistance))
                return;
        }
    }
}
