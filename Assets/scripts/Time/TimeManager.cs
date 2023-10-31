using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TimeManager : MonoBehaviour
{
    public static Action OnSecondChanged;
    public static Action OnMinuteChanged;
    public static Action OnHourChanged;

    public static int Second { get; private set; }
    public static int Minute { get; private set; }
    public static int Hour { get; private set; }

    private float SecondToRealTime = 1f;
    private float timer;

    void Start()
    {
        Minute = 0;
        Hour = 0;
        Second = 0;
        timer = SecondToRealTime;
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if(timer <=0) {
            Second++;
            OnSecondChanged?.Invoke();

            if(Second >= 60) {
                Second = 0;
                Minute++;
                OnMinuteChanged?.Invoke();

                if(Minute >= 60) {
                    Minute = 0;
                    Hour++;
                    OnHourChanged?.Invoke();
                }
            }
            timer = SecondToRealTime;
        }
    }

    public int getHour()
    {
        return Hour;
    }
}
