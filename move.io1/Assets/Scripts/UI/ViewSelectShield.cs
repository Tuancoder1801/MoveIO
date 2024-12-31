using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ViewSelectShield : MonoBehaviour
{
    public ElementSelectShield elementSelectShield;
    public Transform content;

    public Button btBuyShield;
    public Button btEquipShield;

    public Text textGold;
    public Text textPrice;
    public Text textEquip;
    public Text textEquipped;

    private ShieldId selectingId;
    private List<ElementSelectShield> shields = new List<ElementSelectShield>();
    private List<int> ownedShields = new List<int>();

    private void Awake()
    {
        ownedShields = UserData.outfit.GetOwnedSkins(SkinTabType.Shield);

        btBuyShield.onClick.AddListener(BuyShield);
        btEquipShield.gameObject.SetActive(false);

        CreateShields();
    }

    private void OnEnable()
    {
        if (ownedShields.Contains(UserData.outfit.GetEquippedSkin(SkinTabType.Shield)))
        {
            Select((ShieldId)UserData.outfit.GetEquippedSkin(SkinTabType.Shield));
        }
        else
        {
            selectingId = ShieldId.NONE;
            Select(shields[0].id);
        }
    }

    private void CreateShields()
    {
        for (int i = 0; i < GameDataConstant.shields.Count; i++)
        {
            ShieldData shieldData = GameDataConstant.shields[i];
            ElementSelectShield selectShield = Instantiate(elementSelectShield, content);
            selectShield.Load(shieldData);
            shields.Add(selectShield);
        }
    }

    public void Select(ShieldId id)
    {
        if (selectingId != id)
        {
            selectingId = id;
            Highlight();
            UpdateShieldInfo();
            UIGamePlayManager.Instance.player.EquipShield(selectingId);
        }
    }

    private void Highlight()
    {
        for (int i = 0; i < shields.Count; i++)
        {
            shields[i].SetHighlight(selectingId == shields[i].id);
        }
    }

    private void UpdateShieldInfo()
    {
        for (int i = 0; i < GameDataConstant.shields.Count; i++)
        {
            ShieldData shieldData = GameDataConstant.shields[i];

            if (shieldData.shieldId == selectingId)
            {
                textGold.text = shieldData.gold.ToString() + "% Gold";

                if (ownedShields.Contains((int)shieldData.shieldId))
                {

                    btBuyShield.gameObject.SetActive(false);
                    btEquipShield.gameObject.SetActive(true);

                    if ((int)shieldData.shieldId == UserData.outfit.GetEquippedSkin(SkinTabType.Shield))
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
                    btBuyShield.gameObject.SetActive(true);
                    btEquipShield.gameObject.SetActive(false);
                    textPrice.text = shieldData.price.ToString();

                    if (UIGamePlayManager.Instance.coins.currentCoins >= shieldData.price)
                    {
                        textPrice.color = Color.white;
                        btBuyShield.enabled = true;
                    }
                    else
                    {
                        textPrice.color = Color.red;
                        btBuyShield.enabled = false;
                    }
                }
            }
        }
    }

    private void BuyShield()
    {
        for (int i = 0; i < GameDataConstant.shields.Count; i++)
        {
            ShieldData shieldData = GameDataConstant.shields[i];
            if (shieldData.shieldId == selectingId)
            {
                int price = GameDataConstant.shields[i].price;

                if (UIGamePlayManager.Instance.coins.currentCoins >= price)
                {
                    UIGamePlayManager.Instance.coins.SpendCoins(price);

                    if (!ownedShields.Contains((int)shieldData.shieldId))
                    {
                        ownedShields.Add((int)shieldData.shieldId);
                        btBuyShield.gameObject.SetActive(false);
                        btEquipShield.gameObject.SetActive(true);

                        UserData.outfit.Buy(SkinTabType.Shield, (int)shieldData.shieldId);
                        EquipShield();
                    }
                }
            }
        }
    }

    private void EquipShield()
    {
        for (int i = 0; i < GameDataConstant.shields.Count; i++)
        {
            ShieldData shieldData = GameDataConstant.shields[i];

            if (shieldData.shieldId == selectingId)
            {
                UIGamePlayManager.Instance.player.EquipShield(shieldData.shieldId);
                textEquipped.gameObject.SetActive(true);
                textEquip.gameObject.SetActive(false);

                UserData.outfit.Buy(SkinTabType.Shield, (int)shieldData.shieldId);
                //UpdateShieldInfo();
            }
        }
    }



}
