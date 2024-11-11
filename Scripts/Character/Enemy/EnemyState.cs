using Godot;

public abstract partial class EnemyState : CharacterState
{
    protected Enemy characterNode;
    public override void _Ready()
    {
        base._Ready();
        characterNode = GetOwner<Enemy>();
    }
}