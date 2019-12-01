using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    AIStrategyType currentStrategy;
    float currentRandomDelay;
    private PlanetManager planetManager;

    private bool isCompletingStrategy = false;

    private void chooseStrategyRandomly()
    {
        currentStrategy = (AIStrategyType)Random.Range(0, (int)AIStrategyType.numberOfStrategies);
        currentRandomDelay = Random.Range(0, 3.0f);
        isCompletingStrategy = false; 
    }

    void Start()
    {
        planetManager = GetComponent<PlanetManager>();
        chooseStrategyRandomly();
    }
      
    void Update()
    {
        //planetManager.Command_SetRocketRotation(virtWorldPos);
        //planetManager.Command_Shoot();
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


    //////////////////////////////////////////////////
    Vector2 randomLookDirection=Vector2.zero;
    float rotSpeed = 100;
    void Strategy_RandomShoot()
    {
        if (isCompletingStrategy == false)
        {
            randomLookDirection = GetRandomVector(); 
        }

        Vector3 curDir = planetManager.rocketDirection; 
  
        Vector3 newDir;
        if (Vector3.SignedAngle(curDir, randomLookDirection, Vector3.forward) < 0)
            newDir = Quaternion.Euler(Time.deltaTime* rotSpeed, Time.deltaTime * rotSpeed, Time.deltaTime * rotSpeed) * curDir;
        else
            newDir = Quaternion.Euler(-Time.deltaTime * rotSpeed, -Time.deltaTime * rotSpeed, -Time.deltaTime * rotSpeed) * curDir;

        planetManager.Command_SetRocketRotation(newDir + transform.position);

        waitAndShoot();
    }

    //////////////////////////////////////////////////

    float ShootRange =20;
    float t = 0;
    float waitLimit = 5;
    void Strategy_ClosestShoot()
    {
        t += Time.deltaTime;
        Vector3 closestPlanetPos = getClosestAlivePlanetPos(); 
        planetManager.Command_SetRocketRotation(closestPlanetPos);  
        float range = Vector3.Distance(transform.position, closestPlanetPos); 
        if (t > waitLimit)
        {
            chooseStrategyRandomly();
            t = 0;
        }
        if (range < ShootRange)
        {    
            planetManager.Command_Shoot();
            chooseStrategyRandomly();
        } 
    }

    //////////////////////////////////////////////////
    void Strategy_PlayerShoot()
    {
        Vector3 playerPos = getPlayerPlanetPos();
        planetManager.Command_SetRocketRotation(playerPos); 

        waitAndShoot();
    }
    //////////////////////////////////////////////////

    private void waitAndShoot()
    {
        if (isCompletingStrategy == false)
        {
            isCompletingStrategy = true;
            StartCoroutine(shootAfter(currentRandomDelay));
        }
    }
    IEnumerator shootAfter(float delay)
    {
        yield return new WaitForSeconds(delay);
        planetManager.Command_Shoot();
        chooseStrategyRandomly();
    }


    public List<GameObject> getAlivePlanets()
    {
        List<GameObject> planets = new List<GameObject>();
        planets.AddRange(GameObject.FindGameObjectsWithTag("Planet"));
        return planets;
    }


    public Vector3 getPlayerPlanetPos()
    {
        List<GameObject> planets = getAlivePlanets();
        foreach (GameObject planet in planets)
        {
            if (planet.GetComponent<PlanetManager>().IsPlayer())
                return planet.transform.position;
        }

        Debug.LogError("Oops, Player Planer not found.");
        return Vector3.zero;
    }
    public Vector3 getClosestAlivePlanetPos()
    {
        List<GameObject> planets = getAlivePlanets();

        float minDist = 0;
        Vector3 closestPlanet = Vector3.zero; 
        foreach (GameObject planet in planets)
        {
            if (planet.name != gameObject.name)
            { 
                float dist = Vector3.Distance(transform.position, planet.transform.position);
                if (minDist == 0 || minDist > dist)
                {
                    minDist = dist;
                    closestPlanet = planet.transform.position;
                }
            }
        }
        return closestPlanet;
    }

    public Vector3 GetRandomVector()
    {
        return new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0);
    }
}
