using Godot;
using System;

public partial class EnemyPatrolState : EnemyState
{
    protected override void EnterState()
    {
        characterNode.AnimationPlayerNode.Play(GameConstants.ANIM_IDLE);

        destination = GetPointGlobalPosition(1);
        characterNode.Agent2DNode.TargetPosition = destination;
    }

    public override void _PhysicsProcess(double delta)
    {
        Move();
    }
}
