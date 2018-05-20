using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ADTile : BoardTile {
    public GameObject buffPanel;
    public Button defButton;

    public override void tileEffect(PlayerStats player)
    {
        buffPanel.SetActive(true);

        if (player.def >= player.maxDef)
        {
            defButton.enabled = false;
        } else
        {
            defButton.enabled = true;
        }
    }
}
