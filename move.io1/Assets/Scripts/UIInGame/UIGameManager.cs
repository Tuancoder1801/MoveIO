using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIGameManager : Singleton<UIGameManager>
{
    public Text textAlive;
    public TextMeshProUGUI textRank;
    public Text textKiller;
    public Text textCoins;

    public GameObject bg_alive;
    public GameObject endGame;

    public Button bt_backHome;
    public Button bt_touch;
    public Button bt_claim;

    public Indicator[] indicator;

    public Reward reward;

    private bool hasGameEnded = false;

    private void Awake()
    {
        bt_touch.onClick.AddListener(TouchToContinue);
        bt_backHome.onClick.AddListener(BackHome);
        bt_claim.onClick.AddListener(Claim);
    }

    private void Start()
    {
    }

    private void Update()
    {
        CountAlive();
        EndGame();
    }

    private void EndGame()
    {
        if (GameController.Instance.playerInstance != null && GameController.Instance.playerInstance.isDead)
        {
            if (!hasGameEnded)
            {
                //StartCoroutine(ShowEndScreenAfterDelay(2f));
                Invoke("DisplayReward", 2f);

                hasGameEnded = true;
            }
        }
        else if (GameController.Instance.characters.Count <= 1)
        {
            if (!hasGameEnded)
            {
                Invoke("DisplayReward", 2f);
                hasGameEnded = true;
            }
        }
    }

    private void DisplayReward()
    {
        reward.gameObject.SetActive(true);
    }

    private IEnumerator ShowEndScreenAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        bg_alive.SetActive(false);
        endGame.SetActive(true);
        string aliveCountText = textAlive.text.Replace("Alive: ", "");
        textRank.text = "#" + aliveCountText;

        textKiller.text = GameController.Instance.killerName.ToString();
        textKiller.color = GameController.Instance.killerColor;


        textCoins.text = (GameController.Instance.playerInstance.kill * 20).ToString();

        int coinsReward = GameController.Instance.playerInstance.kill * 20;
        float currentTotalCoins = UserData.coins.LoadCoins();

        UserData.coins.SaveCoins((float)coinsReward + currentTotalCoins);


        Debug.Log((float)coinsReward + currentTotalCoins);
    }

    private void CountAlive()
    {
        int aliveEnemiesCount = GameController.Instance.characters.Count(enemy => !enemy.isDead);

        textAlive.text = "Alive: " + aliveEnemiesCount.ToString();
    }

    private void BackHome()
    {
        SceneManager.LoadScene(StaticValue.KEY_MENU_SCENE);
    }

    private void TouchToContinue()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void Claim()
    {   
        reward.gameObject.SetActive(false);
        StartCoroutine(ShowEndScreenAfterDelay(0.2f));
    }
}
