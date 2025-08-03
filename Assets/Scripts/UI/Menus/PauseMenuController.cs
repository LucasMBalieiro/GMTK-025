using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour
{
    [SerializeField] private GameObject[] UIElements;
    [SerializeField] private GameObject PauseMenu;
    [SerializeField] private GameObject OptionsMenu;
    
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    
    private bool _isPaused = false;
    
    private void Start()
    {
        musicSlider.onValueChanged.AddListener(AudioManager.Instance.SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(AudioManager.Instance.SetSFXVolume);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(_isPaused) ResumeGame();
            else PauseGame();
        }
    }

    public void PauseGame()
    {
        foreach (var element in UIElements)
        {
            element.GetComponent<Canvas>().enabled = false;
        }
        PauseMenu.SetActive(true);
        _isPaused = true;
    }

    public void ResumeGame()
    {
        _isPaused = false;
        PauseMenu.SetActive(false);
        OptionsMenu.SetActive(false);
        foreach (var element in UIElements)
        {
            element.GetComponent<Canvas>().enabled = true;
        }
    }

    public void OpenOptionsMenu()
    {
        musicSlider.value = AudioManager.Instance.GetMusicVolume();
        sfxSlider.value = AudioManager.Instance.GetSFXVolume();
        
        AudioManager.Instance.PlaySFX("Machine_Grab");
        PauseMenu.SetActive(false);
        OptionsMenu.SetActive(true);
    }

    public void OpenPauseMenu()
    {
        AudioManager.Instance.PlaySFX("Machine_Grab");
        PauseMenu.SetActive(true);
        OptionsMenu.SetActive(false);
    }

    public void QuitGame()
    {
        AudioManager.Instance.PlaySFX("Machine_Grab");
        SceneManager.LoadScene(0);
    }
}
