using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Upgrades : MonoBehaviour
{
    public PlayerController player;
    public GameObject can;
    public  Slider slider;

    private void Update()
    {
        if (slider.value >= 100)
        {
            can.SetActive(true);
            Time.timeScale = 0; // pause game to select boost
        }
    }

    public void DamageBoost()
    {
        player.BoostPlayerDamage();
        can.SetActive(false);
        Time.timeScale = 1; // resume game
        slider.value = 50; // xp reset
    }

    public void SpeedBoost()
    {
        player.BoostPlayerSpeed();
        can.SetActive(false);
        Time.timeScale = 1; // resume game
        slider.value = 50; // xp reset
    }

    public void CritChance()
    {
        player.BoostPlayerCrit();
        can.SetActive(false);
        Time.timeScale = 1; // resume game
        slider.value = 50; // xp reset
    }


}