using UnityEngine;

public class FallState : IPlayerState
{
    private PlayerContext ctx;

    public FallState(PlayerContext context) => ctx = context;
    float fallTimer = 0f;

    public void Enter()
    {
        fallTimer = 0f;
        ctx.Animator.SetBool("IsGrounded", false);
        ctx.Animator.ResetTrigger("IsFalling");
        ctx.Animator.SetTrigger("IsFalling");
    }

    public void Tick()
    {
        ctx.Motor.Move(ctx.Input.Move);


        fallTimer += Time.deltaTime;

        if (fallTimer >= 3f)
        {
            EventManager.RaiseEvent(new OnPlayerFell());
            fallTimer = 0f; 
        }

        if (ctx.Motor.IsGrounded)
        {
            if (ctx.Input.Move.sqrMagnitude > 0.01f)
                ctx.StateMachine.ChangeState(new RunState(ctx));
            else
                ctx.StateMachine.ChangeState(new IdleState(ctx));

        }
    }

    public void Exit() { }
}