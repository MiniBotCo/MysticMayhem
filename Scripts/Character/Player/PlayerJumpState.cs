using Godot;
using System;

public partial class PlayerJumpState : Node
{
    private Player characterNode;
    public override void _Ready()
    {
        characterNode = GetOwner<Player>();
    }

    public override void _PhysicsProcess(double delta)
    {
        if (characterNode.Velocity.Y == 0)
        {
            GD.Print("Switching to the idle state");
        }

    }

    public override void _Notification(int what)
    {
        base._Notification(what);

        if (what == 5001)
        {
            GD.Print("SHould be player the jump animation right now.  WIll update the jump animation soon!");
        }
    }
}
