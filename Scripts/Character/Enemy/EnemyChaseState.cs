using Godot;
using System;
using System.Linq;

public partial class EnemyChaseState : EnemyState
{
    private CharacterBody2D target;
    [Export] private Timer chaseTimerNode;
    protected override void EnterState()
    {
        //Should have a dedicated move animation for this
        characterNode.AnimationPlayerNode.Play(GameConstants.ANIM_IDLE);
        target = characterNode.ChaseAreaNode.GetOverlappingBodies().First() as CharacterBody2D;

        chaseTimerNode.Timeout += HandleTimeout;
    }

    protected override void ExitState()
    {
        chaseTimerNode.Timeout -= HandleTimeout;
    }

    public override void _PhysicsProcess(double delta)
    {
        Move();
    }

    private void HandleTimeout()
    {
        destination = target.GlobalPosition;
        characterNode.Agent2DNode.TargetPosition = destination;
    }
}
