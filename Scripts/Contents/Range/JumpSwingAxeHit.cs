using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpSwingAxeHit : MonoBehaviour
{
    JumpSwingAxe jumpSwingAxe;

    private void Awake()
    {
        jumpSwingAxe = GetComponentInParent<JumpSwingAxe>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((1 << collision.gameObject.layer) == jumpSwingAxe.EnemyLayer)
        {
            // 오브젝트 공격
            if (collision.transform.TryGetComponent(out IHitable hitable))
            {
                hitable.OnHit(gameObject, true);
            }
        }
    }
}
