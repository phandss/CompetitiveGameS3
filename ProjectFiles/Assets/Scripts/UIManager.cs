using System;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    PlayerController player;
    public int numOfHearts;
    public Image[] hearts;

    public Image ability1;
    public Image ability2;
    public Image ability3;

    public TMP_Text healthText;

    public GameObject winScreen;
    public GameObject loseScreen;


    private void Start()
    {
        ability1 = GameObject.Find("Ability1").GetComponent<Image>();
        ability2 = GameObject.Find("Ability2").GetComponent<Image>();
        ability3 = GameObject.Find("Ability3").GetComponent<Image>();

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        healthText = GameObject.Find("HealthText").GetComponent<TMP_Text>();

        healthText.text = "Health: " + player.playerHealth.ToString();

    }


    public void RemoveHeart()
    {
        if (numOfHearts > 0)
        {
            numOfHearts--;
            hearts[numOfHearts].enabled = false;
        }
    }


    public void ResetHealthBar()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].enabled = true;
        }
        numOfHearts = hearts.Length;
    }

    public void ChangeHealthText(float playerHealth)
    {
        healthText.text = "Health: " + playerHealth.ToString();
    }

    private void ShowCoolDown(int abilityNum, float coolDownLength)
    {
        // Implementation for showing cooldown on UI
    }

    public void WinGame()
    {
        winScreen.SetActive(true);
    }

    public void LoseGame()
    {
        loseScreen.SetActive(true);
    }

    public void RestartButton()
    {
        GameManager.instance.ResetGame();
        winScreen.SetActive(false);
        loseScreen.SetActive(false);

    }


    private void CloseGame()
    {
        Application.Quit();
    }
}
