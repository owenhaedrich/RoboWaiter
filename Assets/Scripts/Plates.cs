using UnityEngine;

public class Plate : MonoBehaviour
{
    public int plateType;    // 0, 1, 2

    private const int ScoreIncrease = 10;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Tables"))
        {
            ScoreKeeper scoreKeeper = FindFirstObjectByType<ScoreKeeper>();
            if (scoreKeeper != null)
            {
                scoreKeeper.UpdateScore(ScoreIncrease);
            }

            Destroy(gameObject);
        }
    }
}