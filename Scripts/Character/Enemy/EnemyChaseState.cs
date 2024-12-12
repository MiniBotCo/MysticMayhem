using Godot;
using System;
using System.Linq;

public partial class EnemyChaseState : EnemyState
{
    private CharacterBody2D target;
    [Export] private Timer chaseTimerNode;
    protected override void EnterState()
    {
        characterNode.AnimationPlayerNode.Play(GameConstants.ANIM_IDLE);
        target = characterNode.ChaseAreaNode.GetOverlappingBodies().First() as CharacterBody2D;

        chaseTimerNode.Timeout += HandleTimeout;
        characterNode.AttackAreaNode.BodyEntered += HandleAttackAreaBodyEntered;
        characterNode.ChaseAreaNode.BodyExited += HandleChaseAreaBodyExited;
    }

    protected override void ExitState()
    {
        chaseTimerNode.Timeout -= HandleTimeout;
        characterNode.AttackAreaNode.BodyEntered -= HandleAttackAreaBodyEntered;
        characterNode.ChaseAreaNode.BodyExited -= HandleChaseAreaBodyExited;
    }

    public override void _PhysicsProcess(double delta)
    {
        destination = target.GlobalPosition;
        characterNode.Agent2DNode.TargetPosition = destination;
        Move();
    }

    private void HandleTimeout()
    {
        destination = target.GlobalPosition;
        characterNode.Agent2DNode.TargetPosition = destination;
    }

    private void HandleAttackAreaBodyEntered(Node2D body)
    {
        characterNode.StateMachineNode.SwitchState<EnemyAttackState>();
    }

    private void HandleChaseAreaBodyExited(Node2D body)
    {
        characterNode.StateMachineNode.SwitchState<EnemyReturnState>();
    }
}
