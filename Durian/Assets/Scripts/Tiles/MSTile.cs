using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MSTile : BoardTile
{
    public GameObject buffPanel;

    private GameStateManager gm;

    private void Start()
    {
        gm = GameObject.Find("Game Manager").GetComponent<GameStateManager>();
    }

    public override void tileEffect(PlayerStats player)
    {
        if (player.movement < 3)
        {
            buffPanel.SetActive(true);
        } else
        {
            gm.Pass();
        }
    }
}
