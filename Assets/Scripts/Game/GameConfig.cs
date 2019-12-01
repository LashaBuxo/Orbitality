using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "GameConfiguration", menuName = "GameConfiguration", order = 1)]
public class GameConfig : ScriptableObject
{
    [Header("Player Parameters")]
    public int maxEnemyPlayers = 4;
    public Color playerColor = Color.green;
    public Color enemyColor = Color.red;
     
    [Header("Planet Parameters")] 
    public GameObject planetPrefab;
    public List<Material> planetMaterials;
    public float planetMaxHP = 100;
    public float gravityPower = 30;
    public float gravityRange = 30; 
    public float planetRotationSpeed = 3;  
    public float distanceBetweenOrbits = 10;


    [Header("Rocket Parameters")]
    public float rocketDamage = 15;
    public float destroyRocketsAfter = 10;
    public List<GameObject> rocketPrefabs;

    //Game States are saved on path: Application.persistentDataPath
    [Header("Saved Data Parameters")]
    public string fileName = "gameState.json"; 
} 
