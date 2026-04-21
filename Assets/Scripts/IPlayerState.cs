using UnityEngine;

public interface IPlayerState
{
    void Enter();
    void Tick();
    void Exit();
}


public class PlayerContext
{
    public PlayerInput Input;
    public PlayerMotor Motor;
    public Animator Animator;
    public PlayerStateMachine StateMachine;
    public Transform CameraTransform;
}