using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public static UIManager Instance;
    [SerializeField] Text healthText;
    [SerializeField] Text scoreText;
    [SerializeField] Text timeText;
    [SerializeField] GameObject gameOverScreen;
    [SerializeField] Text finalScore;
    [SerializeField] GameObject spawnPoints, playerScene;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    public void UpdateUIScore(int newScore)
    {
        scoreText.text = newScore.ToString();
    }

    public void UpdateUIHealth(int newHealth)
    {
        healthText.text = newHealth.ToString();
    }

    public void UpdateUITime(int newTime)
    {
        timeText.text = newTime.ToString();
    }

    public void PlayerDisabled(bool cleanPlayer = false)
    {
        playerScene.SetActive(false);
        Object player = GameObject.FindGameObjectWithTag("Player");
        Destroy(player);
    }

    public void SpawnPointsDisabled(bool clean = false)
    {
        spawnPoints.SetActive(false);
        if(clean)
        {
            Object[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach(GameObject enemy in allEnemies)
            {
                Destroy(enemy);
            }
        }
    }
    
    public void ShowGameOverScreen()
    {
        gameOverScreen.SetActive(true);
        finalScore.text = ""+ GameManager.Instance.Score;
    }

}
