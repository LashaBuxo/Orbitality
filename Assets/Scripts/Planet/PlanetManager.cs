using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class PlanetManager : MonoBehaviour
{
    [Header("Visual Parameters")]
    public Image healthBar;
    public ParticleSystem ring; 
    public GameObject explosionEffect;

    [NonSerialized]
    public PlanetStatus currentStatus=PlanetStatus.notInitialized;

    [NonSerialized]
    public float rocketReloadCooldown;
    [NonSerialized]
    public float currentReloadCooldown = 0;

    private Color controllerColor = Color.red;
     
    private float currentHP; 
      
    private GameObject sunObj;
    private float orbitRadius;

    private RocketType rocketType;
    private GameObject rocketObj;  
    private int planetMatIndex;
     
    public PlanetState getCurrentPlanetState()
    {
        return new PlanetState(rocketType,
                           planetMatIndex,
                          controllerColor, 
                       transform.position, 
                                currentHP, 
                    currentReloadCooldown);
    }
     
    public void Initialize(PlanetState planetState)
    { 
        this.transform.position = planetState.pos;
        this.planetMatIndex = planetState.matIndex; 
        this.rocketType = planetState.rocketType;
        this.controllerColor = planetState.contrCol;
        this.currentHP = planetState.curHP;
        this.currentReloadCooldown = planetState.curRT; 
        updatePlanetValues();

    }

    private void updatePlanetValues()
    {
        sunObj = GameObject.FindWithTag("Sun");
        if (sunObj == null)
        {
            Debug.LogError("Ooops, Sun not found.");
        }

        orbitRadius = Vector3.Distance(transform.position, sunObj.transform.position);
         
        healthBar.color = controllerColor;
        var ringMainPs = ring.main;
        ringMainPs.startColor = controllerColor;
        updateHealthBar();

        rocketReloadCooldown = GameManager.instance.GetRocketReloadCooldown(rocketType);
        updateRocketReloadingCooldown();

        this.GetComponent<MeshRenderer>().material = GameManager.instance.GetMaterialWithIndex(planetMatIndex); 
    }
    
    void Update()
    {
        if (currentStatus==PlanetStatus.notInitialized) return;

        UpdatePlanet();

        updateRocketReloadingCooldown();

        TryReloadingRocket();

        UpdateRocket();  
    } 

    public void UpdatePlanet()
    {
        float Angle = GameManager.instance.gameConfig.planetRotationSpeed* Time.deltaTime*360/orbitRadius  ;
        
        Vector3 vec = transform.position - sunObj.transform.position;
        transform.position= Quaternion.AngleAxis(Angle, Vector3.forward) * vec; 
    } 

    public void updateRocketReloadingCooldown()
    {
        if (currentReloadCooldown < 0)
        {
            currentStatus = PlanetStatus.readyToShoot;
        }
        else
        {
            currentReloadCooldown -= Time.deltaTime;
            currentStatus = PlanetStatus.reloading;
        }
    }
     
    public void TryReloadingRocket()
    {
        if (currentStatus != PlanetStatus.readyToShoot || rocketObj != null) return; //rocket can't be loaded
        rocketObj = Instantiate(GameManager.instance.GetRocketOfType(rocketType), transform);
        rocketDirection = rocketObj.transform.position - transform.position;
    }


    public void UpdateRocket()
    {
        if (rocketObj == null) return; //Rocket is not loaded

        Vector3 currentVector = (rocketObj.transform.position - transform.position).normalized;
        float degree = Vector3.SignedAngle(currentVector, rocketDirection, Vector3.forward);
        rocketObj.transform.RotateAround(transform.position, Vector3.forward, degree);
    }

    //must be colled from controller
    public void Command_Shoot()
    { 
        if (GameManager.gameStatus != GameStatus.Running || currentStatus != PlanetStatus.readyToShoot) return;
        if (rocketObj.GetComponent<RocketManager>().Initialized == false) return;
        if (rocketObj == null)
        { 
            Debug.LogError("Ooops, Rocket not found.");
            return;
        }
        currentStatus = PlanetStatus.reloading;
        rocketObj.GetComponent<RocketManager>().ShootRequested();
        currentReloadCooldown = rocketReloadCooldown;
        rocketObj.transform.parent = null;
        rocketObj = null;
    }

    //must be colled from controller
    [NonSerialized]
    public Vector2 rocketDirection; 
    public void Command_SetRocketRotation(Vector3 rocketLookTarget)
    {
        if (GameManager.gameStatus != GameStatus.Running) return;
        this.rocketDirection =(rocketLookTarget- transform.position).normalized; 
    }

    private void updateHealthBar()
    {
        healthBar.fillAmount = currentHP / GameManager.instance.gameConfig.planetMaxHP;

        if (currentHP == 0)
            DestroyPlanet(); 
    }
      
    private void DestroyPlanet()
    {
        SoundManager.instance.playPlanetFail();
        GameObject sfx = Instantiate(explosionEffect);
        sfx.transform.position = transform.GetChild(0).position;
        Destroy(sfx, sfx.GetComponent<ParticleSystem>().main.duration);
        GameManager.instance.PlanetDestroyed(IsPlayer());
        Destroy(gameObject);
    }

    //Must be called from Rocket which exploded on planet 
    public void DamageReceived()
    {
        Debug.Log("Damage Received on: " + transform.name);
        currentHP = Mathf.Max(0, currentHP - GameManager.instance.gameConfig.rocketDamage);
        updateHealthBar();
    }

    //Must be called from Rocket which was hit in ReadyMode
    public void rocketDestroyed()
    {
        currentStatus = PlanetStatus.reloading; 
        currentReloadCooldown = rocketReloadCooldown;
        rocketObj = null;
    }
     
    public bool IsPlayer()
    {
        return name.Contains("Player");
    }
}
