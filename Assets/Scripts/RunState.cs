using UnityEngine;

public class RunState : IPlayerState
{
    private PlayerContext ctx;

    public RunState(PlayerContext context) => ctx = context;

    public void Enter()
    {
        ctx.Animator.SetBool("IsGrounded", true);
        ctx.Animator.SetBool("IsWalking", true);

    }

    public void Tick()
    {
        ctx.Motor.Move(ctx.Input.Move);

        if (!ctx.Motor.IsGrounded)
        {

            ctx.StateMachine.ChangeState(new FallState(ctx));
        }

        if (ctx.Input.JumpPressed)
        {
            ctx.Motor.Jump();
            ctx.StateMachine.ChangeState(new FallState(ctx));
            return;
        }

        if (ctx.Input.Move.sqrMagnitude < 0.01f)
        {
            ctx.StateMachine.ChangeState(new IdleState(ctx));
        }
    }

    public void Exit() { }
}