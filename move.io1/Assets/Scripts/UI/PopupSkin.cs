using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupSkin : MonoBehaviour
{
    public Button btExit;
    public GameObject[] views;
    public ButtonTab[] tabs;

    private SkinTabType curTab = SkinTabType.Hat;

    private void Awake()
    {
        btExit.onClick.AddListener(ClickExit);
    }

    private void OnEnable()
    {
        ShowCurrentView();

    }

    public void ShowView(SkinTabType tab)
    {
        if (curTab != tab)
        {
            curTab = tab;
            ShowCurrentView();
        }
    }

    private void ShowCurrentView()
    {
        for (int i = 0; i < views.Length; i++)
        {
            views[i].SetActive(i == (int)curTab);
        }

        HighlightTab();
        UpdateSkin();
    }

    private void HighlightTab()
    {
        for (int i = 0; i < tabs.Length; i++)
        {
            tabs[i].SetHighlightTab(i == (int)curTab);
        }
    }

    private void ClickExit()
    {
        UIGamePlayManager.Instance.player.EquipHat((HatId)UserData.outfit.GetEquippedSkin(SkinTabType.Hat));
        UIGamePlayManager.Instance.player.EquipPant((PantId)UserData.outfit.GetEquippedSkin(SkinTabType.Pant));
        UIGamePlayManager.Instance.player.EquipShield((ShieldId)UserData.outfit.GetEquippedSkin(SkinTabType.Shield));
        UIGamePlayManager.Instance.ChangeCharacter((ClothesId)UserData.outfit.GetEquippedSkin(SkinTabType.Set));
        gameObject.SetActive(false);
    }

    private void UpdateSkin()
    {   
        if(curTab != SkinTabType.Set)
        {
            UIGamePlayManager.Instance.ChangeCharacter(ClothesId.NONE);

            if(curTab == SkinTabType.Hat)
            {
                if(UserData.outfit.GetOwnedSkins(SkinTabType.Hat).Contains(UserData.outfit.GetEquippedSkin(SkinTabType.Hat)))
                {
                    UIGamePlayManager.Instance.player.EquipHat((HatId)UserData.outfit.GetEquippedSkin(SkinTabType.Hat));
                }
                else
                {
                    UIGamePlayManager.Instance.player.EquipHat(HatId.CAP);
                }
            }
            else if(curTab == SkinTabType.Pant)
            {
                if (UserData.outfit.GetOwnedSkins(SkinTabType.Pant).Contains(UserData.outfit.GetEquippedSkin(SkinTabType.Pant)))
                {
                    UIGamePlayManager.Instance.player.EquipPant((PantId)UserData.outfit.GetEquippedSkin(SkinTabType.Pant));
                }
                else
                {
                    UIGamePlayManager.Instance.player.EquipPant(PantId.BATMAN);
                }
            }
            else if(curTab == SkinTabType.Shield)
            {
                if (UserData.outfit.GetOwnedSkins(SkinTabType.Shield).Contains(UserData.outfit.GetEquippedSkin(SkinTabType.Shield)))
                {
                    UIGamePlayManager.Instance.player.EquipShield((ShieldId)UserData.outfit.GetEquippedSkin(SkinTabType.Shield));
                }
                else
                {
                    UIGamePlayManager.Instance.player.EquipShield(ShieldId.BLACK);
                }
            }
        }
        else
        {
            UIGamePlayManager.Instance.player.EquipHat(HatId.NONE);
            UIGamePlayManager.Instance.player.EquipPant(PantId.NONE);
            UIGamePlayManager.Instance.player.EquipShield(ShieldId.NONE);

            if (UserData.outfit.GetOwnedSkins(SkinTabType.Set).Contains(UserData.outfit.GetEquippedSkin(SkinTabType.Set)))
            {
                UIGamePlayManager.Instance.ChangeCharacter((ClothesId)UserData.outfit.GetEquippedSkin(SkinTabType.Set));
            }
            else
            {
                UIGamePlayManager.Instance.ChangeCharacter(ClothesId.ANGLE);
            }
        }

        for (int i = 0; i < views.Length; i++)
        {
            views[i].SetActive(i == (int)curTab);
        }
    }
}
