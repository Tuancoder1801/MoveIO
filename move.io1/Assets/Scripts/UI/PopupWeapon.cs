using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupWeapon : MonoBehaviour
{
    public WeaponData[] weaponData;

    public Text textName;
    public Text textDamage;
    public Text textPrice;
    public Text textEquip;
    public Text textEquipped;
    public Image imageWeapon;


    public Button btExit;
    public Button btNextWeapon;
    public Button btBackWeapon;
    public Button btBuyWeapon;
    public Button btEquipWeapon;

    private int currentWeapon = 0;
    private List<int> ownedWeapons = new List<int>();

    private void Awake()
    {
        ownedWeapons = UserData.weapon.ownedWeapons;

        btExit.onClick.AddListener(ClickExit);
        btNextWeapon.onClick.AddListener(NextWeapon);
        btBackWeapon.onClick.AddListener(BackWeapon);
        btBuyWeapon.onClick.AddListener(BuyWeapon);
        btEquipWeapon.onClick.AddListener(EquipWeapon);
    }

    private void Start()
    {

    }

    private void OnEnable()
    {
        currentWeapon = System.Array.FindIndex(weaponData, weapon => (int)weapon.weaponId == UserData.weapon.equippedId);
        if (currentWeapon == -1)
        {
            currentWeapon = 0;
        }

        ShowWeapons(currentWeapon);
    }

    private void ShowWeapons(int currentWeapon)
    {
        if (weaponData != null && weaponData.Length > 0 && currentWeapon < weaponData.Length)
        {
            textName.text = weaponData[currentWeapon].Name;
            textDamage.text = "+" + weaponData[currentWeapon].damage.ToString() + " Damage";

            if (ownedWeapons.Contains((int)weaponData[currentWeapon].weaponId))
            {
                btEquipWeapon.gameObject.SetActive(true);
                btBuyWeapon.gameObject.SetActive(false);

                if ((int)weaponData[currentWeapon].weaponId == UserData.weapon.equippedId)
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
                btBuyWeapon.gameObject.SetActive(true);
                btEquipWeapon.gameObject.SetActive(false);
                textPrice.text = weaponData[currentWeapon].price.ToString();

                if (UIGamePlayManager.Instance.coins.currentCoins >= weaponData[currentWeapon].price)
                {
                    textPrice.color = Color.white;
                    btBuyWeapon.enabled = true;
                }
                else
                {
                    textPrice.color = Color.red;
                    btBuyWeapon.enabled = false;
                }
            }

            imageWeapon.sprite = weaponData[currentWeapon].icon;

            UIGamePlayManager.Instance.player.EquipWeapon(weaponData[currentWeapon].weaponId);
        }
    }

    private void ClickExit()
    {

        UIGamePlayManager.Instance.player.EquipWeapon((WeaponId)UserData.weapon.equippedId);

        gameObject.SetActive(false);
    }

    public void NextWeapon()
    {
        currentWeapon = (currentWeapon + 1) % weaponData.Length;
        ShowWeapons(currentWeapon);
    }

    public void BackWeapon()
    {
        currentWeapon = (currentWeapon - 1 + weaponData.Length) % weaponData.Length;
        ShowWeapons(currentWeapon);
    }

    private void BuyWeapon()
    {
        int price = weaponData[currentWeapon].price;

        if (UIGamePlayManager.Instance.coins.currentCoins >= price)
        {
            UIGamePlayManager.Instance.coins.SpendCoins(price);

            int weaponId = (int)weaponData[currentWeapon].weaponId;
            if (!ownedWeapons.Contains(weaponId))
            {
                ownedWeapons.Add(weaponId);
                btBuyWeapon.gameObject.SetActive(false);
                btEquipWeapon.gameObject.SetActive(true);

                UserData.weapon.Buy((WeaponId)weaponId);
                EquipWeapon();
            }
        }
    }

    private void EquipWeapon()
    {
        int equippedWeaponId = (int)weaponData[currentWeapon].weaponId;
        UIGamePlayManager.Instance.player.EquipWeapon((WeaponId)equippedWeaponId);
        textEquipped.gameObject.SetActive(true);
        textEquip.gameObject.SetActive(false);

        UserData.weapon.Equip(equippedWeaponId);
        ShowWeapons(currentWeapon);
    }
}