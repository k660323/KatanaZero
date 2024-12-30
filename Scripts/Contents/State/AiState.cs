using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AiState : BaseState
{
    protected AiState(Creature creature, Controller controller) : base(creature, controller)
    {
    }
}
