using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Reward : MonoBehaviour
{
    public Image icon;

    private void Awake()
    {
        
    }

    void Start()
    {
        GetReward();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void GetReward()
    {
        List<HatData> unownedHats = GameDataConstant.hats.FindAll(
        hat => !UserData.outfit.GetOwnedSkins(SkinTabType.Hat).Contains((int)hat.hatId));

        if (unownedHats.Count > 0)
        {
            HatData selectedHat = unownedHats[UnityEngine.Random.Range(0, unownedHats.Count)];

            icon.sprite = selectedHat.icon;

            UserData.outfit.Buy(SkinTabType.Hat, (int)selectedHat.hatId);
        }
    }

}
