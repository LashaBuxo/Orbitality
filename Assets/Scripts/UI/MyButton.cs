using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
public class MyButton : MonoBehaviour
{
    public Color highlightBack;
    public Color normalBack;
     
     
     
    public void onPointerEnter()
    { 
        BackLight(true);
        SoundManager.instance.playTap();
    }
    public void onPointerExit()
    { 
        BackLight(false);
    }
 
    
    private void BackLight(bool up)
    {
        if (up)
            GetComponent<Image>().color = highlightBack;
        else
            GetComponent<Image>().color = normalBack;
    }
    

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
