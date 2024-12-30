using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DeadState : AiState
{
    protected DeadState(Creature creature, Controller controller) : base(creature, controller)
    {
    }
}
