using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameTagPlayer : NameTag
{
    public Player player;

    private void Start()
    {
        base.Start();
    }

    void Update()
    {
        base.Update();

        if (player != null)
        {
            textScore.text = player.score.ToString();
        }
    }
}
