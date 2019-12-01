using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MySlider : MonoBehaviour
{
    public int value = 1;
    public bool isMax = false;
    public Text numericUI;
    public Slider unitySlider;
    public MySlider pairedMySlider;

     
    void Start()
    {
         
        updateValue();
    }

    public void onValueChanged()
    {
        updateValue();
        if (isMax)
        {
            pairedMySlider.unitySlider.value = Mathf.Min(value, pairedMySlider.unitySlider.value);
        }
        else
        {
            pairedMySlider.unitySlider.value = Mathf.Max(value, pairedMySlider.unitySlider.value);
        }
        pairedMySlider.updateValue();
    }
    public void updateValue()
    {
        this.value = (int)unitySlider.value;
        numericUI.text = this.value.ToString();
    }
}
