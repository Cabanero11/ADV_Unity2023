using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }
    public TextMeshPro scoreText;

    private int score = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddScore(int scoreValue)
    {
        score += scoreValue;
        Debug.Log("Puntuacion: " + score);
    }



    private void Update()
    {
        scoreText.text = "Puntuación: " + score.ToString();
    }
}