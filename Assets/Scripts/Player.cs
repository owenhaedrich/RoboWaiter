using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    WheelCollider Wheel;
    PlayerControls Controls;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Wheel = GetComponentInChildren<WheelCollider>();
        Controls = new PlayerControls();
        Controls.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 input = Controls.Player.Move.ReadValue<Vector2>();
        Wheel.motorTorque = input.y * 15000f;
        Debug.Log(input);
    }
}
