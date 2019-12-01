using UnityEngine;
using System;

//These variables declares state of planet
[Serializable]
public class PlanetState {  
     
    public PlanetState(RocketType rocketType,int matIndex, Color contrCol,Vector3 pos,float curHP,float curRT )
    {
        this.rocketType = rocketType;
        this.matIndex = matIndex;
        this.contrCol = contrCol;
        this.pos = pos;
        this.curHP = curHP;
        this.curRT = curRT; 
    }

    public RocketType rocketType; // Type of Rocket this planet owns 
    public int matIndex; //Planet Material 
    public Color contrCol; //Controller Color of this planet
    public Vector3 pos; //Planet Position (in current state)  
    public float curHP; //Current HP of this planet (in current state)
    public float curRT; //Current Rocket Reload Time of this planet  (in current state)
     
}
