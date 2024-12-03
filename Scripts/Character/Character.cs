using Godot;
using System;
using System.Linq;

public partial class Character : CharacterBody2D
{
    [Export] protected StatResource[] stats;
    protected Character characterNode;
    
    // Define gravity and other variables
    [Export] public float Gravity = 900.0f;
    [Export] public float JumpForce = -400.0f;
    [Export] public float MaxFallSpeed = 1500.0f;

    [ExportGroup("Required Nodes")]
    [Export] public AnimationPlayer AnimationPlayerNode { get; private set; }
    [Export] public Sprite2D Sprite2DNode { get; private set; }
    [Export] public StateMachine StateMachineNode { get; private set; }
    [Export] public AudioStreamPlayer AudioPlayer { get; private set; }
    [Export] public Area2D HurtboxNode { get; private set; }
    [Export] public Area2D HitboxNode { get; private set; }
    [Export] public CollisionShape2D HitboxShapeNode { get; private set; }


    [ExportGroup("AI Nodes")]
    [Export] public Path2D PathNode { get; private set; }
    [Export] public NavigationAgent2D Agent2DNode { get; private set; }
    [Export] public Area2D ChaseAreaNode { get; private set; }
    [Export] public Area2D AttackAreaNode { get; private set; }

    public Vector2 velocity = Vector2.Zero;

    public override void _Ready()
    {
        HurtboxNode.AreaEntered += HandleHurtboxEntered;
    }

    public void Flip()
    {
        bool isNotMovingHorizontally = Velocity.X == 0;

        if (isNotMovingHorizontally) { return; }

        bool isMovingLeft = Velocity.X < 0;
        Sprite2DNode.FlipH = isMovingLeft;
    }

    protected virtual void HandleHurtboxEntered(Area2D area)
    {
        StatResource health = GetStatResource(Stat.Health);
        Character attacker = area.GetOwner<Character>();
        health.StatValue -= attacker.GetStatResource(Stat.Damage).StatValue;
    }

    public StatResource GetStatResource(Stat stat)
    {
        return stats.Where((element) => element.StatType == stat).FirstOrDefault();
    }

    public void ToggleHitBox(bool flag)
    {
        HitboxShapeNode.Disabled = flag;
    }
}
