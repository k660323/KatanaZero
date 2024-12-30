using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat : MonoBehaviour
{
    [SerializeField]
    float moveSpeed;
    public float MoveSpeed { get { return moveSpeed; } set { moveSpeed = value; } }

    [SerializeField]
    protected float _detectRange;
    public float DetectRange { get { return _detectRange; } set { _detectRange = value; } }

    [SerializeField]
    protected float _patrolDistance;
    public float PatrolDistance { get { return _patrolDistance; } }

    [SerializeField]
    protected float _minChaseDistance;
    public float MinChaseDistance { get { return _minChaseDistance; } }

    [SerializeField]
    protected float _atkRange;
    public float AtkRange { get { return _atkRange; } }

    [SerializeField]
    protected int _enemyLayer;
    public int EnemyLayer { get { return _enemyLayer; } }
}