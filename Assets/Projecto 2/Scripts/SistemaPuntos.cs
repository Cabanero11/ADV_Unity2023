using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class SistemaPuntos : MonoBehaviour
{
    public TextMeshPro scoreText;
    public TextMeshPro highscoreText;
    public static int score = 0;
    public static int highscore = 0;

    private void Start()
    {
        score = 0;
        Debug.Log(score);
        Load();
    }

    public void Update()
    {
        scoreText.text = "Puntuación: " + score.ToString();
        highscoreText.text = "Record: " + highscore.ToString();

        if (score > highscore)
        {
            highscore = score;
            Save();
        }
    }
    public void AddScore(int amount)
    {
        score += amount;
        Save();
    }
    public void SubtractScore(int amount)
    {
        score -= amount;
        Save();
    }
    public void Save()
    {
        PlayerPrefs.SetInt("highscore", highscore);
    }
    public void Load()
    {
        highscore = PlayerPrefs.GetInt("highscore");
    }
}