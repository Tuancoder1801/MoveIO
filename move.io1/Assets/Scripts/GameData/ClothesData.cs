using UnityEngine;

[CreateAssetMenu(fileName = "Clothes-", menuName = "ScriptableObjects/ClothesData")]
public class ClothesData : ScriptableObject
{
    public ClothesId clothesId;
    public Sprite icon;
    public int gold;
    public int price;
}

