using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class IdleManager : MonoBehaviour
{
    [HideInInspector]
    public int length,strength,offlineEarnings,lengthCost,strengthCost,offlineEarningsCost,wallet,totalGain;
    private int[] costs=new int[]
    {
        120,
        151,
        197,
        250,
        324,
        414,
        537,
        687,
        892,
        1145,
        1468,
        1911,
        2479,
        3196,
        4148,
        5359,
        6954,
        9000,
        11687,
    };
    public static IdleManager instance;
    void Awake()
    {
        if(IdleManager.instance)
        {
            Destroy(gameObject);
        }
        else
        {
            IdleManager.instance = this;
        }

        length=-PlayerPrefs.GetInt("Length",30);
        strength=PlayerPrefs.GetInt("Strength",3);
        offlineEarnings=PlayerPrefs.GetInt("Offline",3);
        lengthCost=costs[-length/10-3];
        strengthCost=costs[strength-3];
        offlineEarningsCost=costs[offlineEarnings-3];
        wallet=PlayerPrefs.GetInt("Wallet",0);
    }

    private void OnApplicationPause(bool paused)
    {
        if(paused)
        {
            DateTime dateTime=DateTime.Now;
            PlayerPrefs.SetString("Date",dateTime.ToString());
            print(dateTime.ToString());
        }
        else
        {
            string @string=PlayerPrefs.GetString("Date",string.Empty);
            if(@string!=string.Empty)
            {
                DateTime dateTime=DateTime.Parse(@string);
                totalGain=(int)((DateTime.Now -dateTime).TotalMinutes * offlineEarnings +1.0);
                print(totalGain);
                ScreensManager.instance.ChangeScreen(Screens.RETURN); 
            }
        }
    }
    private void OnApplicationQuit()
    {
        OnApplicationPause(true);
    }
    public void BuyLength()
    {
        length-=10;
        wallet-=lengthCost;
        lengthCost=costs[-length/10-3];
        PlayerPrefs.SetInt("Length",-length);
        PlayerPrefs.SetInt("Wallet",wallet);
        ScreensManager.instance.ChangeScreen(Screens.MAIN); 
    }
    public void BuyStrength()
    {
        strength++;
        wallet-=strengthCost;
        strengthCost=costs[strength-3];
        PlayerPrefs.SetInt("Strength",strength);
        PlayerPrefs.SetInt("Wallet",wallet);
        ScreensManager.instance.ChangeScreen(Screens.MAIN); 
    }
    public void BuyOfflineEarnings()
    {
        offlineEarnings++;
        wallet-=offlineEarningsCost;
        offlineEarningsCost=costs[offlineEarnings-3];
        PlayerPrefs.SetInt("Offline",offlineEarnings);
        PlayerPrefs.SetInt("Wallet",wallet);
        ScreensManager.instance.ChangeScreen(Screens.MAIN); 
    }

    public void CollectMoney()
    {
        wallet+=totalGain;
        PlayerPrefs.SetInt("Wallet",wallet);
        ScreensManager.instance.ChangeScreen(Screens.MAIN); 
    }
}
