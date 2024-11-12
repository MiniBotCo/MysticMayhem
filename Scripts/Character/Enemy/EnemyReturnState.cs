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
        GD.Print("Should be playing an enemy move animation!");
        characterNode.AnimationPlayerNode.Play(GameConstants.ANIM_IDLE);


        characterNode.GlobalPosition = destination;
    }
}
