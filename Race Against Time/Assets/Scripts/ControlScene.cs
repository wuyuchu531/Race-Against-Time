using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ControlScene : MonoBehaviour
{
    public Slider volumeSlider;
    private AudioSource backgroundMusic;

    private const string VolumePrefKey = "AudioVolume";

    private void Start()
    {
        //load the default volume (0.5)
        float savedVolume = PlayerPrefs.GetFloat(VolumePrefKey, 0.5f);
        backgroundMusic = GetComponent<AudioSource>();
        backgroundMusic.volume = savedVolume;
        volumeSlider.value = savedVolume;
    }

    //set volume
    public void SetVolume(float volume)
    {
        backgroundMusic.volume = volume;
        PlayerPrefs.SetFloat(VolumePrefKey, volume);
    }


    //store the data that we want to persist between scenes
    public static ControlScene Instance;

    private void Awake()
    { 
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }


    //go back to game scene
    public void GoBack()
    {
        SceneManager.LoadScene(0);
    }

}
