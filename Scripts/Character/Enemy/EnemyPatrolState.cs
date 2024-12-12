using Godot;
using System;

public partial class EnemyPatrolState : EnemyState
{
    [Export] private Timer idleTimerNode;
    [Export(PropertyHint.Range, "0,20,0.1")] private float maxIdleTime = 4;
    private int pointIndex = 0;
    protected override void EnterState()
    {
        characterNode.AnimationPlayerNode.Play(GameConstants.ANIM_IDLE);

        pointIndex = 1;

        destination = GetPointGlobalPosition(pointIndex);
        characterNode.Agent2DNode.TargetPosition = destination;

        characterNode.Agent2DNode.NavigationFinished += HandleNavigationFinished;
        idleTimerNode.Timeout += HandleTimeout;
        characterNode.ChaseAreaNode.BodyEntered += HandleChaseAreaBodyEntered;
    }

    protected override void ExitState()
    {
        characterNode.Agent2DNode.NavigationFinished -= HandleNavigationFinished;
        idleTimerNode.Timeout -= HandleTimeout;
        characterNode.ChaseAreaNode.BodyEntered -= HandleChaseAreaBodyEntered;
    }

    public override void _PhysicsProcess(double delta)
    {
        //GD.Print("In the Patrol state!");
        if (!idleTimerNode.IsStopped())
        {
            return;
        }
        Move();
    }

    private void HandleNavigationFinished()
    {
        // Enemy Idle Animation should be played here
        RandomNumberGenerator rng = new();
        idleTimerNode.WaitTime = rng.RandfRange(0, maxIdleTime);
        idleTimerNode.Start();
    }

    private void HandleTimeout()
    {
        // Enemy move Animation should be played here
        pointIndex = Mathf.Wrap(pointIndex + 1, 0, characterNode.PathNode.Curve.PointCount);

        destination = GetPointGlobalPosition(pointIndex);
        characterNode.Agent2DNode.TargetPosition = destination;
    }

}
