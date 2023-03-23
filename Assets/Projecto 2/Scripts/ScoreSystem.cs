using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class ScoreSystem : MonoBehaviour
{
    [Header("Si se quiere resetear el Highscore, este es tu Script, ve a Start y descomenta el comentario, de nada")]
    private static ScoreSystem _instance;
    public TextMeshPro scoreText;
    public TextMeshPro highscoreText;
    public static int score = 0;
    public static int highscore = 0;
   


    public static ScoreSystem Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<ScoreSystem>();
                if (_instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(ScoreSystem).Name;
                    _instance = obj.AddComponent<ScoreSystem>();
                }
            }
            return _instance;
        }
    }

    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as ScoreSystem;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    { 
        score = 0;
        Load();

        /** Por si se quiere resetear el highscore
        highscore = 0;
        Save(); 
         */
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
        //PlayerPrefs.SetInt("score", score);
        PlayerPrefs.SetInt("highscore", highscore);
    }
    public void Load()
    {
        //score = PlayerPrefs.GetInt("score");
        highscore = PlayerPrefs.GetInt("highscore");
    }
}