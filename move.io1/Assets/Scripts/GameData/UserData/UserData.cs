using Newtonsoft.Json;
using System.Collections;
using System.Xml.Linq;
using UnityEngine;


public static class UserData
{
    public static UserDataWeapon weapon;
    public static UserDataOutfit outfit;
    public static UserDataCoins coins;

    public const string KEY_PREF_USER_DATA_WEAPON = "user_data_weapon";
    public const string KEY_PREF_USER_DATA_OUTFIT = "user_data_outfit";
    public const string KEY_PREF_USER_DATA_COINS = "user_data_coins";

    public static void Load()
    {
        if (weapon == null)
        {
            string weaponPrefs = PlayerPrefs.GetString(KEY_PREF_USER_DATA_WEAPON);
            if (string.IsNullOrEmpty(weaponPrefs))
            {
                weapon = new UserDataWeapon();
                weapon.Initialize();
            }
            else
            {
                weapon = JsonConvert.DeserializeObject<UserDataWeapon>(weaponPrefs);
            }

            Debug.Log("weapon=" + JsonConvert.SerializeObject(weapon));
        }

        if (outfit == null)
        {
            string outfitPrefs = PlayerPrefs.GetString(KEY_PREF_USER_DATA_OUTFIT);
            if (string.IsNullOrEmpty(outfitPrefs))
            {
                outfit = new UserDataOutfit();
            }
            else
            {
                outfit = JsonConvert.DeserializeObject<UserDataOutfit>(outfitPrefs);
            }

            Debug.Log("outfit=" + JsonConvert.SerializeObject(outfit));
        }

        /*if (coins == null)
        {
            string coinsPrefs = PlayerPrefs.GetString(KEY_PREF_USER_DATA_COINS);

            if (string.IsNullOrEmpty(coinsPrefs))
            {
                coins = new UserDataCoins();
            }
            else
            {
                coins = JsonConvert.DeserializeObject<UserDataCoins>(coinsPrefs);
            }

            Debug.Log("Coins=" + JsonConvert.SerializeObject(coins));
        }*/

        if (coins == null)
        {
            coins = new UserDataCoins(); // Khởi tạo UserDataCoins nếu chưa được tạo
        }
    }
}
