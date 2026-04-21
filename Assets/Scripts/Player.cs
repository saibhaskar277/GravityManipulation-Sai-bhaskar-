using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("References")]
    public Transform cameraTransform; // drag Main Camera here in Inspector
    public Animator anim;

    private PlayerStateMachine stateMachine;

    void Awake()
    {
        var input = GetComponent<PlayerInput>();
        var motor = GetComponent<PlayerMotor>();

        motor.cameraTransform = cameraTransform;
        motor.Init(input);

        var ctx = new PlayerContext
        {
            Input = input,
            Motor = motor,
            Animator = anim,
            CameraTransform = cameraTransform,
        };

        stateMachine = new PlayerStateMachine();
        ctx.StateMachine = stateMachine;

        stateMachine.ChangeState(new IdleState(ctx));
    }

    void Update() => stateMachine.Update();
}