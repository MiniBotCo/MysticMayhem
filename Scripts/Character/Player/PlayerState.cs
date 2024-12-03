using System;
using Godot;

public abstract partial class PlayerState : CharacterState
{
    protected Player characterNode;
    public async override void _Ready()
    {
        base._Ready();

        characterNode = GetOwner<Player>();

        // Waits for the player to be ready before connecting to the health stat
        await ToSignal(characterNode, Node.SignalName.Ready);
        characterNode.GetStatResource(Stat.Health).OnZero += HandleZeroHealth;
    }

    protected void CheckForAttackInput()
    {
        if (Input.IsActionJustPressed(GameConstants.INPUT_ATTACK))
        {
            characterNode.StateMachineNode.SwitchState<PlayerAttackState>();
        }
    }

    private void HandleZeroHealth()
    {
        characterNode.StateMachineNode.SwitchState<PlayerDeathState>();
    }
}