using UnityEngine;
using TMPro;

public class ScoreKeeper : MonoBehaviour
{
    private int score = 0;

    public TextMeshProUGUI scoreText; 

    void Awake()
    {
        UpdateScoreDisplay();
    }

    public void UpdateScore(int scoreToAdd)
    {
        score += scoreToAdd;
        UpdateScoreDisplay();
    }

    private void UpdateScoreDisplay()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score.ToString();
        }
        else
        {
            Debug.LogError("Score Text (TMP) reference is missing on the ScoreKeeper!");
        }
    }
}