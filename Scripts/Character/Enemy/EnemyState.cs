using Godot;

public abstract partial class EnemyState : CharacterState
{
    protected Enemy characterNode;
    protected Vector2 destination;
    public override void _Ready()
    {
        base._Ready();
        characterNode = GetOwner<Enemy>();
    }

    protected Vector2 GetPointGlobalPosition(int index)
    {
        Vector2 localPos = characterNode.PathNode.Curve.GetPointPosition(index);
        Vector2 globalPos = characterNode.PathNode.GlobalPosition;
        return localPos + globalPos;
    }

    protected void Move()
    {
        characterNode.Velocity = characterNode.GlobalPosition.DirectionTo(destination) * characterNode.EnemySpeed;

        characterNode.MoveAndSlide();
        characterNode.Flip();
    }
}