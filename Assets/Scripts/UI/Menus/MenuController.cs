using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu, options, howToPlay, credits;

    private void Start() {
        howToPlay.SetActive(false);
        credits.SetActive(false);
        options.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void StartGame(){
        SceneManager.LoadScene(1);
    }
    public void Menu(){
        SceneManager.LoadScene(0);
    }
    public void Quit(){
        Application.Quit();
    }

    public void BackToMain(){
        howToPlay.SetActive(false);
        credits.SetActive(false);
        options.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void OpenHowToPlay(){
        credits.SetActive(false);
        mainMenu.SetActive(false);
        options.SetActive(false);
        howToPlay.SetActive(true);
    }

    public void OpenCredits(){
        howToPlay.SetActive(false);
        mainMenu.SetActive(false);
        options.SetActive(false);
        credits.SetActive(true);
    }

    public void OpenOptions() {
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