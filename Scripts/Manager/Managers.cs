using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : Singleton<Managers>
{
    static MapManager map = new MapManager();
    static ResourceManager resource = new ResourceManager();
    static SoundManager sound = new SoundManager();

    public static MapManager Map { get { return map; } }
    public static ResourceManager Resource { get { return resource; } }
    public static SoundManager Sound { get { return sound; } }


    protected override void Awake()
    {
        base.Awake();
    }

    public void DisableAllObject()
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            GameObject go = gameObject.transform.GetChild(i).gameObject;
            if (go.activeInHierarchy)
            {
                if (go.TryGetComponent(out Bullet bullet))
                {
                    bullet.DestroyProjectile();
                }
                else if(go.TryGetComponent(out Axe axe))
                {
                    axe.DestroyProjectile();
                }
                else if(go.TryGetComponent(out JumpSwingAxe jumpAxe))
                {
                    jumpAxe.DestroyProjectile();
                }
                else if(go.TryGetComponent(out Effect effect))
                {
                    effect.OnEffectFinished();
                }
                else
                {
                    resource.Destory(gameObject.transform.GetChild(i).gameObject);
                }
            }
        }
    }
}
