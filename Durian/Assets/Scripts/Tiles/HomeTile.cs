using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomeTile : BoardTile
{
    public GameObject homePanel;
    public Button healButton;
    public Button tpButton;

    private GameStateManager gm;

    private void Start()
    {
        gm = GameObject.Find("Game Manager").GetComponent<GameStateManager>();
    }

    public override void tileEffect(PlayerStats player)
    {
        if (player.hp >= player.maxHp && player.holdingFlag)
        {
            gm.Pass();
        } else
        {
            homePanel.SetActive(true);

            if (player.hp >= player.maxHp)
            {
                healButton.enabled = false;
            }
            else
            {
                healButton.enabled = true;
            }

            if (player.holdingFlag)
            {
                tpButton.enabled = false;
            }
            else
            {
                tpButton.enabled = true;
            }
        }
    }
}
