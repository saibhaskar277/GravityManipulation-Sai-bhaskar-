using UnityEngine;

public class IdleState : IPlayerState
{
    private PlayerContext ctx;

    public IdleState(PlayerContext context) => ctx = context;

    public void Enter()
    {
        ctx.Animator.SetBool("IsGrounded", true);
        ctx.Animator.SetBool("IsWalking", false);
    }

    public void Tick()
    {
        if (!ctx.Motor.IsGrounded)
        {
            ctx.StateMachine.ChangeState(new FallState(ctx));
            return;
        }

        if (ctx.Input.JumpPressed)
        {
            ctx.Motor.Jump();
            ctx.StateMachine.ChangeState(new FallState(ctx));
            return;
        }

        if (ctx.Input.Move.sqrMagnitude > 0.01f)
        {
            ctx.StateMachine.ChangeState(new RunState(ctx));
        }
    }

    public void Exit() { }
}