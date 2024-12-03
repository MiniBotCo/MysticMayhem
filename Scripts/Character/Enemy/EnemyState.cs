using System;
using Godot;

public abstract partial class EnemyState : CharacterState
{
    protected Enemy characterNode;
    protected Vector2 destination;
    public async override void _Ready()
    {
        base._Ready();
        characterNode = GetOwner<Enemy>();

        // Waits for the enemy to be ready before connecting to the health stat
        await ToSignal(characterNode, Node.SignalName.Ready);
        characterNode.GetStatResource(Stat.Health).OnZero += HandleZeroHealth;
    }

    protected Vector2 GetPointGlobalPosition(int index)
    {
        return characterNode.ToGlobal(characterNode.PathNode.Curve.GetPointPosition(index));
    }

    protected void Move()
    {
        characterNode.Agent2DNode.GetNextPathPosition();
        characterNode.Velocity = characterNode.GlobalPosition.DirectionTo(destination) * characterNode.EnemySpeed;

        characterNode.MoveAndSlide();
        characterNode.Flip();
    }

    protected void HandleChaseAreaBodyEntered(Node2D body)
    {
        characterNode.StateMachineNode.SwitchState<EnemyChaseState>();
    }

    private void HandleZeroHealth()
    {
        characterNode.StateMachineNode.SwitchState<EnemyDeathState>();
    }
}