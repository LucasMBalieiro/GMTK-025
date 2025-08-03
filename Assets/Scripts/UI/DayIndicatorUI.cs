using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class DayIndicatorUI : MonoBehaviour
{
    [SerializeField] private RectTransform box;
    private TextMeshProUGUI dayText;
    
    private void Start()
    {
        dayText = this.GetComponent<TextMeshProUGUI>();

        var gm = GameManager.Instance;
        dayText.text = $"Day {gm.CurrentDay:D2}\n";
        dayText.text += $"Job: {RoleParser.ParseRole(gm.CurrentRole)}";

        var seq = DOTween.Sequence();
        seq.Append(box.DOMoveY(transform.position.y - box.rect.height/2, 1f)
                .SetEase(Ease.OutBounce));
        seq.AppendInterval(4f);
        seq.Append(box.DOMoveY(transform.position.y + box.rect.height/2, .5f)
            .SetEase(Ease.InBounce));
        seq.AppendCallback(() => box.gameObject.SetActive(false));
    }
}
