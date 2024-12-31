using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameController : Singleton<GameController>
{
    public Canvas canvas;
    public Player[] playerPrefab;
    public Enemy enemyPrefab;

    public SmoothCameraFlow Camera;
    public Transform PlayerIndex;
    public Transform[] enemyIndex;

    public LayerMask layerMask;
    public List<Character> characters = new List<Character>();

    public bool isCountDownActive = true;
    public float timeRemaining;

    public Text textCount;
    public GameObject countDown;

    public Player playerInstance;

    public string killerName;
    public Color killerColor;

    void Start()
    {
        GameDataConstant.Load();
        UserData.Load();
        CreatePlayer();
        CreateEnemy();
    }

    #region createCharacter
    private void CreatePlayer()
    {
        if (playerPrefab != null)
        {
            if (UserData.outfit.GetOwnedSkins(SkinTabType.Set).Contains(UserData.outfit.GetEquippedSkin(SkinTabType.Set)))
            {
                for (int i = 0; i < playerPrefab.Length; i++)
                {
                    if (UserData.outfit.GetOwnedSkins(SkinTabType.Set).Contains(UserData.outfit.GetEquippedSkin(SkinTabType.Set)))
                    {

                        playerInstance = Instantiate(playerPrefab[i], PlayerIndex.position, PlayerIndex.rotation);
                        playerInstance.tag = "Player";
                        playerInstance.isLobby = false;
                        playerInstance.circle.SetActive(true);
                        playerInstance.EquipClothes((ClothesId)UserData.outfit.GetEquippedSkin(SkinTabType.Set));
                        Camera.SetTarget(playerInstance.transform);

                    }
                }
            }
            else
            {
                playerInstance = Instantiate(playerPrefab[0], PlayerIndex.position, PlayerIndex.rotation);
                playerInstance.tag = "Player";
                playerInstance.isLobby = false;
                playerInstance.circle.SetActive(true);
                //playerInstance.EquipClothes((ClothesId)UserData.outfit.GetEquippedSkin(SkinTabType.Set));
                Camera.SetTarget(playerInstance.transform);
            }

            characters.Add(playerInstance);
        }
    }

    private void CreateEnemy()
    {
        if (enemyPrefab != null)
        {
            for(int i = 0; i < UIGameManager.Instance.indicator.Length; i++)
            {
                UIGameManager.Instance.indicator[i].gameObject.SetActive(false);
            }

            for (int i = 0; i < enemyIndex.Length; i++)
            {
                Enemy enemy = Instantiate(enemyPrefab, enemyIndex[i].position, enemyIndex[i].rotation);
                enemy.tag = "Enemy1";

                characters.Add(enemy);

                if (i < UIGameManager.Instance.indicator.Length)
                {
                    UIGameManager.Instance.indicator[i].SetEnemy(enemy);
                    UIGameManager.Instance.indicator[i].gameObject.SetActive(true);   
                }
            }
        }
    }
    #endregion

    void Update()
    {
        CountDownTime();
        
        CheckEnemyStatus();
    }

    #region CountDownTime
    private void CountDownTime()
    {
        if (isCountDownActive)
        {
            if (timeRemaining > 0)
            {
                countDown.gameObject.SetActive(true);
                timeRemaining -= Time.deltaTime;
                DisplayCount(timeRemaining);
            }
            else
            {
                timeRemaining = 0;
                textCount.text = "GO!";
                isCountDownActive = false;

                Invoke(nameof(HideCountDown), 1f);
            }
        }
    }

    private void HideCountDown()
    {
        countDown.gameObject.SetActive(false);
    }

    private void DisplayCount(float timeToDisplay)
    {
        timeToDisplay = Mathf.Max(timeToDisplay, 0);
        int seconds = Mathf.FloorToInt(timeToDisplay);

        textCount.text = seconds.ToString();
    }

    #endregion

    private void CheckEnemyStatus()
    {
        // Remove dead enemies from the list
        characters.RemoveAll(enemy => enemy.isDead);
    }
}
