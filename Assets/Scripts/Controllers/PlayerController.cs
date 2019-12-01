using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlanetManager planetManager;
    void Start()
    {
        planetManager = GetComponent<PlanetManager>();
    }

    //Detects if mouse covers Menu Button to ignore that kind of commands near to the GUI
    private bool MouseOvereGui(Vector2 pos)
    {
        pos = new Vector2(pos.x/Screen.width , pos.y/Screen.height)-Vector2.one/2;
        return (pos.x * pos.x + pos.y + pos.y > 1 && pos.x < 0 && pos.y > 0);
    }
     
    void Update()
    {
        if (MouseOvereGui(Input.mousePosition)) return;
         
        Vector3 virtWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition); 

        //Rocket Direction has changed
        planetManager.Command_SetRocketRotation(virtWorldPos); 

        //Shoot Requested
        if (Input.GetMouseButtonDown(0))
        {
            planetManager.Command_Shoot();
        }
    }
}
