using Godot;

public abstract partial class CharacterState : Node
{
    public override void _Ready()
    {
        SetPhysicsProcess(false);
        SetProcessInput(false);
    }

    public override void _Notification(int what)
    {
        base._Notification(what);

        if (what == GameConstants.NOTIFICATION_ENTER_STATE)
        {
            EnterState();
            SetPhysicsProcess(true);
            SetProcessInput(true);
        }
        else if (what == GameConstants.NOTIFICATION_EXIT_STATE)
        {
            SetPhysicsProcess(false);
            SetProcessInput(false);
            ExitState();
        }
    }

    protected virtual void EnterState() { }
    protected virtual void ExitState() { }
}