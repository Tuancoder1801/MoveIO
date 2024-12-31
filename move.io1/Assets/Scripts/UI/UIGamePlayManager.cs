using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGamePlayManager : Singleton<UIGamePlayManager>
{
    public Button btPopupWeapon;
    public PopupWeapon popupWeapon;
    public Button btPopupSkin;
    public PopupSkin popupSkin;

    public Player player;
    public Character[] characters;

    public Coins coins;

    private void Awake()
    {
        GameDataConstant.Load();
        UserData.Load();
        btPopupWeapon.onClick.AddListener(ClickBtPopupWeapon);
        btPopupSkin.onClick.AddListener(ClickBtPopupSkin);

    }

    private void Start()
    {
        ChangeCharacter(ClothesId.NONE);

        if (UserData.outfit.GetOwnedSkins(SkinTabType.Set).Contains(UserData.outfit.GetEquippedSkin(SkinTabType.Set)))
        {
            ChangeCharacter((ClothesId)UserData.outfit.GetEquippedSkin(SkinTabType.Set));
        }
    }

    private void Update()
    {

        if (Input.GetKeyUp(KeyCode.G))
        {
            coins.AddCoins(10000);
        }
    }

    private void ClickBtPopupWeapon()
    {
        popupWeapon.gameObject.SetActive(true);
    }

    private void ClickBtPopupSkin()
    {
        popupSkin.gameObject.SetActive(true);
    }

    public void ChangeCharacter(ClothesId clothesId)
    {
        for (int i = 0; i < characters.Length; i++)
        {
            if (characters[i].clothesPrefabs != null && characters[i].clothesPrefabs.clothesId == clothesId)
            {
                player = characters[i] as Player;
                player.gameObject.SetActive(true);

                for (int j = 0; j < characters.Length; j++)
                {
                    if (characters[j] != player)
                    {
                        characters[j].gameObject.SetActive(false);

                        if (!UserData.outfit.GetOwnedSkins(SkinTabType.Hat).Contains(UserData.outfit.GetEquippedSkin(SkinTabType.Hat)))
                        {
                            characters[j].EquipHat(HatId.NONE);
                        }

                        if (!UserData.outfit.GetOwnedSkins(SkinTabType.Pant).Contains(UserData.outfit.GetEquippedSkin(SkinTabType.Pant)))
                        {
                            characters[j].EquipPant(PantId.NONE);
                        }

                        if (!UserData.outfit.GetOwnedSkins(SkinTabType.Shield).Contains(UserData.outfit.GetEquippedSkin(SkinTabType.Shield)))
                        {
                            characters[j].EquipShield(ShieldId.NONE);
                        }
                    }
                }

                return;
            }
        }
    }


}