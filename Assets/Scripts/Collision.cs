using UnityEngine;

public class Collision : MonoBehaviour
{
    private Vector3 startPos = new Vector3(0, 1.5f, 0);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(UnityEngine.Collision other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Hit an obstacle!");
            transform.position = startPos;
            transform.rotation = Quaternion.identity;
        }
    }
}
