using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DayIndicatorUI : MonoBehaviour
{
    private TextMeshProUGUI dayText;
    
    private void Start()
    {
        dayText = this.GetComponent<TextMeshProUGUI>();

        var gm = GameManager.Instance;
        dayText.text = $"Day {gm.CurrentDay:D2}\n";
        dayText.text += $"Job: {RoleParser.ParseRole(gm.CurrentRole)}";
    }
}
