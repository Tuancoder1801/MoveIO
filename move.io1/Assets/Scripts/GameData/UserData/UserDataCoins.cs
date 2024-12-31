using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserDataCoins
{
    /*public UserDataCoins()
    {
        currentCoins = 10000;
        GetCoins(currentCoins);
    }*/

    public void SaveCoins(float currentCoins)
    {
        PlayerPrefs.SetFloat(UserData.KEY_PREF_USER_DATA_COINS, currentCoins);
        PlayerPrefs.Save();
    }

    public float LoadCoins()
    {
        return PlayerPrefs.GetFloat(UserData.KEY_PREF_USER_DATA_COINS, 0);
    }
}
