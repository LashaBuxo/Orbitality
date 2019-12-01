using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIHelper 
{

    //Returns random AIStrategyType
    public static AIStrategyType getRandomAIStrategyType()
    {
        return (AIStrategyType)Random.Range(0, (int)AIStrategyType.numberOfStrategies);
    }

    /// <summary>  
    /// Returns random Vector3 where z=0
    /// </summary>     
    public static Vector3 GetRandomVector()
    {
        return new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0);
    }
     

    /// <summary>  
    /// Returns all alive planets in the scene
    /// </summary>   
    public static List<GameObject> getAlivePlanets()
    {
        List<GameObject> planets = new List<GameObject>();
        planets.AddRange(GameObject.FindGameObjectsWithTag("Planet"));
        return planets;
    }


    /// <summary>  
    /// Returns player position in the scene  
    /// </summary>   
    public static Vector3 getPlayerPlanetPos()
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


    /// <summary>  
    /// Returns closest alive planet
    /// </summary>    
    public static Vector3 getClosestAlivePlanetPos(Transform from)
    {
        List<GameObject> planets = getAlivePlanets();

        float minDist = 0;
        Vector3 closestPlanet = Vector3.zero;
        foreach (GameObject planet in planets)
        {
            if (planet.name != from.name)
            {
                float dist = Vector3.Distance(from.position, planet.transform.position);
                if (minDist == 0 || minDist > dist)
                {
                    minDist = dist;
                    closestPlanet = planet.transform.position;
                }
            }
        }
        return closestPlanet;
    }

}
