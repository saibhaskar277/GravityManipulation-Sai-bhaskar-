using UnityEngine;

public class ThirdPersonSpiderCamera : MonoBehaviour
{
    [Header("Target")]
    public Transform target;
    public PlayerInput input;
    public Vector3 offset = new Vector3(0, 1.5f, 0);

    [Header("Distance")]
    public float distance = 5f;
    public float minDistance = 1f;
    public float pullInSpeed = 15f;   // how fast cam snaps toward player on hit
    public float pullOutSpeed = 4f;    // how slowly it eases back out (feels smoother)

    [Header("Sensitivity")]
    public float sensitivity = 220f;

    [Header("Collision")]
    public float sphereRadius = 0.3f;  // size of the probe sphere
    public LayerMask blockingLayers = ~0; // everything by default; exclude Player layer in Inspector

    private float yaw;
    private float pitch = 20f;
    private Vector3 currentUp;
    private float currentDist; // actual distance after collision

    // ── Init ────────────────────────────────────────────

    void Start()
    {
        currentUp = Vector3.up;
        currentDist = distance;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        input.OnGravityChanged += OnGravityChanged;
    }

    void OnDestroy()
    {
        input.OnGravityChanged -= OnGravityChanged;
    }

    // ── Gravity change ──────────────────────────────────

    void OnGravityChanged(Vector3 oldGravity, Vector3 newGravity)
    {
        Vector3 newUp = -newGravity.normalized;
        Vector3 flatForward = Vector3.ProjectOnPlane(transform.forward, newUp);

        if (flatForward.sqrMagnitude > 0.001f)
        {
            flatForward.Normalize();
            yaw = Vector3.SignedAngle(GetNorth(newUp), flatForward, newUp);
        }
    }

    // ── Late Update ─────────────────────────────────────

    void LateUpdate()
    {
        Vector3 targetUp = -input.CurrentGravity.normalized;
        currentUp = Vector3.Slerp(currentUp, targetUp, 10f * Time.deltaTime);

        // ── Mouse input ─────────────────────────────────
        yaw += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        pitch -= Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, -30f, 75f);

        // ── Orbit direction ─────────────────────────────
        Vector3 north = GetNorth(currentUp);
        Vector3 orbitDir = Quaternion.AngleAxis(yaw, currentUp) * north;

        float pitchRad = pitch * Mathf.Deg2Rad;
        Vector3 camDir = -orbitDir * Mathf.Cos(pitchRad)
                           + currentUp * Mathf.Sin(pitchRad);

        Vector3 pivot = target.position + currentUp * offset.y;

        // ── Collision ───────────────────────────────────
        float desiredDist = GetDesiredDistance(pivot, camDir);

        // pull in fast, ease out slowly — same feel as Cinemachine
        float lerpSpeed = desiredDist < currentDist ? pullInSpeed : pullOutSpeed;
        currentDist = Mathf.Lerp(currentDist, desiredDist, lerpSpeed * Time.deltaTime);

        // ── Apply ───────────────────────────────────────
        Vector3 pos = pivot + camDir * currentDist;
        transform.position = pos;
        transform.rotation = Quaternion.LookRotation(pivot - pos, currentUp);
    }

    // ── Collision check ─────────────────────────────────

    float GetDesiredDistance(Vector3 pivot, Vector3 camDir)
    {
        // SphereCast from pivot outward along camDir up to full distance.
        // If something is hit, clamp to the hit point minus a small margin
        // so the sphere never pokes through the surface.
        if (Physics.SphereCast(
                pivot,
                sphereRadius,
                camDir,
                out RaycastHit hit,
                distance,
                blockingLayers,
                QueryTriggerInteraction.Ignore))
        {
            // hit.distance is pivot→hit surface; subtract radius so the
            // camera sphere sits just in front of the wall, not inside it
            return Mathf.Clamp(hit.distance - sphereRadius, minDistance, distance);
        }

        return distance;
    }

    // ── Helpers ─────────────────────────────────────────

    static Vector3 GetNorth(Vector3 up)
    {
        Vector3 n = Vector3.ProjectOnPlane(Vector3.forward, up);
        if (n.sqrMagnitude < 0.001f)
            n = Vector3.ProjectOnPlane(Vector3.right, up);
        return n.normalized;
    }
}