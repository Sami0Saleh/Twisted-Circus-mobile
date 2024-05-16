using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] AudioSource MusicPlayer;
    [SerializeField] AudioSource PlayerSound;
    [SerializeField] AudioSource BossSounds;

    [SerializeField] AudioClip StartMusic;
    [SerializeField] AudioClip LoopMusic;

    private void Update()
    {
        if (MusicPlayer.isPlaying == false) { MusicPlayer.clip = LoopMusic; MusicPlayer.Play(0); MusicPlayer.loop = true; }
    }

}
