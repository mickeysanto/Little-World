using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimeGUI : MonoBehaviour
{
    public TextMeshProUGUI timeText;

    private void OnEnable() {
        TimeManager.OnSecondChanged += UpdateTime;
        TimeManager.OnMinuteChanged += UpdateTime;
        TimeManager.OnHourChanged += UpdateTime;
    }

    private void OnDisable() {
        TimeManager.OnSecondChanged -= UpdateTime;
        TimeManager.OnMinuteChanged -= UpdateTime;
        TimeManager.OnHourChanged -= UpdateTime;
    }

    private void UpdateTime() {
        timeText.text = $"{TimeManager.Hour.ToString("00")}:{TimeManager.Minute.ToString("00")}:{TimeManager.Second.ToString("00")}";
    }
}
