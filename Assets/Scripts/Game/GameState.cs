using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//These variables declares state of Game
//Except Rockets which Flies during state

[Serializable]
public class GameState {
    public GameState( )
    { 
        planetsStates = new List<PlanetState>();
    } 
    public List<PlanetState> planetsStates; // Current States of Planets: 0 - playerPlanet, other - Enemies.
}
