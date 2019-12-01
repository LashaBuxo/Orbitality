using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameManager : MonoBehaviour
{
    public GameConfig gameConfig;

    public static GameStatus gameStatus = GameStatus.NotStarted;
       
    private int enemiesCount;
    private int totalPlanets; 
    private List<GameObject> planets=new List<GameObject>(); 
    private List<int> randomMaterialsOrder;
    private List<int> randomOrbitsOrder;

    public static GameManager instance;  
    private void Awake()
    { 
        instance = this;
        DontDestroyOnLoad(gameObject);
    } 

    public void StartGame(int minEnemies,int maxEnemies)
    {
        planets.Clear();

        enemiesCount = Random.Range(minEnemies, maxEnemies + 1);
        totalPlanets = enemiesCount + 1;
        fixCameraOrthoSize();

        generateRandomVariables();
        generatePlanets();
        attachControllers();

        gameStatus = GameStatus.Running;
    }

    public void loadGame()
    {
        string path = Path.Combine(Application.persistentDataPath, gameConfig.fileName);
        GameState gameState = JsonUtility.FromJson<GameState>(File.ReadAllText(path));

        enemiesCount = gameState.planetsStates.Count - 1;
        totalPlanets = enemiesCount + 1;
        fixCameraOrthoSize();

        planets.Clear();
        generatePlanets(true, gameState);
        attachControllers();

        Debug.Log("Game State Succesfully loaded from: " + Application.persistentDataPath + " path.");
        gameStatus = GameStatus.Running;
    }

    private void fixCameraOrthoSize()
    {
        Camera.main.orthographicSize = (totalPlanets + 0.5f) * gameConfig.distanceBetweenOrbits;
    } 

    private void generateRandomVariables()
    { 
        randomMaterialsOrder = ComputationalFunctions.getRandomOrderFromInterval(totalPlanets, 0, totalPlanets); 
        randomOrbitsOrder = ComputationalFunctions.getRandomOrderFromInterval(totalPlanets, 0,totalPlanets); 
    } 

    private void generatePlanets(bool isLoadRequest=false,GameState gameState=null)
    {
        for (int i = 0; i < totalPlanets; i++)
        {
            GameObject planet=Instantiate(gameConfig.planetPrefab);
            planet.name = "Planet "+i;
            PlanetState planetState ;
            if (isLoadRequest)
            {
                planetState = gameState.planetsStates[i];
            }
            else
            {
                RocketType rocketType = ComputationalFunctions.getRandomRocketType();
                int matIndex = randomMaterialsOrder[i];
                Color col = i > 0 ? gameConfig.enemyColor : gameConfig.playerColor;
                Vector3 pos = ComputationalFunctions.getRandomPositionOnOrbit(randomOrbitsOrder[i], totalPlanets, gameConfig.distanceBetweenOrbits);
                float HP = gameConfig.planetMaxHP;
                float RT = GetRocketReloadCooldown(rocketType);

                planetState = new PlanetState(rocketType, matIndex, col, pos, HP, RT); 
            }

            planet.GetComponent<PlanetManager>().Initialize(planetState);
            planets.Add(planet); 
        }
    }

    public void attachControllers()
    { 
        planets[0].name += ":Player"; 
        planets[0].AddComponent<PlayerController>();
        HUDManager.instance.playerManager= planets[0].GetComponent<PlanetManager>();
        for (int i=1;i<planets.Count;i++)
        {
            planets[i].name += ":EnemyAI";
            planets[i].AddComponent<AIController>();
        }
    }

      
    public void SaveGame()
    {
        GameState gameState = new GameState();
        foreach (GameObject planet in planets)
        {
            if (planet == null) continue;
            PlanetState planetState = planet.GetComponent<PlanetManager>().getCurrentPlanetState();
            gameState.planetsStates.Add(planetState);
        }
        string path = Path.Combine(Application.persistentDataPath, gameConfig.fileName);
        File.WriteAllText(path, JsonUtility.ToJson(gameState, true));
        Debug.Log("Game State Succesfully saved on: " + Application.persistentDataPath + " path.");
        UIManager.instance.savedFileExist = true;
    }

    public GameObject GetRocketOfType(RocketType rocketType)
    {
        return gameConfig.rocketPrefabs[(int)rocketType];
    }
    public Material GetMaterialWithIndex(int matIndex)
    {
        return gameConfig.planetMaterials[matIndex];
    }
    public float GetRocketReloadCooldown(RocketType rocketType)
    {
       return gameConfig.rocketPrefabs[(int)rocketType].GetComponent<RocketManager>().reloadCooldown;
    } 

    public void planetDestroyed(bool isPlayer)
    {
        if (isPlayer)
        {
            SoundManager.instance.playGameOver(false); 
            gameStatus = GameStatus.Finished;
            StartCoroutine(finishAfter(0.5f, false));
        }
        else enemiesCount--;
        if (enemiesCount == 0)
        {
            SoundManager.instance.playGameOver(true); 
            gameStatus = GameStatus.Finished;
            StartCoroutine(finishAfter(0.5f, true)); 
        } 
    }

    IEnumerator finishAfter(float t,bool playerWon)
    {
        yield return new WaitForSeconds(t); 
        HUDManager.instance.FinishGame(playerWon);
    } 
}
