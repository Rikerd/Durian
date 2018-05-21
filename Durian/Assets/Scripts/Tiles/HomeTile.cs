using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomeTile : BoardTile
{
    public GameObject homePanel;
    public Button healButton;

    public override void tileEffect(PlayerStats player)
    {
        homePanel.SetActive(true);

        if (player.hp >= player.maxHp)
        {
            healButton.enabled = false;
        } else
        {
            healButton.enabled = true;
        }
    }
}
