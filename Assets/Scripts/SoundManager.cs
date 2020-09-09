using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour {

    public AudioClip globalMusic;
    public AudioClip getPoints;
    public AudioClip mergeBlocks;
    public AudioClip clickMenu;
    public AudioClip endGame;

    private AudioSource aSource;
    public AudioSource musicSource;

    public AudioMixer audioMixer;

    private bool isread = false;

    public Slider master;
    public Slider effect;
    public Slider music;

    private void Awake()
    {
        aSource = GetComponent<AudioSource>();

        if (musicSource != null) {
            musicSource.clip = globalMusic;
            musicSource.Play();
        }

       // Debug.Log(PlayerPrefs.GetFloat("musicV"));

    }

    public void Start()
    {
        if (PlayerPrefs.HasKey("masterV"))
        {
            master.value = PlayerPrefs.GetFloat("masterV");
            audioMixer.SetFloat("masterV", PlayerPrefs.GetFloat("masterV"));
        }

        if (PlayerPrefs.HasKey("effectV"))
        {
            effect.value = PlayerPrefs.GetFloat("effectV");
            audioMixer.SetFloat("effectV", PlayerPrefs.GetFloat("effectV"));
        }

        if (PlayerPrefs.HasKey("musicV"))
        {
            music.value = PlayerPrefs.GetFloat("musicV");
            audioMixer.SetFloat("musicV", PlayerPrefs.GetFloat("musicV"));
        }


        isread = true;
    }


    public void PlayGetPoints()
    {
        aSource.PlayOneShot(getPoints);
    }

    public void PlayMergeBlocks()
    {
        aSource.PlayOneShot(mergeBlocks);
    }

    public void PlayClickMenu()
    {
        aSource.PlayOneShot(clickMenu);
    }

    public void PlayEndGame()
    {
        aSource.PlayOneShot(endGame);
    }

    public void OnChangeV()
    {
        if (!isread)
            return;

        audioMixer.SetFloat("masterV" , master.value);
        audioMixer.SetFloat("effectV" , effect.value);
        audioMixer.SetFloat("musicV" , music.value);

        PlayerPrefs.SetFloat("masterV" , master.value);
        PlayerPrefs.SetFloat("effectV", effect.value);
        PlayerPrefs.SetFloat("musicV", music.value);

        PlayerPrefs.Save();


    }
}
