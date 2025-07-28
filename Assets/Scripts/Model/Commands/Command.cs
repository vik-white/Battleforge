using System;

public abstract class Command
{
    public Action OnCompleted;
    
    public virtual void  Execute(IBattle battle, Character character) {}
}