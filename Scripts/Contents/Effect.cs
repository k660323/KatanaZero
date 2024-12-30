using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{  
    public void InitEffect(Vector3 pos, Vector2 dir, GameObject parent = null)
    {
        transform.position = pos;
        Vector3 scale = transform.localScale;
        if(dir == Vector2.right)
        {
            transform.localScale = new Vector3(Mathf.Abs(scale.x), scale.y, scale.z);
        }
        else
        {
            transform.localScale = new Vector3(-Mathf.Abs(scale.x), scale.y, scale.z);
        }

        if (parent != null)
            gameObject.transform.SetParent(parent.transform);
    }


    public void OnEffectFinished()
    {
        Managers.Resource.Destory(gameObject);
    }
}
