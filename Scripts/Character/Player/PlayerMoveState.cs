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
            characterNode.velocity.X = characterNode.direction.X * characterNode.PlayerSpeed;
        }
        else
        {
            characterNode.velocity.X = Mathf.MoveToward(characterNode.Velocity.X, 0, characterNode.PlayerSpeed);
        }

        if (characterNode.direction == Vector2.Zero)
        {
            characterNode.StateMachineNode.SwitchState<PlayerIdleState>();
            return;
        }

        //Switch to the Jump State
        if (Input.IsActionJustPressed(GameConstants.INPUT_JUMP))
        {
            characterNode.StateMachineNode.SwitchState<PlayerJumpState>();
        }

        characterNode.Velocity = characterNode.velocity;

        characterNode.MoveAndSlide();
        characterNode.Flip();
    }

    protected override void EnterState()
    {
        base.EnterState();
        characterNode.AnimationPlayerNode.Play(GameConstants.ANIM_MOVE);
    }

}
