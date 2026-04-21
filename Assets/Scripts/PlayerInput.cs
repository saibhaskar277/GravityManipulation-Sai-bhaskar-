using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public Vector2 Move { get; private set; }
    public bool JumpPressed { get; private set; }
    public Vector3 CurrentGravity { get; private set; } = Vector3.down;
    public Vector3 PreviewGravity { get; private set; }
    public bool IsPreviewing { get; private set; }

    public event System.Action<Vector3, Vector3> OnGravityChanged;

    void Update()
    {
        Move = Vector2.ClampMagnitude(
            new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")), 1f);

        JumpPressed = Input.GetKeyDown(KeyCode.Space);

        HandlePreview();
        HandleConfirm();
    }
    Vector3 forward, right;
    void HandlePreview()
    {
        IsPreviewing = false;

        forward = transform.forward;
        right = transform.right;

        if (Input.GetKey(KeyCode.I)) // forward
        {
            PreviewGravity = -forward;
            IsPreviewing = true;
        }
        else if (Input.GetKey(KeyCode.K)) // backward
        {
            PreviewGravity = forward;
            IsPreviewing = true;
        }
        else if (Input.GetKey(KeyCode.J)) // left
        {
            PreviewGravity = right;
            IsPreviewing = true;
        }
        else if (Input.GetKey(KeyCode.L)) // right
        {
            PreviewGravity = -right;
            IsPreviewing = true;
        }

        if (IsPreviewing)
            PreviewGravity = PreviewGravity.normalized;
    }

    void HandleConfirm()
    {
        if (IsPreviewing && Input.GetKeyDown(KeyCode.Return))
        {
            Vector3 oldGravity = CurrentGravity;
            CurrentGravity = PreviewGravity;

            OnGravityChanged?.Invoke(oldGravity, CurrentGravity);
        }
    }
}