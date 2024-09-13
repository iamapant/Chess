using System;

public abstract class BaseState {
    public abstract void Enter();
    public abstract void Exit();
    public abstract void Update();

    public virtual void OnTrigger() {
        
    }
}