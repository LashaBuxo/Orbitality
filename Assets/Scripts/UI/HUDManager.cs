using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HUDManager : MonoBehaviour
{
    public GameObject HUD;
    public GameObject gameFinished;
    public Text gameResult;

    public Text rocketStatus;
    public Image reloadBar;
    public Image healthBar;

    public PlanetManager playerManager; 

    public static HUDManager instance;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        
    }
    public void onMenuClick()
    {
        UIManager.instance.updateMenu(MenuType.MainMenu); 
    } 

    public void FinishGame(bool playerWon)
    {
        UIManager.instance.PauseGame();
        gameFinished.SetActive(true);
        HUD.SetActive(false);
        if (playerWon)
        {
            gameResult.text = "You Won!";
            gameResult.color = Color.green;
        } else
        {
            gameResult.text = "You Lost!";
            gameResult.color = Color.red;
        }
    }
    void Update()
    { 
        if (playerManager != null)
        { 
            rocketStatus.text = playerManager.currentStatus.ToString();
            if (playerManager.currentStatus == PlanetStatus.reloading)
            {
                reloadBar.fillAmount = 1 - playerManager.currentReloadCooldown / playerManager.rocketReloadCooldown;
            }
            healthBar.fillAmount = playerManager.healthBar.fillAmount;
            HUD.SetActive(true);
        } else
        {
            HUD.SetActive(false);
        }
    }
}
