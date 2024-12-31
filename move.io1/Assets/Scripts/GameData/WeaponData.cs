using UnityEngine;

[CreateAssetMenu(fileName = "Weapon-", menuName = "ScriptableObjects/WeaponData")]
public class WeaponData : BaseSkinData
{
    public WeaponId weaponId;
    public int damage;
}

