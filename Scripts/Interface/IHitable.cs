using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHitable
{
    public void OnHit(GameObject owner, bool isGround = true);

    public void OnHit(GameObject owner, float damage, bool isGround = true);
}
