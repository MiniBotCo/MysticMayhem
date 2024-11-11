using Godot;

public abstract partial class PlayerState : CharacterState
{
    protected Player characterNode;
    public override void _Ready()
    {
        base._Ready();
        characterNode = GetOwner<Player>();
    }
}