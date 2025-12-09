using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform FollowTarget;
    public Vector3 Offset = new Vector3(0, 5, -10);
    public float CameraLerpSpeed = 5f;
    public float MaxTiltAngle = 30f;
    public Vector3 RotationOffset = new Vector3(0, 90f, 0);

    void FixedUpdate()
    {
        // Calculate base rotation
        Quaternion targetYRotation = Quaternion.Euler(0, FollowTarget.rotation.eulerAngles.y, 0);

        // Clamp X/Z tilt
        float targetXAngle = FollowTarget.rotation.eulerAngles.x;
        float targetZAngle = FollowTarget.rotation.eulerAngles.z;

        if (targetXAngle > 180) targetXAngle -= 360;
        if (targetZAngle > 180) targetZAngle -= 360;

        float clampedX = Mathf.Clamp(targetXAngle, -MaxTiltAngle, MaxTiltAngle);
        float clampedZ = Mathf.Clamp(targetZAngle, -MaxTiltAngle, MaxTiltAngle);

        Quaternion targetTiltRotation = Quaternion.Euler(clampedX, 0, clampedZ);
        Quaternion baseRotation = targetYRotation * targetTiltRotation;

        // Apply rotation correction
        Quaternion offsetCorrection = Quaternion.Euler(RotationOffset);
        Quaternion targetRotation = baseRotation * offsetCorrection;

        // Apply rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * CameraLerpSpeed);

        // Calculate and apply position
        Vector3 rotatedOffset = transform.rotation * Offset;
        Vector3 targetPosition = FollowTarget.position + rotatedOffset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.fixedDeltaTime * CameraLerpSpeed);
    }
}