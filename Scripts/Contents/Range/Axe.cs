using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : MonoBehaviour
{
    protected int _enemyLayer;

    public int EnemyLayer { get { return _enemyLayer; } }

    public Creature _owner;

    protected Collider2D col;

    // �Ѿ� ����
    protected Vector3 _dir;
    // �̵� �ӵ�    
    [SerializeField]
    protected float _moveSpeed;

    protected Vector2 cachedStartPos;

    // ���� �ֱ�
    [SerializeField]
    protected float _moveTime;
    // 
    protected IEnumerator _throwWeaponCor;

    private void Awake()
    {
        TryGetComponent(out col);
    }

    public void SetProjectile(Vector2 startPos, Vector2 dir, int enemyLayer, Creature owner)
    {
        transform.position = startPos;
        _dir = dir;
        _enemyLayer = enemyLayer;
        _owner = owner;
        col.enabled = true;

        cachedStartPos = startPos;
        transform.rotation = Quaternion.identity;
        if (_throwWeaponCor != null)
            StopCoroutine(_throwWeaponCor);
        _throwWeaponCor = ThrowWeaponCor(_moveTime);
        StartCoroutine(_throwWeaponCor);
    }

    protected virtual void FixedUpdate()
    {
        transform.rotation = transform.rotation * Quaternion.Euler(0.0f, 0.0f, 30.0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((1 << collision.gameObject.layer) == _enemyLayer)
        {
            // ������Ʈ ����
            if (collision.transform.TryGetComponent(out IHitable hitable))
            {
                hitable.OnHit(gameObject, true);
            }
        }
    }

    // ���� �ֱⰡ ������ ����
    protected IEnumerator ThrowWeaponCor(float time)
    {
        WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();
        float _time = time;
        while (_time > 0.0f)
        {
            _time -= Time.fixedDeltaTime;
            transform.position += _moveSpeed * Time.fixedDeltaTime * _dir;
            yield return waitForFixedUpdate;
        }

        _time = time;
        while (_time > 0.0f)
        {
            _time -= Time.fixedDeltaTime;
            transform.position += _moveSpeed * Time.fixedDeltaTime * -_dir;
            yield return waitForFixedUpdate;
        }

        _owner.GetAnimator.SetTrigger("CatchWeaponTrigger");

        DestroyProjectile();
    }

    public void DestroyProjectile()
    {
        if (gameObject.activeInHierarchy == false)
            return;

        if (_throwWeaponCor != null)
            StopCoroutine(_throwWeaponCor);
        _throwWeaponCor = null;

        col.enabled = false;
        Managers.Resource.Destory(gameObject);
    }

  
}
