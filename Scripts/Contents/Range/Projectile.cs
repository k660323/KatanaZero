using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
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

    private void Awake()
    {
        col = transform.GetComponentInChildren<Collider2D>();
    }

    public abstract void SetProjectile(Vector2 startPos, Vector2 dir, int enemyLayer, Creature owner);

    public abstract void DestroyProjectile();
}
