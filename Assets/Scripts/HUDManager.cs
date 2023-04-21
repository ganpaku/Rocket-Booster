using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{

    [SerializeField] Slider fuelMeter;

    public Slider GetFuelMeter()
    {
        return fuelMeter;
    }

    void Awake()
    {        
        int numGameSessions = FindObjectsOfType<HUDManager>().Length;
        if (numGameSessions > 1)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    void Update()
    {
        
    }

    public void ResetFuel()
    {
        fuelMeter.value = fuelMeter.maxValue;
    }


}
