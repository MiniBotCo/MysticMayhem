using Godot;
using System;
using System.Reflection.Metadata;

public partial class PlayerAttackState : PlayerState
{
    [Export] private Timer attackTimerNode;

    public override void _Ready()
    {
        base._Ready();
        attackTimerNode.Timeout += HandleAttackTimeout;
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
            //GD.Print("Switched to the attack state");

            if (Input.IsActionPressed(GameConstants.INPUT_MOVE_LEFT))
            {
                characterNode.Sprite2DNode.FlipH = true;
            }
            else if (Input.IsActionPressed(GameConstants.INPUT_MOVE_RIGHT))
            {
                characterNode.Sprite2DNode.FlipH = false;
            }

            characterNode.AudioPlayer.Stream = characterNode.swordSwingSound;
            characterNode.AudioPlayer.Play();
            characterNode.AnimationPlayerNode.Play(GameConstants.ANIM_ATTACK);
            SetPhysicsProcess(true);
            attackTimerNode.Start();
        }
    }
    private void HandleAttackTimeout()
    {
        characterNode.StateMachineNode.SwitchState<PlayerIdleState>();
    }
}
