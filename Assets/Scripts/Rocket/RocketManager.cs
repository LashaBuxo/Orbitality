using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class RocketManager : MonoBehaviour
{ 
    public float reloadCooldown = 1;
    public float mass=1;
    public float acceleration = 30;

    public GameObject rocketPlayer;
    public GameObject rocketEnemy;

    public GameObject explosionEffect;
    public GameObject trailEffect; 

    [NonSerialized]
    public bool isFlying = false; 
    [NonSerialized]
    public bool Initialized = false;

     
    private Rigidbody rigidBody;
    private GameConfig gameConfig;
     

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.mass = mass;
        rigidBody.constraints = RigidbodyConstraints.FreezeAll;
        trailEffect.SetActive(false);
        chooseRocketBody();
        Initialized = true;
    }
     
    private void FixedUpdate()
    {
        if (isFlying)
        {
            Vector3 curDir = (transform.GetChild(0).position - transform.position).normalized;

            float angle = Vector3.SignedAngle(curDir, rigidBody.velocity, transform.forward);
            transform.RotateAround(transform.position, transform.forward, angle); 
            rigidBody.AddForce(curDir * acceleration, ForceMode.Acceleration);

            CatchGravitationalWaves();
        }
    }

    //Finds all Planets and applies forces from the planet gravitation
    public void CatchGravitationalWaves()
    {
        List<GameObject> planets = new List<GameObject>();
        planets.AddRange(GameObject.FindGameObjectsWithTag("Planet"));
        planets.Add(GameObject.FindGameObjectWithTag("Sun"));

        foreach (GameObject planet in planets)
        {
            Vector3 forceDirection = planet.transform.position - transform.position;
            if (forceDirection.magnitude < 1) continue;
            rigidBody.AddForce(forceDirection.normalized * gameConfig.gravityPower * rigidBody.mass * forceDirection.magnitude / gameConfig.gravityRange, ForceMode.Acceleration);
        }
    }

    public void ShootRequested()
    {  
        gameConfig = GameManager.instance.gameConfig;
        SoundManager.instance.playLaunch();
        isFlying = true; 
        rigidBody.constraints = RigidbodyConstraints.None | RigidbodyConstraints.FreezePositionZ;
        Vector3 direction = (transform.GetChild(0).position - transform.position).normalized;

        rigidBody.velocity = direction * acceleration ;
        trailEffect.SetActive(true);
        Destroy(gameObject, gameConfig.destroyRocketsAfter);
    } 

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Rocket":
                if (transform.parent != null)
                    transform.parent.GetComponent<PlanetManager>().rocketDestroyed();
                destroyRocket();
                break;
            case "Planet":
                collision.gameObject.SendMessage("DamageReceived");
                destroyRocket();
                break;
            case "Sun":
                destroyRocket();
                break;
            default:
                print("Ooops, Incorrect Menu type.");
                break;
        }
    }

    public void destroyRocket()
    { 
        SoundManager.instance.playRocketFail();
        GameObject sfx = Instantiate(explosionEffect);
        sfx.transform.position = transform.GetChild(0).position;
        Destroy(sfx, sfx.GetComponent<ParticleSystem>().main.duration); 
        Destroy(gameObject);
    }  
      
    public void chooseRocketBody()
    {
        bool isPlayer = transform.parent.GetComponent<PlanetManager>().IsPlayer(); 
        rocketPlayer.SetActive(isPlayer);
        rocketEnemy.SetActive(!isPlayer); 
    } 
}
