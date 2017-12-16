using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class GUIController : MonoBehaviour
{
    public Text GenCounter;
    public Text TimeCounter;
    public Text BestTime;
    private int currentGen = 0;
    public float time;
    

    void Update()
    {
        time += Time.deltaTime;
        TimeCounter.text = "Time: " + time;
    }

    public void ResetTime()
    {
        time = 0;
    }

    public void IncrementCurrentGen()
    {
        currentGen++;
        GenCounter.text = "Generation: " + currentGen;
    }

    public float GetCurrentBestTime()
    {
        return HelperFunc.ParseTimeFromGUI(BestTime.text);
    }

    public void SetCurrentBestTime(float time)
    {
        BestTime.text = time.ToString();
    }
}
