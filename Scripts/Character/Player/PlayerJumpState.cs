using Godot;
using System;

public partial class PlayerJumpState : Node
{
    private Player characterNode;
    public override void _Ready()
    {
        characterNode = GetOwner<Player>();
        SetPhysicsProcess(false);
    }

    public override void _PhysicsProcess(double delta)
    {
        GD.Print("Jump state enabled!");
        if (characterNode.Velocity.Y == 0)
        {
            characterNode.stateMachineNode.SwitchState<PlayerIdleState>();
        }

    }

    public override void _Notification(int what)
    {
        base._Notification(what);

        if (what == 5001)
        {
            GD.Print("SHould be player the jump animation right now.  WIll update the jump animation soon!");
            characterNode.animationPlayerNode.Play(GameConstants.ANIM_IDLE);
            SetPhysicsProcess(true);
        }
        else if (what == 5002)
        {
            SetPhysicsProcess(false);
        }
    }
}
