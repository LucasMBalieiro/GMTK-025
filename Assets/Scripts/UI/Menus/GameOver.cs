using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI indicatorText;

    private void Start()
    {
        indicatorText.DOFade(0f, 2f)
            .SetLoops(-1, LoopType.Yoyo);
    }

    public void BackToMenu()
    {
        GameManager.DestroyInstance();
        SceneManager.LoadScene("menu");
    }
}
