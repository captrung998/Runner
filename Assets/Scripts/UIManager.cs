using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Image[] lifeHearts;
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI scoreText;

    public GameObject gameOverPanel;

    private static UIManager instance;
    public static UIManager Instance { get => instance; }


    private void Awake()
    {
        instance = this;
    }

    public void UpdateLives(int lives)
    {
        for (int i = 0; i < lifeHearts.Length; i++)
        {
            if (lives > i)
            {
                lifeHearts[i].color = Color.white;
            }
            else lifeHearts[i].color = Color.black;
        }
    }

    public void UpdateCoin(int coin)
    {
        coinText.text = coin.ToString();

    }

    public void GameOver()
    {
        Time.timeScale = 0;
        gameOverPanel.SetActive(true);
    }

    public void UpdateScore(int score)
    {        
        scoreText.text = "Score: " + score.ToString() + "m";
    }

}
