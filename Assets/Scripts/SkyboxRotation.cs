using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxRotation : MonoBehaviour
{  
    void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time/2);
    }
}
