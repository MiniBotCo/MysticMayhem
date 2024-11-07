using Godot;
using System;

public partial class PlayerMoveState : Node
{
    private Player characterNode;
    public override void _Ready()
    {
        characterNode = GetOwner<Player>();
        SetPhysicsProcess(false);
        SetProcessInput(false);
    }

    public override void _PhysicsProcess(double delta)
    {
        GD.Print("MOVE STATE");

        characterNode.velocity = characterNode.Velocity;

        // Get the input direction and handle the movement/deceleration.
        characterNode.direction = Input.GetVector(GameConstants.INPUT_MOVE_LEFT, GameConstants.INPUT_MOVE_RIGHT, GameConstants.INPUT_JUMP, "ui_down");
        if (characterNode.direction != Vector2.Zero)
        {
            characterNode.velocity.X = characterNode.direction.X * characterNode.Speed;
        }
        else
        {
            characterNode.velocity.X = Mathf.MoveToward(characterNode.Velocity.X, 0, characterNode.Speed);
        }


        if (characterNode.direction == Vector2.Zero)
        {
            characterNode.stateMachineNode.SwitchState<PlayerIdleState>();
            return;
        }

        //Switch to the Jump State
        if (Input.IsActionJustPressed(GameConstants.INPUT_JUMP))
        {
            characterNode.stateMachineNode.SwitchState<PlayerJumpState>();
        }

        characterNode.Velocity = characterNode.velocity;

        characterNode.MoveAndSlide();
        characterNode.Flip();

    }

    public override void _Notification(int what)
    {
        base._Notification(what);

        if (what == 5001)
        {
            characterNode.animationPlayerNode.Play(GameConstants.ANIM_MOVE);
            SetPhysicsProcess(true);
            SetProcessInput(true);
        }
        else if (what == 5002)
        {
            SetPhysicsProcess(false);
            SetProcessInput(false);
        }
    }
}
