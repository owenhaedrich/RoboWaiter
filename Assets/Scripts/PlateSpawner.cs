using UnityEngine;

public class PlateSpawner : MonoBehaviour
{
    public Transform[] spawnPoints;
    public GameObject[] platePrefabs;

    public float spawnInterval = 0.03f;
    private float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0f;
            SpawnPlate();
        }
    }

    void SpawnPlate()
    {
        int p = Random.Range(0, platePrefabs.Length);
        int s = Random.Range(0, spawnPoints.Length);

        Instantiate(
            platePrefabs[p],
            spawnPoints[s].position,
            spawnPoints[s].rotation
        );
    }
}

