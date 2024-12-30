using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum WeaponType
    {
        Melee,
        Range
    }

    public WeaponType weaponType;

    protected Creature creature;
    protected Controller controller;

    public bool _isAttack = false;
    [HideInInspector]
    public bool _isCoolTime = false;
    public float _coolTime = 0.0f;
    public float _curTime = 0.0f;

    // 근접 변수
    [SerializeField]
    public string swingEffect;

    public Vector2 _attackSize = Vector2.one;

    // 원거리 변수
    [SerializeField]
    public string projectileName;

    [SerializeField]
    public string fireEffect;

    [SerializeField]
    public string smokeEffect;

    [SerializeField]
    public string atkSound;

    [SerializeField]
    public string[] reloadSound;

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

    public void Attack()
    {
        if(weaponType == WeaponType.Melee)
        {
            if (_isAttack)
            {
                RaycastHit2D hit = Physics2D.BoxCast(creature.AtkPos.position, _attackSize, 0.0f, controller.LookDir, 0.0f, creature.GetStat.EnemyLayer);
                if (hit == false)
                    return;

                // 오브젝트 공격
                if (hit.collider)
                {
                    if (hit.transform.TryGetComponent(out IHitable hitable))
                    {
                        hitable.OnHit(gameObject, true);
                    }
                }
            }
        }
        else if(weaponType == WeaponType.Range)
        {
            if (_isAttack)
            {
                // 오브젝트 발사
                GameObject projectileObj = Managers.Resource.Instantiate($"Prefabs/Projectile/{projectileName}");
                if (projectileObj.TryGetComponent(out Bullet bullet))
                {
                    bullet.SetProjectile(creature.AtkPos.position, controller.LookDir, creature.GetStat.EnemyLayer, creature);
                }
            }
        }
    }

    public void FixedUpdate()
    {
        if (weaponType == WeaponType.Melee)
            Attack();
    }

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

    public void PlayReloadSound()
    {
        if(reloadSound.Length > 0)
        {
            Managers.Sound.Play2D($"Sounds/{reloadSound[Random.Range(0, reloadSound.Length)]}");
        }
    }

    public void PlayEffect()
    {
        if(weaponType == WeaponType.Melee)
        {
            if (swingEffect == "")
                return;

            GameObject effectObj = Managers.Resource.Instantiate($"Prefabs/Effect/{swingEffect}");
            if (effectObj != null && effectObj.TryGetComponent(out Effect efftect))
                efftect.InitEffect(creature.transform.position, controller.LookDir);
        }
        else if (weaponType == WeaponType.Range)
        {
            GameObject effectObj;
            Effect effect;
            effectObj = Managers.Resource.Instantiate($"Prefabs/Effect/{smokeEffect}");
            if (effectObj != null && effectObj.TryGetComponent(out effect))
                effect.InitEffect(creature.AtkPos.position, controller.LookDir);

            effectObj = Managers.Resource.Instantiate($"Prefabs/Effect/{fireEffect}");
            if (effectObj != null && effectObj.TryGetComponent(out effect))
                effect.InitEffect(creature.AtkPos.position, controller.LookDir);
        }
    }
}
