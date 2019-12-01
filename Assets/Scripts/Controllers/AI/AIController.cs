using System.Collections;
using UnityEngine;

public class AIController : MonoBehaviour
{
    private AIStrategyType currentStrategy;
    private float currentRandomDelay;  
    private bool isCompletingStrategy = false; 

    private void chooseStrategyRandomly()
    {
        currentStrategy = AIHelper.getRandomAIStrategyType();
        currentRandomDelay = Random.Range(0, gameConfig.shootMaximumDelay);
        isCompletingStrategy = false; 
    }
     
    private PlanetManager planetManager;
    private GameConfig gameConfig;
    void Start()
    {
        planetManager = GetComponent<PlanetManager>();
        gameConfig = GameManager.instance.gameConfig;
         
        chooseStrategyRandomly();
    }
      
    void Update()
    { 
        if (planetManager.currentStatus == PlanetStatus.readyToShoot)
        {
            switch (currentStrategy)
            {
                case AIStrategyType.random:
                    Strategy_RandomShoot();
                    break;
                case AIStrategyType.shootClosest:
                     Strategy_ClosestShoot();
                    break;
                case AIStrategyType.shootPlayer:
                    Strategy_PlayerShoot();
                    break;
                default:
                    Debug.LogError("Oops, wrong strategy.");
                    break;
            }  
        }
    }
     
    Vector2 randomLookDirection;
    float deltaDegree = 5;
    void Strategy_RandomShoot()
    {
        if (isCompletingStrategy == false)
        {
            randomLookDirection = AIHelper.GetRandomVector(); 
        }

        Vector3 curDir = planetManager.rocketDirection; 
  
        Vector3 newDir;
        float curAngle = Vector3.SignedAngle(curDir, randomLookDirection, Vector3.forward);
        if (curAngle > deltaDegree)
        { 
            newDir = Quaternion.Euler(-Time.deltaTime * 100, -Time.deltaTime * 100, -Time.deltaTime * 100) * curDir;
            planetManager.Command_SetRocketRotation(newDir + transform.position);
        }
        if (curAngle < -deltaDegree)
        {
            newDir = Quaternion.Euler(-Time.deltaTime * 100, -Time.deltaTime * 100, -Time.deltaTime * 100) * curDir;
            planetManager.Command_SetRocketRotation(newDir + transform.position);
        }
         
        WaitAndShoot();
    }

     
    float t = 0; 
    void Strategy_ClosestShoot()
    {
        t += Time.deltaTime;
        Vector3 closestPlanetPos = AIHelper.getClosestAlivePlanetPos(transform); 
        planetManager.Command_SetRocketRotation(closestPlanetPos);  
        float range = Vector3.Distance(transform.position, closestPlanetPos); 
        if (t > gameConfig.shootMaximumDelay)
        {
            chooseStrategyRandomly();
            t = 0;
        }
        if (range < gameConfig.shootRange)
        {    
            planetManager.Command_Shoot();
            chooseStrategyRandomly();
        } 
    }
     
    void Strategy_PlayerShoot()
    {
        Vector3 playerPos =AIHelper. getPlayerPlanetPos();
        planetManager.Command_SetRocketRotation(playerPos); 

        WaitAndShoot();
    }


    ////////////////////////////////////////////////// 
    private void WaitAndShoot()
    {
        if (isCompletingStrategy == false)
        {
            isCompletingStrategy = true;
            StartCoroutine(ShootAfter(currentRandomDelay));
        }
    }

    IEnumerator ShootAfter(float delay)
    {
        yield return new WaitForSeconds(delay);
        planetManager.Command_Shoot();
        chooseStrategyRandomly();
    }
    ////////////////////////////////////////////////// 
}
