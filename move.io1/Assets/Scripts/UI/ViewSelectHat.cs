using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ViewSelectHat : MonoBehaviour
{
    public Transform content;
    public ElementSelectHat elementSelectHat;

    public Button btBuyHat;
    public Button btEquipHat;

    public Text textRange;
    public Text textPrice;
    public Text textEquip;
    public Text textEquipped;

    private HatId selectingId;
    private List<ElementSelectHat> hats = new List<ElementSelectHat>();

    private List<int> ownedHats = new List<int>();

    private void Awake()
    {
        ownedHats = UserData.outfit.GetOwnedSkins(SkinTabType.Hat);

        btBuyHat.onClick.AddListener(BuyHat);
        btEquipHat.onClick.AddListener(EquipHat);

        CreateHats();
    }

    private void OnEnable()
    {
        if (ownedHats.Contains(UserData.outfit.GetEquippedSkin(SkinTabType.Hat)))
        {
            Select((HatId)UserData.outfit.GetEquippedSkin(SkinTabType.Hat));
        }
        else
        {
            selectingId = HatId.NONE;
            Select(hats[0].id);
        }
    }

    private void CreateHats()
    {
        for (int i = 0; i < GameDataConstant.hats.Count; i++)
        {
            HatData hatData = GameDataConstant.hats[i];
            ElementSelectHat selectHat = Instantiate(elementSelectHat, content);

            selectHat.Load(hatData);
            hats.Add(selectHat);
        }
    }

    public void Select(HatId id)
    {
        if (selectingId != id)
        {
            selectingId = id;
            Highlight();
            UpdateHatInfo();
        }
    }

    private void Highlight()
    {
        for (int i = 0; i < hats.Count; i++)
        {
            hats[i].SetHighlight(selectingId == hats[i].id);
        }
    }

    private void UpdateHatInfo()
    {
        for (int i = 0; i < GameDataConstant.hats.Count; i++)
        {
            HatData hatData = GameDataConstant.hats[i];

            if (hatData.hatId == selectingId)
            {
                textRange.text = hatData.range.ToString() + "% Range";

                if (ownedHats.Contains((int)hatData.hatId))
                {
                    btBuyHat.gameObject.SetActive(false);
                    btEquipHat.gameObject.SetActive(true);

                    if ((int)hatData.hatId == UserData.outfit.GetEquippedSkin(SkinTabType.Hat))
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
                    btBuyHat.gameObject.SetActive(true);
                    btEquipHat.gameObject.SetActive(false);
                    textPrice.text = hatData.price.ToString();

                    if (UIGamePlayManager.Instance.coins.currentCoins >= hatData.price)
                    {
                        textPrice.color = Color.white;
                        btBuyHat.enabled = true;
                    }
                    else
                    {
                        textPrice.color = Color.red;
                        btBuyHat.enabled = false;
                    }
                }


                UIGamePlayManager.Instance.player.EquipHat(hatData.hatId);
            }
        }
    }

    private void BuyHat()
    {
        for (int i = 0; i < GameDataConstant.hats.Count; i++)
        {
            HatData hatData = GameDataConstant.hats[i];
            if (hatData.hatId == selectingId)
            {
                int price = GameDataConstant.hats[i].price;

                if (UIGamePlayManager.Instance.coins.currentCoins >= price)
                {
                    UIGamePlayManager.Instance.coins.SpendCoins(price);

                    if (!ownedHats.Contains((int)hatData.hatId))
                    {
                        ownedHats.Add((int)hatData.hatId);
                        btBuyHat.gameObject.SetActive(false);
                        btEquipHat.gameObject.SetActive(true);

                        UserData.outfit.Buy(SkinTabType.Hat, (int)hatData.hatId);
                        EquipHat();
                    }
                }
            }
        }
    }

    private void EquipHat()
    {
        for (int i = 0; i < GameDataConstant.hats.Count; i++)
        {
            HatData hatData = GameDataConstant.hats[i];

            if (hatData.hatId == selectingId)
            {
                UIGamePlayManager.Instance.player.EquipHat(hatData.hatId);
                textEquipped.gameObject.SetActive(true);
                textEquip.gameObject.SetActive(false);

                UserData.outfit.Equip(SkinTabType.Hat, (int)hatData.hatId);
                //UpdateHatInfo();
            }
        }
    }
}
