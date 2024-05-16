using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LaughMeter : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    public Slider slider;
    private float checkNumber;

    
    //public void SetMaxLaugh(int laugh)
    //{
    //    slider.maxValue = laugh;
    //    slider.value = laugh;
    //}

    public void gainLaugh(float laugh)
    {
    slider.value += laugh;
    }

    public void loseLaugh(float laugh)
    {
        checkNumber = slider.value - laugh;
        if (checkNumber <= 0) { slider.value -= laugh; playerController.GameEnd(); }
        else { slider.value -= laugh; }
    }
}
