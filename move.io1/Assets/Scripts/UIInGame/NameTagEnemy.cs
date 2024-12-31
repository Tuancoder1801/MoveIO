using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class NameTagEnemy : NameTag
{
    public Enemy enemy;

    private void Start()
    {
        base.Start();

        UpdateColor();
    }

    void Update()
    {
        base.Update();

        if (enemy != null)
        {
            textScore.text = enemy.score.ToString();
            textNameTag.text = enemy.enemyName.ToString();
        }

        if (enemy.isDead)
        {
            this.gameObject.SetActive(false);
            return;
        }
     
    }

    private void UpdateColor()
    {
        if (enemy != null && enemy.characterRenderer != null)
        {
            Color enemyColor = enemy.characterRenderer.material.color;

            if (textNameTag != null)
            {
                textNameTag.color = enemyColor;
            }

            if (bg_score != null)
            {
                bg_score.color = enemyColor;
            }
        }
    }
}
