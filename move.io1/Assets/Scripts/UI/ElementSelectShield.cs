using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElementSelectShield : MonoBehaviour
{
    public Button button;
    public Image icon;
    public GameObject objHighlight;

    public ShieldId id;
    public void Load(ShieldData data)
    {
        icon.sprite = data.icon;
        id = data.shieldId;
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
        FindObjectOfType<ViewSelectShield>().Select(id);
    }
}
