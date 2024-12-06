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

        // Handle Jump.
        if (characterNode.IsOnFloor())
        {
            characterNode.AudioPlayer.Stream = characterNode.landSound;
            characterNode.AudioPlayer.Play();
            characterNode.StateMachineNode.SwitchState<PlayerIdleState>();
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
        characterNode.AnimationPlayerNode.Play(GameConstants.ANIM_JUMP);
    }
}
