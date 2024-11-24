using Godot;
using System;
using System.Linq;

public partial class EnemyAttackState : EnemyState
{
    private Vector2 targetPosition;
    protected override void EnterState()
    {
        characterNode.AnimationPlayerNode.Play(GameConstants.ANIM_ATTACK);
        characterNode.AnimationPlayerNode.AnimationFinished += HandleAnimationFinished;
    }

    protected override void ExitState()
    {
        characterNode.AnimationPlayerNode.AnimationFinished -= HandleAnimationFinished;
    }

    private void PerformHit()
    {
        characterNode.ToggleHitBox(false);
        float distanceHitBoxFacingLeft = 36.0f;
        float distanceHitBoxFacingRight = 36.0f;
        Vector2 newPosition = characterNode.Sprite2DNode.FlipH ? Vector2.Left : Vector2.Right;
        float distanceMultiplier = characterNode.Sprite2DNode.FlipH ? distanceHitBoxFacingLeft : distanceHitBoxFacingRight;
        newPosition *= distanceMultiplier;
        characterNode.HitboxNode.Position = newPosition;
    }

    private void HandleAnimationFinished(StringName animName)
    {
        characterNode.ToggleHitBox(true);
        Node2D target = characterNode.AttackAreaNode.GetOverlappingBodies().FirstOrDefault();
        if (target == null)
        {
            characterNode.StateMachineNode.SwitchState<EnemyPatrolState>();
            return;
        }
        characterNode.AnimationPlayerNode.Play(GameConstants.ANIM_ATTACK);
    }

}