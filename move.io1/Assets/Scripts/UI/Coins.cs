using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Coins : MonoBehaviour
{
    public float currentCoins = 0;

    public Text textCoins;

    private void Start()
    {
        float currentCoins = UserData.coins.LoadCoins();
        textCoins.text = currentCoins.ToString();


        Debug.Log(currentCoins + " " + textCoins.ToString());
    }


    private void Update()
    {
    }

    public void AddCoins(int amount)
    {
        if (amount < 0)
            return;

        currentCoins += amount;
        textCoins.text = currentCoins.ToString();

        UserData.coins.SaveCoins(currentCoins);
    }

    public void SpendCoins(int amount)
    {
        if (amount < 0)
            return;

        if (currentCoins >= amount)
        {
            currentCoins -= amount;
            textCoins.text = currentCoins.ToString();
            UserData.coins.SaveCoins(currentCoins);
        }
    }
}
