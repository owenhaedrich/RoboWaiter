using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform FollowTarget;
    public Vector3 Offset = new Vector3(0, 5, -10);

    // FixedUpdate is called once per physics update
    void FixedUpdate()
    {
        transform.position = FollowTarget.position + Offset;
    }
}
