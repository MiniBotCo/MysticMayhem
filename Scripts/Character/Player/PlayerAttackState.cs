using Godot;
using System;
using System.Reflection.Metadata;

public partial class PlayerAttackState : PlayerState
{
    protected override void EnterState()
    {
        characterNode.AudioPlayer.Stream = characterNode.swordSwingSound;
        characterNode.AudioPlayer.Play();
        characterNode.AnimationPlayerNode.Play(GameConstants.ANIM_ATTACK);
        characterNode.AnimationPlayerNode.AnimationFinished += HandleAnimationFinished;
    }

    protected override void ExitState()
    {
        characterNode.AnimationPlayerNode.AnimationFinished -= HandleAnimationFinished;
    }

    public override void _Ready()
    {
        base._Ready();
    }

    public override void _PhysicsProcess(double delta)
    {
        characterNode.velocity = characterNode.Velocity;
        characterNode.velocity.X = 0;

        // Add the gravity.
        if (!characterNode.IsOnFloor())
        {
            characterNode.velocity += characterNode.GetGravity() * (float)delta;
        }

        characterNode.Velocity = characterNode.velocity;

        characterNode.MoveAndSlide();
        characterNode.Flip();
    }

    public override void _Notification(int what)
    {
        base._Notification(what);

        if (what == GameConstants.NOTIFICATION_ENTER_STATE)
        {
            if (Input.IsActionPressed(GameConstants.INPUT_MOVE_LEFT))
            {
                characterNode.Sprite2DNode.FlipH = true;
            }
            else if (Input.IsActionPressed(GameConstants.INPUT_MOVE_RIGHT))
            {
                characterNode.Sprite2DNode.FlipH = false;
            }
            SetPhysicsProcess(true);
        }
    }

    private void HandleAnimationFinished(StringName animName)
    {
        characterNode.ToggleHitBox(true);
        characterNode.StateMachineNode.SwitchState<PlayerIdleState>();
    }

    private void PerformHit()
    {
        float distanceHitBoxFacingLeft = 16.0f;
        float distanceHitBoxFacingRight = 44.0f;
        Vector2 newPosition = characterNode.Sprite2DNode.FlipH ? Vector2.Left : Vector2.Right;
        float distanceMultiplier = characterNode.Sprite2DNode.FlipH ? distanceHitBoxFacingLeft : distanceHitBoxFacingRight;
        newPosition *= distanceMultiplier;
        characterNode.HitboxNode.Position = newPosition;

        characterNode.ToggleHitBox(false);
    }
}
