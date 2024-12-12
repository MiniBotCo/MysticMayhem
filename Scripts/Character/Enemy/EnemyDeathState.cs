using Godot;
using System;

public partial class EnemyDeathState : EnemyState
{
    protected async override void EnterState()
    {
        characterNode.AnimationPlayerNode.Play(GameConstants.ANIM_DEATH);
        await ToSignal(characterNode.AnimationPlayerNode, AnimationPlayer.SignalName.AnimationFinished);
        characterNode.EmitSignal(Enemy.SignalName.Death);
        characterNode.QueueFree();
    }

    protected override void ExitState()
    {
    }

}
