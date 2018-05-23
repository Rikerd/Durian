using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ADTileNetworked : BoardTileNetworked
{

    public override void tileEffect(NetworkedPlayerController player)
    {
        player.adPromptPanel.SetActive(true);

        if (player.playersStats.def >= player.playersStats.maxDef)
        {
            //defButton.enabled = false;
        }
        else
        {
            //defButton.enabled = true;
        }
    }
}
