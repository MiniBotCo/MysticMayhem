using Godot;
using System;
using System.Linq;

public partial class EnemyChaseState : EnemyState
{
    private Node2D target = null;
    [Export] private Timer chaseTimerNode;
    protected override void EnterState()
    {
        //Should have a dedicated move animation for this
        characterNode.AnimationPlayerNode.Play(GameConstants.ANIM_IDLE);

        characterNode.ChaseAreaNode.BodyEntered += OnChaseAreaBodyEntered;

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
        if (target != null)
        {
            destination = target.GlobalPosition;
            characterNode.Agent2DNode.TargetPosition = destination;
        }
    }

    private void OnChaseAreaBodyEntered(Node2D body)
    {
        if(body.IsInGroup("Player"))
        {
            target = body;
        }
    }
}
