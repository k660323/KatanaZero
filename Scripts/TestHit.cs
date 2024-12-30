using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestHit : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Monster"))
        {
            IHitable hitable = (collision.gameObject.GetComponent<Creature>() as IHitable);
            if (hitable != null)
            {
                hitable.OnHit(gameObject, true);
            }
           
        }
    }
}
