using Unity.VisualScripting;
using UnityEditor.ShaderGraph;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform FollowTarget;

    [Header("Third Person Settings")]
    public Vector3 DefaultOffset = new Vector3(0, 5, -10);
    Vector3 DefaultRotationOffset = new Vector3(0, 90f, 0);

    [Header("Top Down Settings")]
    public Vector3 ToggledOffset = new Vector3(0, 20, 0);
    Vector3 ToggledRotationOffset = new Vector3(90f, 180f, 0);

    [Header("General Settings")]
    public float CameraLerpSpeed = 5f;
    public float MaxTiltAngle = 30f;

    // Internal State
    Vector3 Offset;
    Vector3 RotationOffset;
    bool isTopDown = false; // Track the current mode

    PlayerControls Controls;

    void Start()
    {
        Controls = new PlayerControls();
        Controls.Enable();

        // Initialize to Default (Third Person)
        Offset = DefaultOffset;
        RotationOffset = DefaultRotationOffset;
        isTopDown = false;
    }

    void Update()
    {
        if (Controls.Player.CameraToggle.WasPressedThisFrame())
        {
            // Toggle the state
            isTopDown = !isTopDown;

            // Apply the state
            if (isTopDown)
            {
                Offset = ToggledOffset;
                RotationOffset = ToggledRotationOffset;
            }
            else
            {
                Offset = DefaultOffset;
                RotationOffset = DefaultRotationOffset;
            }
        }
    }

    void FixedUpdate()
    {
        if (isTopDown)
        {
            // TOP DOWN LOGIC
            Quaternion targetRotation = Quaternion.Euler(RotationOffset);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * CameraLerpSpeed);

            Vector3 targetPosition = FollowTarget.position + Offset;
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.fixedDeltaTime * CameraLerpSpeed);
        }
        else
        {
            // THIRD PERSON LOGIC

            Quaternion targetYRotation = Quaternion.Euler(0, FollowTarget.rotation.eulerAngles.y, 0);

            float targetXAngle = FollowTarget.rotation.eulerAngles.x;
            float targetZAngle = FollowTarget.rotation.eulerAngles.z;

            if (targetXAngle > 180) targetXAngle -= 360;
            if (targetZAngle > 180) targetZAngle -= 360;

            float clampedX = Mathf.Clamp(targetXAngle, -MaxTiltAngle, MaxTiltAngle);
            float clampedZ = Mathf.Clamp(targetZAngle, -MaxTiltAngle, MaxTiltAngle);

            Quaternion targetTiltRotation = Quaternion.Euler(clampedX, 0, clampedZ);
            Quaternion baseRotation = targetYRotation * targetTiltRotation;

            Quaternion offsetCorrection = Quaternion.Euler(RotationOffset);
            Quaternion targetRotation = baseRotation * offsetCorrection;

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * CameraLerpSpeed);

            Vector3 rotatedOffset = transform.rotation * Offset;
            Vector3 targetPosition = FollowTarget.position + rotatedOffset;
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.fixedDeltaTime * CameraLerpSpeed);
        }
    }
}