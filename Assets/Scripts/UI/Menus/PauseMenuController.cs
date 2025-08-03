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

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        foreach (var element in UIElements)
        {
            element.SetActive(false);
        }
        PauseMenu.SetActive(true);
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        PauseMenu.SetActive(false);
        foreach (var element in UIElements)
        {
            element.SetActive(true);
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
