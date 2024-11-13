using Godot;
using System;

public partial class EnemyReturnState : EnemyState
{
    private Vector2 destination;

    public override void _Ready()
    {
        base._Ready();
        Vector2 localPos = characterNode.PathNode.Curve.GetPointPosition(0);
        Vector2 globalPos = characterNode.PathNode.GlobalPosition;
        destination = localPos + globalPos;
    }
    protected override void EnterState()
    {
        characterNode.AnimationPlayerNode.Play(GameConstants.ANIM_IDLE);

        characterNode.Agent2DNode.TargetPosition = destination;
        GD.Print("The desitination is: " + destination);

    }

    public override void _PhysicsProcess(double delta)
    {
        if (characterNode.Agent2DNode.IsNavigationFinished())
        {
            GD.Print("Reached destination!");
            return;
        }

        GD.Print("Distance to target: " + characterNode.Agent2DNode.DistanceToTarget());
        GD.Print("CharacterNode GlobalPosition: " + characterNode.GlobalPosition);
        GD.Print("Destination is: " + destination);

        characterNode.Velocity = characterNode.GlobalPosition.DirectionTo(destination) * characterNode.EnemySpeed;

        characterNode.MoveAndSlide();
        characterNode.Flip();
    }
}
