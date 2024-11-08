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

    public override void _Notification(int what)
    {
        base._Notification(what);

        if (what == 5001)
        {
            GD.Print("Switched to the attack state");
            characterNode.animationPlayerNode.Play(GameConstants.ANIM_ATTACK);
            SetPhysicsProcess(true);
            attackTimerNode.Start();
        }
    }
    private void HandleAttackTimeout()
    {
        characterNode.stateMachineNode.SwitchState<PlayerIdleState>();
    }
}
