using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip rocketExplosion;
    public AudioClip planetExplosion;
    public AudioClip rocketLaunch;

    public AudioClip gameWon;
    public AudioClip gameLost;

    public AudioClip tap;

    public static SoundManager instance;
    public void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    } 

    public void playTap()
    {
        playAudio(tap);
    }
    public void playLaunch()
    {
        playAudio(rocketLaunch);
    }
    public void playRocketFail()
    {
        playAudio(rocketExplosion);
    }
    public void playPlanetFail()
    {
        playAudio(planetExplosion);
    }
    public void playGameOver(bool playerWon)
    {
        if (playerWon)
            playAudio(gameWon);
        else
            playAudio(gameLost);
    } 


    public void playAudio(AudioClip clip)
    {
        GameObject audioObj = new GameObject("audioObj");
        AudioSource audio= audioObj.AddComponent<AudioSource>();
        audio.clip = clip;
        audio.Play();
        Destroy(audioObj, clip.length);
    }
}
