using Godot;
using System;

public partial class EnemyChaseState : EnemyState
{
    protected override void EnterState()
    {
        //Should have a dedicated move animation for this
        characterNode.AnimationPlayerNode.Play(GameConstants.ANIM_IDLE);
        GD.Print("Entered chase state!");
    }
}
