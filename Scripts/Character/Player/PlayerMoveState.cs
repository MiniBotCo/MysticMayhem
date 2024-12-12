using Godot;
using System;

public partial class PlayerMoveState : PlayerState
{
    public override void _PhysicsProcess(double delta)
    {
        characterNode.velocity = characterNode.Velocity;

        // Add the gravity.
        if (!characterNode.IsOnFloor())
        {
            characterNode.velocity += characterNode.GetGravity() * (float)delta;
        }

        // Get the input direction and handle the movement/deceleration.
        characterNode.direction = Input.GetVector(GameConstants.INPUT_MOVE_LEFT, GameConstants.INPUT_MOVE_RIGHT, GameConstants.INPUT_JUMP, "ui_down");
        if (characterNode.direction != Vector2.Zero)
        {
            characterNode.velocity.X = characterNode.direction.X * characterNode.GetStatResource(Stat.Speed).StatValue;
        }
        else
        {
            characterNode.velocity.X = Mathf.MoveToward(characterNode.Velocity.X, 0, characterNode.GetStatResource(Stat.Speed).StatValue);
        }

        if (characterNode.direction == Vector2.Zero)
        {

            characterNode.StateMachineNode.SwitchState<PlayerIdleState>();
            return;
        }

        //Apply jump speed and switch to the Jump State
        if (Input.IsActionJustPressed(GameConstants.INPUT_JUMP))
        {
            characterNode.AudioPlayer.Stream = characterNode.jumpSound;
            characterNode.AudioPlayer.Play();
            characterNode.velocity.Y = -characterNode.GetStatResource(Stat.JumpSpeed).StatValue;
        }

        if (!characterNode.IsOnFloor())
        {
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
        characterNode.AnimationPlayerNode.Play(GameConstants.ANIM_MOVE);
    }

}
