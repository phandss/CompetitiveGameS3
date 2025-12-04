using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int score = 0;
    public int lives = 3;
    public int WinCon = 10;

    UIManager uiManager;

    public static GameManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddScore(int amount)
    {
        score = score + amount;
        
        if(score >= WinCon)
        {
            uiManager.WinGame();
        }
    }

    public void LoseLife()
    {
        lives = lives - 1;
        uiManager.RemoveHeart();

        if (lives <= 0)
        {
            uiManager.LoseGame();
        }
    }



    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        score = 0;
        lives = 3;
        uiManager.ResetHealthBar();
        
    }
}
