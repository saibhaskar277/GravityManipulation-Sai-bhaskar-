using UnityEngine;

public class GravityPreview : MonoBehaviour
{
    public PlayerInput input;
    public Transform targetPreview;
    public GameObject previewObject;

    void Update()
    {
        if (input.IsPreviewing)
        {
            if (!previewObject.activeSelf) previewObject.SetActive(true);

            previewObject.transform.position = targetPreview.position;

            // 1. Determine the new 'Up' direction
            Vector3 newUp = -input.PreviewGravity.normalized;

            // 2. Project the player's current forward onto the new surface plane
            // This ensures the hologram faces the same way the player is facing
            Vector3 projectedForward = Vector3.ProjectOnPlane(targetPreview.forward, newUp);

            // 3. If forward becomes invalid (e.g., looking straight at the new floor), 
            // use the player's current up as a fallback forward
            if (projectedForward.sqrMagnitude < 0.001f)
                projectedForward = Vector3.ProjectOnPlane(targetPreview.up, newUp);

            // 4. Apply the rotation cleanly
            previewObject.transform.rotation = Quaternion.LookRotation(projectedForward, newUp);
        }
        else
        {
            if (previewObject.activeSelf) previewObject.SetActive(false);
        }
    }
}