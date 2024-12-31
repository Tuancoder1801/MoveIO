using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElementSelectClothes : MonoBehaviour
{
    public Button button;
    public Image icon;
    public GameObject objHighlight;

    public ClothesId id;
    public void Load(ClothesData data)
    {
        icon.sprite = data.icon;
        id = data.clothesId;
    }

    private void Start()
    {
        button.onClick.AddListener(OnClick);
    }

    public void SetHighlight(bool isOn)
    {
        objHighlight.SetActive(isOn);
    }

    private void OnClick()
    {
        FindObjectOfType<ViewSelectClothes>().Select(id);
    }
}
