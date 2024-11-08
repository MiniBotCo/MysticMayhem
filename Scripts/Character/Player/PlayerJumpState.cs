using Godot;
using System;

public partial class PlayerJumpState : Node
{
    private Player characterNode;
    private int jumpCount = 0;
    private int maxJumps = 2;
    public override void _Ready()
    {
        characterNode = GetOwner<Player>();
        SetPhysicsProcess(false);
    }

    public override void _PhysicsProcess(double delta)
    {
        characterNode.velocity = characterNode.Velocity;

        // Add the gravity.
        if (!characterNode.IsOnFloor())
        {
            characterNode.velocity += characterNode.GetGravity() * (float)delta;
        }

        // Handle Jump.
        if (characterNode.IsOnFloor())
        {
            characterNode.velocity.Y = characterNode.JumpVelocity;
        }

        //Transition back to the idle state when the player is not jumping
        if (characterNode.Velocity.Y == 0)
        {
            characterNode.stateMachineNode.SwitchState<PlayerIdleState>();
        }

        characterNode.Velocity = characterNode.velocity;

        characterNode.MoveAndSlide();
        characterNode.Flip();

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
