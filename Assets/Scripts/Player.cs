using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public bool Reset = false;
    public bool FlipLeanControls = false;

    // Components
    WheelCollider Wheel;
    Rigidbody Body;
    BoxCollider PickupArea;
    PlayerControls Controls;

    [Header("Hands")]
    Transform HoldPointR;
    FixedJoint HoldJointR;
    GameObject HeldObjectR;

    Transform HoldPointL;
    FixedJoint HoldJointL;
    GameObject HeldObjectL;

    [Header("Movement Parameters")]
    public float Speed = 20f;
    public float TurnTorque = 5f;
    public float TurnSpeed = 5f;

    [Header("Leaning Parameters")]
    public float LeanSpeed = 30f;
    public float LeanTurnSpeed = 3f;
    public float LeanTurnAtMinVelocity = 0.2f;
    public float LeanTurnVelocityFactor = 0.5f;

    [Header("Balancing Parameters")]
    public float MinimumSpeedForBalancing = 0.1f;
    public float MinimumSpeedFactor = 1.0f;
    public float SpeedFactor = 1.5f;
    public float ProportionalBalancing = 1.0f;
    public float Damping = 1.0f;
    public float outputMax = 15.0f;

    void Start()
    {
        Wheel = GetComponentInChildren<WheelCollider>();
        PickupArea = GetComponentInChildren<BoxCollider>();
        Body = transform.Find("Body").GetComponent<Rigidbody>();

        // Setup Hands
        HoldPointR = GameObject.Find("HoldPointR").transform;
        HoldJointR = HoldPointR.GetComponent<FixedJoint>();

        HoldPointL = GameObject.Find("HoldPointL").transform;
        HoldJointL = HoldPointL.GetComponent<FixedJoint>();

        Controls = new PlayerControls();
        Controls.Enable();
    }

    void FixedUpdate()
    {
        HandleMovement();
        HandleBalancing();

        // Handle Interactions
        if (Controls.Player.InteractR.WasPressedThisFrame())
        {
            HandleInteraction(HoldPointR, HoldJointR, ref HeldObjectR);
        }

        if (Controls.Player.InteractL.WasPressedThisFrame())
        {
            HandleInteraction(HoldPointL, HoldJointL, ref HeldObjectL);
        }

        // Reset the level
        if (Reset || Controls.Player.Reset.IsPressed())
        {
            Controls.Disable();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    // --- PICKUP AND DROP ---

    void HandleInteraction(Transform holdPoint, FixedJoint holdJoint, ref GameObject heldObject)
    {
        if (heldObject != null)
        {
            holdJoint.connectedBody = null;
            heldObject = null;
            return;
        }

        Collider[] hitColliders = Physics.OverlapBox(PickupArea.transform.position, PickupArea.size / 2, PickupArea.transform.rotation);

        GameObject bestTarget = null;
        float closestDistanceSqr = float.MaxValue;

        foreach (Collider hit in hitColliders)
        {
            GameObject potentialItem = hit.gameObject;

            if (!potentialItem.CompareTag("Plates")) continue;

            if (potentialItem == HeldObjectR || potentialItem == HeldObjectL) continue;

            float distSqr = (potentialItem.transform.position - holdPoint.position).sqrMagnitude;
            if (distSqr < closestDistanceSqr)
            {
                closestDistanceSqr = distSqr;
                bestTarget = potentialItem;
            }
        }

        if (bestTarget != null)
        {
            heldObject = bestTarget;
            Rigidbody targetRb = heldObject.GetComponent<Rigidbody>();

            if (targetRb != null)
            {
                heldObject.transform.position = holdPoint.position;
                heldObject.transform.rotation = holdPoint.rotation;

                holdJoint.connectedBody = targetRb;

                Debug.Log($"Picked up {heldObject.name} with {holdPoint.name}");
            }
        }
    }

    // --- MOVEMENT & BALANCING ---

    void HandleMovement()
    {
        Vector2 moveInput = Controls.Player.Move.ReadValue<Vector2>();
        Vector2 leanInput = Controls.Player.Lean.ReadValue<Vector2>();

        // Forward/Backward
        Wheel.motorTorque = moveInput.y * Speed;

        // Turning
        Body.AddRelativeTorque(0, moveInput.x * TurnTorque, 0);

        // Leaning Forces
        if (FlipLeanControls)
        {
            Body.AddRelativeForce(leanInput.y * LeanSpeed, 0, -leanInput.x * LeanSpeed);
        }
        else
        {
            Body.AddRelativeForce(leanInput.x * LeanSpeed, 0, leanInput.y * LeanSpeed);
        }

        // Camber Thrust (Leaning Turn)
        float leanAmount = Mathf.DeltaAngle(0f, Body.transform.eulerAngles.x) / 180f;

        float currentVelocity = Body.linearVelocity.magnitude;

        float velocityFactor = LeanTurnAtMinVelocity + currentVelocity * LeanTurnVelocityFactor;
        Body.AddRelativeTorque(0, -leanAmount * LeanTurnSpeed * velocityFactor, 0);
    }

    void HandleBalancing()
    {
        Vector2 leanInput = Controls.Player.Lean.ReadValue<Vector2>();
        Body.AddRelativeTorque(BalanceControl(leanInput != Vector2.zero));
    }

    Vector3 BalanceControl(bool leaning)
    {
        float angleX = Mathf.DeltaAngle(0f, Body.transform.eulerAngles.x);
        float angleZ = Mathf.DeltaAngle(0f, Body.transform.eulerAngles.z);

        float velX = Body.angularVelocity.x;
        float velZ = Body.angularVelocity.z;

        float damping = Damping;
        if (leaning) damping = 0f;

        float currentSpeed = Body.linearVelocity.magnitude;

        float speedFactor = currentSpeed < MinimumSpeedForBalancing ? MinimumSpeedFactor : SpeedFactor * currentSpeed;
        speedFactor = 1.0f; // Overridden for testing

        float torqueX = -(ProportionalBalancing * angleX + Damping * velX) * speedFactor;
        float torqueZ = -(ProportionalBalancing * angleZ + Damping * velZ) * speedFactor;

        return new Vector3(
            Mathf.Clamp(torqueX, -outputMax, outputMax),
            0,
            Mathf.Clamp(torqueZ, -outputMax, outputMax)
        );
    }
}