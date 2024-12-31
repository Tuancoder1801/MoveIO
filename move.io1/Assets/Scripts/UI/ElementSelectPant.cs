using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElementSelectPant : MonoBehaviour
{
    public Button button;
    public Image icon;
    public GameObject objHighlight;

    public PantId id;

    public void Load(PantData data)
    {
        icon.sprite = data.icon;
        id = data.pantId;
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
        FindObjectOfType<ViewSelectPant>().Select(id);
    }
}
