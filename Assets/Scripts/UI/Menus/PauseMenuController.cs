using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    [SerializeField] private GameObject[] UIElements;
    [SerializeField] private GameObject PauseMenu;
    [SerializeField] private GameObject OptionsMenu;
    
    private bool _isPaused = false;

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
        PauseMenu.SetActive(false);
        OptionsMenu.SetActive(true);
    }

    public void OpenPauseMenu()
    {
        PauseMenu.SetActive(true);
        OptionsMenu.SetActive(false);
    }

    public void QuitGame()
    {
        SceneManager.LoadScene(0);
    }
}
