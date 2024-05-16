using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{

    [SerializeField] Slider _laughMeter;
    [SerializeField] Animator BossAnimator;
    [SerializeField] AudioSource BossAudioSource;
    public bool isSoundPlaying = false;

    

    private void Update()
    {
        if (_laughMeter.value < 65)
        {
            BossAnimator.SetBool("Angry", true);
            if (isSoundPlaying) { BossAudioSource.Pause(); isSoundPlaying = false; }
            BossAnimator.SetBool("Happy", false);
        }
        else
        {
            BossAnimator.SetBool("Angry", false);

            if (!isSoundPlaying) { BossAudioSource.Play(0); isSoundPlaying = true; }
            
            BossAnimator.SetBool("Happy", true);
        }
    }
}
