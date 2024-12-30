using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpSwingAxe : MonoBehaviour
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

    float rotateZ;

    private void Awake()
    {
        col = transform.GetComponentInChildren<Collider2D>();
    }

    public void SetProjectile(Vector2 startPos, Vector2 dir, int enemyLayer, Creature owner)
    {
        transform.position = startPos;
        transform.rotation = Quaternion.identity;
        _enemyLayer = enemyLayer;
        rotateZ = 0.0f;
        col.enabled = true;
    }

    protected virtual void FixedUpdate()
    {
        float nextZ = _moveSpeed * Time.deltaTime;
        transform.rotation = transform.rotation * Quaternion.Euler(0.0f, 0.0f, nextZ);
        rotateZ += nextZ;
        if (rotateZ >= 360.0f)
            DestroyProjectile();
    }

    public void DestroyProjectile()
    {
        if (gameObject.activeInHierarchy == false)
            return;

        col.enabled = false;
        Managers.Resource.Destory(gameObject);
    }
}
