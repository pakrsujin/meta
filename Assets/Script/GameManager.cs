using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private static GameManager gameManager;
    
    public static GameManager instance{ get { return gameManager; } }

    public GameManager endPanel;
    private int currentScore = 0;
    UIManager uiManager;
    
    public UIManager UIManager { get { return uiManager; } }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        
        gameManager = this;
        uiManager = FindObjectOfType<UIManager>();
    }

    public void Start()
    {
        StartCoroutine(StartGameAfterDelay(2f));
        uiManager.UpdateScore(0);
        
    }
    private IEnumerator StartGameAfterDelay(float delay)
    {
        Time.timeScale = 0f; // 게임 일시정지
        yield return new WaitForSecondsRealtime(delay); // 실제 시간 기준 2초 대기
        Time.timeScale = 1f; // 게임 다시 실행
    }

    public void GameOver()
    {
        Debug.Log("Game Over");
        uiManager.SetRestart();
        Time.timeScale = 0.0f;
        endPanel.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void AddScore(int score)
    {
        currentScore += score;
        Debug.Log("Score:" + currentScore);
        uiManager.UpdateScore(currentScore);
    }
}
