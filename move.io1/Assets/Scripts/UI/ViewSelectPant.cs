using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ViewSelectPant : MonoBehaviour
{
    public ElementSelectPant elementSelectPant;
    public Transform content;

    public Button btBuyPant;
    public Button btEquipPant;

    public Text textSpeed;
    public Text textPrice;
    public Text textEquip;
    public Text textEquipped;

    private PantId selectingId;
    private List<ElementSelectPant> pants = new List<ElementSelectPant>();
    private List<int> ownedPants = new List<int>();

    private void Awake()
    {
        ownedPants = UserData.outfit.GetOwnedSkins(SkinTabType.Pant);

        btBuyPant.onClick.AddListener(BuyPant);
        btEquipPant.gameObject.SetActive(false);

        CreatePants();
    }

    private void OnEnable()
    {
        if (ownedPants.Contains(UserData.outfit.GetEquippedSkin(SkinTabType.Pant)))
        {
            Select((PantId)UserData.outfit.GetEquippedSkin(SkinTabType.Pant));
        }
        else
        {
            selectingId = PantId.NONE;
            Select(pants[0].id);
        }
    }

    private void CreatePants()
    {
        for (int i = 0; i < GameDataConstant.pants.Count; i++)
        {
            PantData pantData = GameDataConstant.pants[i];
            ElementSelectPant selectPant = Instantiate(elementSelectPant, content);
            selectPant.Load(pantData);
            pants.Add(selectPant);
        }
    }

    public void Select(PantId id)
    {
        if (selectingId != id)
        {
            selectingId = id;
            Highlight();
            UpdatePantInfo();
            UIGamePlayManager.Instance.player.EquipPant(selectingId);
        }
    }

    private void Highlight()
    {
        for (int i = 0; i < pants.Count; i++)
        {
            pants[i].SetHighlight(selectingId == pants[i].id);
        }
    }

    private void UpdatePantInfo()
    {
        for (int i = 0; i < GameDataConstant.pants.Count; i++)
        {
            PantData pantData = GameDataConstant.pants[i];

            if (pantData.pantId == selectingId)
            {
                textSpeed.text = pantData.speed.ToString() + "% Move Speed";

                if (ownedPants.Contains((int)pantData.pantId))
                {
                    btBuyPant.gameObject.SetActive(false);
                    btEquipPant.gameObject.SetActive(true);

                    if ((int)pantData.pantId == UserData.outfit.GetEquippedSkin(SkinTabType.Pant))
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
                    btBuyPant.gameObject.SetActive(true);
                    btEquipPant.gameObject.SetActive(false);
                    textPrice.text = pantData.price.ToString();

                    if (UIGamePlayManager.Instance.coins.currentCoins >= pantData.price)
                    {
                        textPrice.color = Color.white;
                        btBuyPant.enabled = true;
                    }
                    else
                    {
                        textPrice.color = Color.red;
                        btBuyPant.enabled = false;
                    }
                }
            }
        }
    }

    private void BuyPant()
    {
        for (int i = 0; i < GameDataConstant.pants.Count; i++)
        {
            PantData pantData = GameDataConstant.pants[i];
            if (pantData.pantId == selectingId)
            {
                int price = GameDataConstant.pants[i].price;

                if (UIGamePlayManager.Instance.coins.currentCoins >= price)
                {
                    UIGamePlayManager.Instance.coins.SpendCoins(price);

                    if (!ownedPants.Contains((int)pantData.pantId))
                    {
                        ownedPants.Add((int)pantData.pantId);
                        btBuyPant.gameObject.SetActive(false);
                        btEquipPant.gameObject.SetActive(true);

                        UserData.outfit.Buy(SkinTabType.Pant, (int)pantData.pantId);
                        EquipPant();
                    }
                }
            }
        }
    }

    private void EquipPant()
    {
        for (int i = 0; i < GameDataConstant.pants.Count; i++)
        {
            PantData pantData = GameDataConstant.pants[i];

            if (pantData.pantId == selectingId)
            {
                UIGamePlayManager.Instance.player.EquipPant(pantData.pantId);
                textEquipped.gameObject.SetActive(true);
                textEquip.gameObject.SetActive(false);

                UserData.outfit.Equip(SkinTabType.Pant ,(int)pantData.pantId);
                //UpdatePantInfo();
            }
        }
    }
}
