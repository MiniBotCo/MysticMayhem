using Godot;
using System;

public partial class PlayerIdleState : PlayerState
{
    public override void _PhysicsProcess(double delta)
    {
        characterNode.velocity = characterNode.Velocity;
        //Resetting the character movement to 0 for the idle state
        characterNode.velocity.X = 0;

        // Add the gravity.
        if (!characterNode.IsOnFloor())
        {
            characterNode.velocity += characterNode.GetGravity() * (float)delta;
        }

        characterNode.direction = Input.GetVector(GameConstants.INPUT_MOVE_LEFT, GameConstants.INPUT_MOVE_RIGHT, GameConstants.INPUT_JUMP, "ui_down");
        if (characterNode.direction != Vector2.Zero && characterNode.direction.Y == 0)
        {
            characterNode.StateMachineNode.SwitchState<PlayerMoveState>();
        }

        //Switch to the Jump State
        if (Input.IsActionJustPressed(GameConstants.INPUT_JUMP) && characterNode.IsOnFloor())
        {
            characterNode.AudioPlayer.Stream = characterNode.jumpSound;
            characterNode.AudioPlayer.Play();
            characterNode.velocity.Y = -characterNode.GetStatResource(Stat.JumpSpeed).StatValue;
            characterNode.StateMachineNode.SwitchState<PlayerJumpState>();
        }

        characterNode.Velocity = characterNode.velocity;

        characterNode.MoveAndSlide();
        characterNode.Flip();
    }

    public override void _Input(InputEvent @event)
    {
        CheckForAttackInput();
    }

    protected override void EnterState()
    {
        base.EnterState();
        characterNode.AnimationPlayerNode.Play(GameConstants.ANIM_IDLE);
    }
}
