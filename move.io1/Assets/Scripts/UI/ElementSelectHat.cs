using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ElementSelectHat : MonoBehaviour
{
    public Button button;
    public Image icon;
    public GameObject objHighlight;
    public GameObject iconLock;

    public HatId id;

    public void Load(HatData data)
    {
        id = data.hatId;
        icon.sprite = data.icon;
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
        FindObjectOfType<ViewSelectHat>().Select(id);
    }
}
