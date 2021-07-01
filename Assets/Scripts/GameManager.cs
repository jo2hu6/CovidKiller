using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int time = 30;
    public int difficulty = 1;
    public bool gameOver;
    [SerializeField] int score;
    [SerializeField] Text timeText;
    bool begin = false;
    [SerializeField] AudioClip buttonClip, deathClip;
    bool isPaused = false;

    public int Score
    {
        get => score;
        set
        {
            score = value;
            UIManager.Instance.UpdateUIScore(score);
            if(score % 1000 == 0)
            {
                difficulty++;
            }
        }
    }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        UIManager.Instance.UpdateUIScore(score);
        StartCoroutine(CountDownRoutine());
    }

    IEnumerator CountDownRoutine()
    {
        while(time > 0)
        {
            yield return new WaitForSeconds(1);
            time--;
            UIManager.Instance.UpdateUITime(time);
        }

        if(!isPaused)
        {
            if(time == 0)
            {
                gameOver = true;
                UIManager.Instance.ShowGameOverScreen();
                UIManager.Instance.SpawnPointsDisabled(true);
                UIManager.Instance.PlayerDisabled(true);
                Camera.main.GetComponent<AudioSource>().Stop();
                AudioSource.PlayClipAtPoint(deathClip, transform.position);
                Time.timeScale = 0;
                isPaused = true;
            }
        }

        if(isPaused)
        {
            Time.timeScale = 1;
            UIManager.Instance.SpawnPointsDisabled(false);
            UIManager.Instance.PlayerDisabled(false);
            isPaused = false;
        }

    }

    public void PlayAgainRetard()
    {
        SceneManager.LoadScene("Game");
    }

    public void PlayAgain()
    {
        Invoke("PlayAgainRetard", 0.5f);
        AudioSource.PlayClipAtPoint(buttonClip, transform.position);
    }

}
