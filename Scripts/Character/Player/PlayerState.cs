using Godot;

public abstract partial class PlayerState : CharacterState
{
    protected Player characterNode;
    public override void _Ready()
    {
        base._Ready();
        characterNode = GetOwner<Player>();
    }

    protected void CheckForAttackInput()
    {
        if (Input.IsActionJustPressed(GameConstants.INPUT_ATTACK))
        {
            characterNode.StateMachineNode.SwitchState<PlayerAttackState>();
        }
    }
}