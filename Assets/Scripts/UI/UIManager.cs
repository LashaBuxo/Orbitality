using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System;
public class UIManager : MonoBehaviour
{
    public MenuType currentMenu = MenuType.Hidden;

    [Header("Menu GameObjects")]
    public GameObject mainMenu;
    public GameObject modesMenu;
    public GameObject continueBut;
    public GameObject saveBut;
    public GameObject loadBut;

    [Header("Game Properties")]
    public MySlider minEnemiesSlider;
    public MySlider maxEnemiesSlider;

    public GameConfig gameConfig;
    public static UIManager instance;

    [NonSerialized]
    public bool savedFileExist = false;
    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject); 
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;

        minEnemiesSlider.unitySlider.maxValue = gameConfig.maxEnemyPlayers;
        maxEnemiesSlider.unitySlider.maxValue = gameConfig.maxEnemyPlayers;

        maxEnemiesSlider.value = gameConfig.maxEnemyPlayers;
        maxEnemiesSlider.unitySlider.value = gameConfig.maxEnemyPlayers;

        string path = Path.Combine(Application.persistentDataPath, gameConfig.fileName);
        savedFileExist = File.Exists(path);
    }
     
    public void updateMenu(MenuType menuType)
    {
        currentMenu = menuType;
        switch (menuType)
        {
            case MenuType.Hidden:
                mainMenu.SetActive(false);
                modesMenu.SetActive(false);
                break;
            case MenuType.MainMenu:
                mainMenu.SetActive(true);
                modesMenu.SetActive(false);
                PauseGame();
                break;
            case MenuType.ModesMenu:
                mainMenu.SetActive(false);
                modesMenu.SetActive(true);
                PauseGame();
                break; 
            default:
                print("Ooops, Incorrect Menu type.");
                break;
        } 
    }

    public void OnClickContinue()
    {
        if (GameManager.instance == null)
        {
            Debug.LogError("Oops, Game has not started. can't continue.");
            return;
        }
        ResumeGame();
        updateMenu(MenuType.Hidden);
    }

    private bool isLoadRequest = false;
    public void OnClickNewGame()
    { 
        updateMenu(MenuType.Hidden);
        ResumeGame();
        isLoadRequest = false;
        SceneManager.LoadScene(1); 
    }
    public void OnClickLoad()
    {
        updateMenu(MenuType.Hidden);
        ResumeGame();
        isLoadRequest = true;
        SceneManager.LoadScene(1);
        //updateMenu(MenuType.MainMenu);
    }

    public void OnClickQuit()
    {
        Application.Quit();
    } 
    public void OnClickModes()
    { 
        updateMenu(MenuType.ModesMenu);
    }
    public void OnClickBackFromModes()
    {
        updateMenu(MenuType.MainMenu);
    }
    public void OnClickSave()
    {
        GameManager.instance.SaveGame();
    }

      
    public void PauseGame()
    {
        if (GameManager.gameStatus!=GameStatus.Finished && GameManager.gameStatus!=GameStatus.NotStarted)
            GameManager.gameStatus = GameStatus.Paused;
        Time.timeScale = 0;
    }
    public void ResumeGame()
    {
        Time.timeScale = 1;
        GameManager.gameStatus = GameStatus.Running;
    }

    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if (arg0.buildIndex == 0)
        { 
            updateMenu(MenuType.MainMenu);
        } else
        { 
            if (!isLoadRequest)
                GameManager.instance.StartGame(minEnemiesSlider.value, maxEnemiesSlider.value);
            else
                GameManager.instance.loadGame();
        }
    }

    private void Update()
    {  
        if (GameManager.gameStatus == GameStatus.Paused)
        {
            continueBut.SetActive(true);
            saveBut.SetActive(true);
        } else
        {
            continueBut.SetActive(false);
            saveBut.SetActive(false);
        }
        if (savedFileExist)
        {
            loadBut.SetActive(true);
        } else
        {
            loadBut.SetActive(false);
        }
    }
}
