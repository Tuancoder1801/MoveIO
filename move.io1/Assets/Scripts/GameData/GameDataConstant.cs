using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class GameDataConstant
{
    public static List<WeaponData> weapons;

    public static List<HatData> hats;

    public static List<PantData> pants;

    public static List<ClothesData> clothes;

    public static List<ShieldData> shields;

    public static void Load()
    {
        if(weapons == null)
        {
            weapons = Resources.LoadAll<WeaponData>("Weapons").ToList();
        }

        if(hats == null)
        {
            hats = Resources.LoadAll<HatData>("Hats").ToList() ;
        }

        if(pants == null)
        {
            pants = Resources.LoadAll<PantData>("Pants").ToList();
        }

        if(clothes == null) 
        {
            clothes = Resources.LoadAll<ClothesData>("Clothes").ToList();
        }

        if(shields == null)
        {
            shields = Resources.LoadAll<ShieldData>("Shields").ToList();
        }
    }
}
