using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    protected int _enemyLayer;

    public int EnemyLayer { get { return _enemyLayer; } }

    public Creature _owner;

    protected Collider2D col;

    // 총알 방향
    protected Vector3 _dir;
    // 이동 속도    
    [SerializeField]
    protected float _moveSpeed;


    // 생명주기 코루틴
    protected IEnumerator _lifeTimeCor;
    // 생명 주기
    [SerializeField]
    protected float _lifeTime;

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

        if (_lifeTimeCor != null)
            StopCoroutine(_lifeTimeCor);
        _lifeTimeCor = LifeTimeCor(_lifeTime);
        StartCoroutine(_lifeTimeCor);
    }

    protected virtual void FixedUpdate()
    {
        transform.position += _dir * _moveSpeed * Time.fixedDeltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((1 << collision.gameObject.layer) == _enemyLayer)
        {
            // 오브젝트 공격
            if (collision.transform.TryGetComponent(out IHitable hitable))
            {
                hitable.OnHit(gameObject, true);
            }

            DestroyProjectile();
        }
        else if (1<< collision.gameObject.layer == (1 << 6))
        {
            DestroyProjectile();
        }
    }

    // 생명 주기가 끝나면 삭제
    protected IEnumerator LifeTimeCor(float time)
    {
        while (time > 0.0f)
        {
            time -= Time.deltaTime;
            yield return null;
        }

        DestroyProjectile();
    }

    public void DestroyProjectile()
    {
        if (gameObject.activeInHierarchy == false)
            return;

        if (_lifeTimeCor != null)
            StopCoroutine(_lifeTimeCor);
        _lifeTimeCor = null;
        
        col.enabled = false;
        Managers.Resource.Destory(gameObject);
    }

    public void ReflectDir(int layer)
    {
        if (gameObject.activeInHierarchy == false)
            return;

        SetProjectile(transform.position, -_dir, 1 << layer, _owner);
    }
}
