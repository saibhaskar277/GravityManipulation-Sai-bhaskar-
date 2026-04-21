using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class PlayerMotor : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 6f;
    public float acceleration = 20f;
    public float airControl = 0.4f;

    [Header("Gravity")]
    public float gravityStrength = 25f;
    public float jumpForce = 8f;

    [Header("Rotation")]
    public float rotationSpeed = 12f;

    [Header("Ground Check")]
    public float groundCheckDistance = 0.3f;

    [Header("References")]
    public Transform cameraTransform;

    private PlayerInput input;
    private Rigidbody rb;
    private CapsuleCollider col;

    // cached every frame in FixedUpdate — shared across all methods
    private Vector3 gravityDir;
    private Vector3 upDir;

    // cached once in Init — collider dimensions never change at runtime
    private float groundSphereRadius;
    private float groundSphereOffset;

    private Vector3 lastGravity;

    public bool IsGrounded { get; private set; }

    // ── Init ────────────────────────────────────────────

    public void Init(PlayerInput inputRef)
    {
        input = inputRef;
        rb = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();

        rb.useGravity = false;
        rb.freezeRotation = true;
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        // bake collider-derived constants — no need to recompute each frame
        groundSphereRadius = col.radius * 0.9f;
        groundSphereOffset = col.height * 0.5f - col.radius;

        lastGravity = input.CurrentGravity;
        CacheGravityVectors();
    }

    // ── Fixed Update ────────────────────────────────────

    void FixedUpdate()
    {
        if (input == null) return;

        // normalize once → gravityDir & upDir reused by every method this frame
        CacheGravityVectors();

        HandleGravityChange();
        CheckGround();
        ApplyGravity();
    }

    // ── Gravity cache ───────────────────────────────────

    void CacheGravityVectors()
    {
        gravityDir = input.CurrentGravity.normalized;
        upDir = -gravityDir;
    }

    // ── Movement ────────────────────────────────────────
    Vector3 camForward, camRight, moveDir, desiredVelocity,  vel, vertical, horizontal, origin;  
    public void Move(Vector2 inputAxis)
    {
        if (cameraTransform == null) return;

        camForward = Vector3.ProjectOnPlane(cameraTransform.forward, upDir).normalized;
        camRight = Vector3.ProjectOnPlane(cameraTransform.right, upDir).normalized;

        moveDir = (camForward * inputAxis.y + camRight * inputAxis.x).normalized;
        desiredVelocity = moveDir * speed;  
        // read velocity once
        vel = rb.linearVelocity;
        vertical = Vector3.Project(vel, gravityDir);
        horizontal = vel - vertical;

        float control = IsGrounded ? 1f : airControl;

        rb.linearVelocity = Vector3.Lerp(
            horizontal,
            desiredVelocity,
            acceleration * control * Time.fixedDeltaTime
        ) + vertical;


        // Rotation
        if (moveDir.sqrMagnitude < 0.001f) return;

        rb.MoveRotation(Quaternion.Slerp(
            rb.rotation,
            Quaternion.LookRotation(moveDir, upDir),
            rotationSpeed * Time.fixedDeltaTime
        ));
    }
     

    // ── Gravity ─────────────────────────────────────────
    float verticalSpeed, multiplier;
    void ApplyGravity()
    {
        verticalSpeed = Vector3.Dot(rb.linearVelocity, gravityDir);
        multiplier = verticalSpeed > 0f ? 2.5f : 1.2f;

        rb.AddForce(gravityDir * (gravityStrength * multiplier), ForceMode.Acceleration);
    }

    void HandleGravityChange()
    {
        if (Vector3.Angle(lastGravity, input.CurrentGravity) < 0.1f) return;

        Quaternion align = Quaternion.FromToRotation(-lastGravity.normalized, upDir);
        rb.linearVelocity = align * rb.linearVelocity;
        lastGravity = input.CurrentGravity;
    }

    // ── Jump ────────────────────────────────────────────

    public void Jump()
    {
        if (!IsGrounded) return;

        Vector3 vel = rb.linearVelocity;
        vel -= Vector3.Project(vel, gravityDir);
        rb.linearVelocity = vel;
        rb.AddForce(upDir * jumpForce, ForceMode.VelocityChange);
    }

    // ── Ground Check ────────────────────────────────────

    void CheckGround()
    {
        origin = transform.position + upDir * groundSphereOffset;

        IsGrounded = Physics.SphereCast(origin, groundSphereRadius, gravityDir,
                         out RaycastHit hit, groundCheckDistance)
                     && Vector3.Angle(hit.normal, upDir) < 60f;
    }
}