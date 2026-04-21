using UnityEngine;

public class GravitySystem : MonoBehaviour
{
    public Transform player;
    public PlayerInput input;

    void Update()
    {
        RotatePlayerSmooth();
    }

    void RotatePlayerSmooth()
    {
        Quaternion targetRotation =
            Quaternion.FromToRotation(player.up, -input.CurrentGravity) * player.rotation;

        player.rotation = Quaternion.Slerp(
            player.rotation,
            targetRotation,
            10f * Time.deltaTime
        );
    }
}