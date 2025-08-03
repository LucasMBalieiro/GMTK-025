using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DayEndUI : MonoBehaviour
{
    [SerializeField] private Image foodAnimation;

    [SerializeField] private TextMeshProUGUI levelIndicator;
    [SerializeField] private Image xpBar;
    
    [SerializeField] private TextMeshProUGUI levelUpMessage;

    public void OpenUI()
    {
        var gm = GameManager.Instance;
        levelUpMessage.gameObject.SetActive(false);
        
        foodAnimation.transform.DORotate(Vector3.forward * -360f, 5f, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Incremental);

        levelIndicator.text = $"{gm.CurrentLevel}";
        xpBar.fillAmount = gm.CurrentXp / gm.xpThreshold;
        
        var (leveledUp, promoted) = gm.GainXp();

        if (leveledUp)
        {
            var levelUpAnim = DOTween.Sequence();
            levelUpAnim.Append(xpBar.DOFillAmount(1f, 1f));
            levelUpAnim.Append(xpBar.DOFillAmount(0f, 0.2f));
            levelUpAnim.AppendCallback(() =>
            {
                levelIndicator.text = $"{gm.CurrentLevel}";
                levelUpMessage.text = $"Congratulations!!!\n";
                if (promoted)
                    levelUpMessage.text += $"Promoted to {RoleParser.ParseRole(gm.CurrentRole)}";
                
                levelUpMessage.gameObject.SetActive(true);
            });
            levelUpAnim.Append(xpBar.DOFillAmount(gm.CurrentXp / gm.xpThreshold, 1f)
                .SetEase(Ease.InCirc));
        }
        else
        { 
            xpBar.DOFillAmount(gm.CurrentXp / gm.xpThreshold, 1f)
                .SetEase(Ease.InCirc);
        }
    }
}
