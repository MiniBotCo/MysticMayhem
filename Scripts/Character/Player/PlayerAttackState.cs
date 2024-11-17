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
            GD.Print("Switched to the attack state");
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
