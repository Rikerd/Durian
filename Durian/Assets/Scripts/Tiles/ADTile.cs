using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ADTile : BoardTile {
    public GameObject buffPanel;
    public Button defButton;

    private GameStateManager gm;

    private void Start()
    {
        gm = GameObject.Find("Game Manager").GetComponent<GameStateManager>();
    }

    public override void tileEffect(PlayerStats player)
    {
        if (player.holdingFlag)
        {
            gm.Pass();
        } else
        {
            buffPanel.SetActive(true);

            if (player.def >= player.maxDef)
            {
                defButton.enabled = false;
            }
            else
            {
                defButton.enabled = true;
            }
        }
    }
}
