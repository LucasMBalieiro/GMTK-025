using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu, options, howToPlay, credits;
    
    [SerializeField] private Slider sliderMusic, SliderSFX;

    private void Start() {
        howToPlay.SetActive(false);
        credits.SetActive(false);
        options.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void StartGame(){
        AudioManager.Instance.PlaySFX("Machine_Grab");
        SceneManager.LoadScene(1);
    }
    public void Menu(){
        AudioManager.Instance.PlaySFX("Machine_Grab");
        SceneManager.LoadScene(0);
    }
    public void Quit(){
        AudioManager.Instance.PlaySFX("Machine_Grab");
        Application.Quit();
    }

    public void BackToMain(){
        AudioManager.Instance.PlaySFX("Machine_Grab");
        howToPlay.SetActive(false);
        credits.SetActive(false);
        options.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void OpenHowToPlay(){
        AudioManager.Instance.PlaySFX("Machine_Grab");
        credits.SetActive(false);
        mainMenu.SetActive(false);
        options.SetActive(false);
        howToPlay.SetActive(true);
    }

    public void OpenCredits(){
        AudioManager.Instance.PlaySFX("Machine_Grab");
        howToPlay.SetActive(false);
        mainMenu.SetActive(false);
        options.SetActive(false);
        credits.SetActive(true);
    }

    public void OpenOptions() {
        sliderMusic.value = AudioManager.Instance.GetMusicVolume();
        SliderSFX.value = AudioManager.Instance.GetSFXVolume();
        AudioManager.Instance.PlaySFX("Machine_Grab");
        howToPlay.SetActive(false);
        mainMenu.SetActive(false);
        credits.SetActive(false);
        options.SetActive(true);
    }
    
    public void OnMusicVolumeChanged(float volume)
    {
        AudioManager.Instance.SetMusicVolume(volume);
    }

    public void OnSFXVolumeChanged(float volume)
    {
        AudioManager.Instance.SetSFXVolume(volume);
    }
}