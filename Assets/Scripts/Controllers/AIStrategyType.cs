using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum AIStrategyType
{ 
    
    random, //Rocket Direction is random and shoots instantly 
    shootClosest, //Rocket is always towards to the closest planet, and will shoot whenever it is in Range
    shootPlayer,  //Shoots Rocket in direction of player instantly 
    numberOfStrategies //Simple way to have countity of strategies in ENUM
}  
