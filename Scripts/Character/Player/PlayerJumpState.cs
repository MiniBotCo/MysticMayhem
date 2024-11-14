using Godot;
using System;

public partial class PlayerJumpState : PlayerState
{
    private int jumpCount = 0;
    private int maxJumps = 2;

    public override void _PhysicsProcess(double delta)
    {
        characterNode.velocity = characterNode.Velocity;

        // Add the gravity.
        if (!characterNode.IsOnFloor())
        {
            characterNode.AnimationPlayerNode.Play(GameConstants.ANIM_JUMP);
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
            characterNode.StateMachineNode.SwitchState<PlayerIdleState>();
        }

        characterNode.Velocity = characterNode.velocity;

        characterNode.MoveAndSlide();
        characterNode.Flip();

    }

    protected override void EnterState()
    {
        base.EnterState();
        GD.Print("SHould be player the jump animation right now.  WIll update the jump animation soon!");
        characterNode.AnimationPlayerNode.Play(GameConstants.ANIM_JUMP);
    }
}
