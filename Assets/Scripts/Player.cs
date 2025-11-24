using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    WheelCollider Wheel;
    Rigidbody Body;
    PlayerControls Controls;
    public float Speed = 20f;
    public float TurnTorque = 5f;
    public bool Reset = false;

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
        Vector2 input = Controls.Player.Move.ReadValue<Vector2>();
        Wheel.motorTorque = input.y * Speed;
        Body.AddRelativeTorque(0, input.x * TurnTorque, 0);

        // Apply balancing torque
        Body.AddRelativeTorque(BalanceControl());
        //Debug.Log(BalanceControl());

        if (Reset)
        {
           SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    // Try to keep the player balanced upright using PID controller
    public float GainP = 3.0f;
    public float GainI = 1.0f;
    public float GainD = 1.0f;
    public float lastErrorX = 0.0f;
    public float integratedErrorX = 0.0f;
    public float lastErrorZ = 0.0f;
    public float integratedErrorZ = 0.0f;
    public float outputMax = 25.0f;
    public float outputMin = -25.0f;

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
            Mathf.Clamp(torqueX, outputMin, outputMax),
            0,
            Mathf.Clamp(torqueZ, outputMin, outputMax)
        );
    }
}
