using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarLoad : MonoBehaviour
{
    protected virtual void Awake()
    {
        Managers.Map.LoadMap(gameObject.scene.name);
    }
}
