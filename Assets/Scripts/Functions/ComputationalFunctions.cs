using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Static Class includes static functions 
/// which are used in scripts too many times
/// </summary>
public static class ComputationalFunctions  
{ 
    //Returns random rocket type
    public static RocketType getRandomRocketType()
    {
        return (RocketType)Random.Range(0, (int)RocketType.NumberOfTypes);
    }

    /// <summary>  
    /// Returns random position on orbit(circle) 
    /// </summary>    
    public static Vector2 getRandomPositionOnOrbit(int orbitIndex, int totalOrbits,float distBetwOrb)
    {
        float maxRadius = totalOrbits * distBetwOrb;
        float distToOrbit = maxRadius * (orbitIndex + 1) / (1.0f * totalOrbits);
        float x = Random.Range(-distToOrbit, distToOrbit);
        float y = Mathf.Sqrt(distToOrbit * distToOrbit - x * x);
        if (Random.Range(0, 2) == 0) y = -y;
        return new Vector2(x, y);
    }



    /// <summary>  
    /// Returns randomly chosen order list from interval l-r(exclusive r) with specific length  
    /// </summary>   
    public static List<int> getRandomOrderFromInterval(int length, int l, int r)
    {
        List<int> order = new List<int>();
        bool[] used = new bool[r];

        for (int i = 0; i < length; i++)
        {
            int randInd = Random.Range(0, r - l - i);
            int randVal = -1;

            for (int j = l; j < r; j++)
                if (used[j] == false)
                {
                    if (randInd == 0)
                    {
                        randVal = j;
                        break;
                    }
                    randInd--;
                }

            order.Add(randVal);
            used[randVal] = true;
        }

        return order;
    }

}
