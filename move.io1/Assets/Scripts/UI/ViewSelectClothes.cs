using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ViewSelectClothes : MonoBehaviour
{
    public Transform content;
    public ElementSelectClothes elementSelectClothes;

    public Button btBuyClothes;
    public Button btEquipClothes;

    public Text textGold;
    public Text textPrice;
    public Text textEquip;
    public Text textEquipped;

    private ClothesId selectingId;
    private List<ElementSelectClothes> clothes = new List<ElementSelectClothes>();
    private List<int> ownedClothes = new List<int>();

    private void Awake()
    {
        ownedClothes = UserData.outfit.GetOwnedSkins(SkinTabType.Set);
        btBuyClothes.onClick.AddListener(BuyClothes);
        btEquipClothes.gameObject.SetActive(false);

        CreateClothes();
    }

    private void OnEnable()
    {
        if (ownedClothes.Contains(UserData.outfit.GetEquippedSkin(SkinTabType.Set)))
        {
            Select((ClothesId)UserData.outfit.GetEquippedSkin(SkinTabType.Set));
        }
        else
        {
            //selectingId = ClothesId.NONE;
            Select(clothes[0].id);
        }
    }

    private void CreateClothes()
    {
        for (int i = 0; i < GameDataConstant.clothes.Count; i++)
        {
            ClothesData clothesData = GameDataConstant.clothes[i];
            ElementSelectClothes selectClothes = Instantiate(elementSelectClothes, content);
            selectClothes.Load(clothesData);
            clothes.Add(selectClothes);
        }
    }

    public void Select(ClothesId id)
    {
        if (selectingId != id)
        {
            selectingId = id;
            Highlight();
            UpdateClothesInfo();
        }
    }

    private void Highlight()
    {
        for (int i = 0; i < clothes.Count; i++)
        {
            clothes[i].SetHighlight(selectingId == clothes[i].id);
        }
    }

    private void UpdateClothesInfo()
    {
        for (int i = 0; i < GameDataConstant.clothes.Count; i++)
        {
            ClothesData clothesData = GameDataConstant.clothes[i];

            if (clothesData.clothesId == selectingId)
            {
                textGold.text = clothesData.gold.ToString() + "% Gold";

                if (ownedClothes.Contains((int)clothesData.clothesId))
                {
                    btBuyClothes.gameObject.SetActive(false);
                    btEquipClothes.gameObject.SetActive(true);

                    if ((int)clothesData.clothesId == UserData.outfit.GetEquippedSkin(SkinTabType.Set))
                    {
                        textEquipped.gameObject.SetActive(true);
                        textEquip.gameObject.SetActive(false);
                    }
                    else
                    {
                        textEquip.gameObject.SetActive(true);
                        textEquipped.gameObject.SetActive(false);
                    }
                }
                else
                {
                    btBuyClothes.gameObject.SetActive(true);
                    btEquipClothes.gameObject.SetActive(false);
                    textPrice.text = clothesData.price.ToString();

                    if (UIGamePlayManager.Instance.coins.currentCoins >= clothesData.price)
                    {
                        textPrice.color = Color.white;
                        btBuyClothes.enabled = true;
                    }
                    else
                    {
                        textPrice.color = Color.red;
                        btBuyClothes.enabled = false;
                    }
                }

                UIGamePlayManager.Instance.ChangeCharacter(clothesData.clothesId);
            }
        }
    }
    private void BuyClothes()
    {
        for (int i = 0; i < GameDataConstant.clothes.Count; i++)
        {
            ClothesData clothesData = GameDataConstant.clothes[i];
            if (clothesData.clothesId == selectingId)
            {
                int price = GameDataConstant.clothes[i].price;

                if (UIGamePlayManager.Instance.coins.currentCoins >= price)
                {
                    UIGamePlayManager.Instance.coins.SpendCoins(price);

                    if (!ownedClothes.Contains((int)clothesData.clothesId))
                    {
                        ownedClothes.Add((int)clothesData.clothesId);
                        btBuyClothes.gameObject.SetActive(false);
                        btEquipClothes.gameObject.SetActive(true);

                        UserData.outfit.Buy(SkinTabType.Set, (int)clothesData.clothesId);
                        EquipClothes();
                    }
                }
            }
        }
    }

    private void EquipClothes()
    {
        for (int i = 0; i < GameDataConstant.clothes.Count; i++)
        {
            ClothesData clothesData = GameDataConstant.clothes[i];

            if (clothesData.clothesId == selectingId)
            {
                UIGamePlayManager.Instance.player.EquipClothes(clothesData.clothesId);
                textEquipped.gameObject.SetActive(true);
                textEquip.gameObject.SetActive(false);

                UserData.outfit.Equip(SkinTabType.Set, (int)clothesData.clothesId);
                UpdateClothesInfo();
            }
        }
    }

    
    
}
