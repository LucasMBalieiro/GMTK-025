using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderUI : MonoBehaviour
{
    [SerializeField] private Image orderImage;
    [SerializeField] private Image progressBar;
    [SerializeField] private GameObject doneMessage;

    private float _cookingDuration;

    public void InitUI(ItemData data)
    {
        orderImage.sprite = data.itemSprite;
        progressBar.fillAmount = 0f;
        doneMessage.SetActive(false);
    }
    
    public void StartProgress(float cookTime)
    {
        _cookingDuration = cookTime; 
        StartCoroutine(UpdateCookingProgress());
    }

    private IEnumerator UpdateCookingProgress()
    {
        var currentProgress = 0f;
        while (currentProgress < _cookingDuration)
        {
            currentProgress += Time.deltaTime;
            
            progressBar.fillAmount = currentProgress / _cookingDuration;
            if (currentProgress >= _cookingDuration)
            {
                doneMessage.SetActive(true);
                AudioManager.Instance.PlaySFX("Order");
                break;
            }
            
            yield return null;
        }
    }
}
