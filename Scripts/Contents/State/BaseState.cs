
public abstract class BaseState
{
    protected Creature creature;
    protected Controller controller;

    public BaseState(Creature creature, Controller controller)
    {
        this.creature = creature;
        this.controller = controller;
    }

    public virtual bool CheckCondition() { return true; }

    public abstract void EnterState();

    public abstract void ExitState();

    public abstract void FixedState();

    public abstract void UpdateState();
}
