using Godot;
using System;

public partial class PlayerDeathState : PlayerState
{

	protected override async void EnterState()
	{
		characterNode.AnimationPlayerNode.Play(GameConstants.ANIM_DEATH);
		await ToSignal(characterNode.AnimationPlayerNode, AnimationPlayer.SignalName.AnimationFinished);
		characterNode.EmitSignal(Player.SignalName.Death);
		characterNode.QueueFree();
	}

	protected override void ExitState()
	{
		
	}
}
