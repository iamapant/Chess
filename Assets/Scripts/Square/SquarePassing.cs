using System;

[Serializable]
public abstract class SquarePassing {
    public virtual bool CanEnter(Entity entity) => true;
    public abstract void OnEnter(Entity entity);
    public abstract void OnExit(Entity entity);
}