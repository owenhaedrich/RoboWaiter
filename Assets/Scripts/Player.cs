using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public bool Reset = false;
    WheelCollider Wheel;
    Rigidbody Body;
    PlayerControls Controls;
    public float Speed = 20f;
    public float TurnTorque = 5f;
    public float TurnSpeed = 5f;
    public float LeanSpeed = 10f;
    public float LeanTurnSpeed = 5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Wheel = GetComponentInChildren<WheelCollider>();
        Body = transform.Find("Body").GetComponent<Rigidbody>();
        Controls = new PlayerControls();
        Controls.Enable();
    }

    // FixedUpdate is called once per physics update
    void FixedUpdate()
    {
        Vector2 moveInput = Controls.Player.Move.ReadValue<Vector2>();
        Vector2 leanInput = Controls.Player.Lean.ReadValue<Vector2>();

        // Forward and backward movement
        Wheel.motorTorque = moveInput.y * Speed;

        // Left and right turning
        Body.AddRelativeTorque(0, moveInput.x * TurnTorque, 0);

        // Leaning
        Body.AddRelativeTorque(-leanInput.x * LeanSpeed, 0, -leanInput.y * LeanSpeed);

        // Leaning turn
        float leanAmount = Mathf.DeltaAngle(0f, Body.transform.eulerAngles.x) / 180;
        //Body.AddRelativeTorque(0, -leanAmount * LeanTurnSpeed, 0);

        // Apply balancing torque
        Body.AddRelativeTorque(BalanceControl());
        Debug.Log(BalanceControl());

        if (Reset)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    // Try to keep the player balanced upright using PD controller
    public float GainP = 3.0f;
    public float GainD = 1.0f;
    public float outputMax = 25.0f;

    Vector3 BalanceControl()
    {
        // Get signed angles (-180 to 180)
        float angleX = Mathf.DeltaAngle(0f, Body.transform.eulerAngles.x);
        float angleZ = Mathf.DeltaAngle(0f, Body.transform.eulerAngles.z);

        // Get angular velocities for damping
        float velX = Body.angularVelocity.x;
        float velZ = Body.angularVelocity.z;

        // Simple PD control (often better than PID for balancing)
        float torqueX = -(GainP * angleX + GainD * velX);
        float torqueZ = -(GainP * angleZ + GainD * velZ);

        return new Vector3(
            Mathf.Clamp(torqueX, -outputMax, outputMax),
            0,
            Mathf.Clamp(torqueZ, -outputMax, outputMax)
        );
    }
}
