using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UserDataOutfit
{
    public Dictionary<int, int> equippedSkinId = new Dictionary<int, int>();
    public Dictionary<int, List<int>> ownedSkins = new Dictionary<int, List<int>>();

    private void Save()
    {
        string json = JsonConvert.SerializeObject(this);
        PlayerPrefs.SetString(UserData.KEY_PREF_USER_DATA_OUTFIT, json);
        PlayerPrefs.Save();
    }

    public int GetEquippedSkin(SkinTabType type)
    {
        int key = (int)type;
        if (equippedSkinId.ContainsKey(key))
        {
            return equippedSkinId[key];
        }

        return 0;
    }

    public List<int> GetOwnedSkins(SkinTabType type)
    {
        int key = (int)type;
        if(ownedSkins.ContainsKey(key))
        {
            return ownedSkins[key];
        }
        return new List<int>();
    }

    public void Buy(SkinTabType type, int id)
    {
        List<int> owned = GetOwnedSkins(type);
        if (owned.Contains(id)==false)
        {
            owned.Add(id);
            ownedSkins[(int)type] = owned;
            Save();
        }

    }

    public void Equip(SkinTabType type, int id)
    {   
        int typeInt = (int)type;

        if (ownedSkins.ContainsKey(typeInt) && ownedSkins[typeInt].Contains(id))
        {
            equippedSkinId[typeInt] = id;
            Save();
        }
    }
}
