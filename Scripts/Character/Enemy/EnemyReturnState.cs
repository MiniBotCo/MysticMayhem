using Godot;
using System;

public partial class EnemyReturnState : EnemyState
{
    public override void _Ready()
    {
        base._Ready();
    }
    protected override void EnterState()
    {
        destination = GetPointGlobalPosition(GD.RandRange(0, 1));
        characterNode.AnimationPlayerNode.Play(GameConstants.ANIM_IDLE);

        characterNode.Agent2DNode.TargetPosition = destination;

        characterNode.ChaseAreaNode.BodyEntered += HandleChaseAreaBodyEntered;
    }

    protected override void ExitState()
    {
        characterNode.ChaseAreaNode.BodyEntered -= HandleChaseAreaBodyEntered;
    }

    public override void _PhysicsProcess(double delta)
    {

        if (characterNode.Agent2DNode.IsNavigationFinished())
        {
            characterNode.StateMachineNode.SwitchState<EnemyPatrolState>();
            return;
        }

        Move();
    }
}
