using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    public void ButtonPlay()
    {
        SceneManager.LoadScene("PlayScene");
    }

    public void ButtonExit()
    {
        SceneManager.LoadScene("MenuScene");
    }

    public void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }
}
