using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ButtonTab : MonoBehaviour
{
    public SkinTabType type;
    public Button tab;
    public GameObject highLightTab;

    private void Start()
    {
        tab.onClick.AddListener(OnClicḳ);
    }

    public void SetHighlightTab(bool isOn)
    {
        highLightTab.SetActive(isOn);
    }

    private void OnClicḳ()
    {
        // Goi ham show view cua PopupSkin
        FindObjectOfType<PopupSkin>().ShowView(type);
    }
}
