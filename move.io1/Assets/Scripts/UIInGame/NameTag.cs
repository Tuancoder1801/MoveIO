using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NameTag : MonoBehaviour
{
    public TextMeshProUGUI textNameTag;
    public TextMeshProUGUI textScore;
    public Image bg_score;

    public Transform mainCamera;

    protected Vector3 offset = new Vector3(0, 180, 0);

    public virtual void Start()
    {
        mainCamera = GameObject.Find("Camera") != null ?
            GameObject.Find("Camera").GetComponent<Transform>() : null;
    }


    public virtual void Update()
    {
        if (mainCamera != null)
        {
            transform.LookAt(mainCamera.transform);
            transform.Rotate(offset);
        }
    }
}
