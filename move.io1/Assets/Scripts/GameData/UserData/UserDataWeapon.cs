using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserDataWeapon
{
    public int equippedId;
    public List<int> ownedWeapons = new List<int>();

    public void Initialize()
    {
        for (int i = 0; i < GameDataConstant.weapons.Count; i++)
        {
            var wData = GameDataConstant.weapons[i];
            if (wData.weaponId == WeaponId.ARROW)
            {
                Buy(wData.weaponId);
                Equip(wData.weaponId);
            }
        }
    }

    private void Save()
    {
        string json = JsonConvert.SerializeObject(this);
        PlayerPrefs.SetString(UserData.KEY_PREF_USER_DATA_WEAPON, json);
        PlayerPrefs.Save();
    }

    public void Buy(WeaponId id)
    {
        int idInt = (int)id;

        if (ownedWeapons.Contains(idInt) == false)
        {
            ownedWeapons.Add(idInt);
            Save();
        }
    }

    public void Buy(int id)
    {
        Buy((WeaponId)id);
    }

    public void Equip(int weaponId)
    {
        if (ownedWeapons.Contains(weaponId))
        {
            equippedId = weaponId;
            Save();
        }
    }

    public void Equip(WeaponId id)
    {
        Equip((int)id);
    }
}
