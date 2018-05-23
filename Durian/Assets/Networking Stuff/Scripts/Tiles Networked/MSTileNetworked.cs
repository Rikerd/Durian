using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MSTileNetworked : BoardTileNetworked
{
    private NetworkedGameStateManager gm;

    private void Start()
    {
        gm = GameObject.Find("Game Manager").GetComponent<NetworkedGameStateManager>();
    }

    public override void tileEffect(NetworkedPlayerController player)
    {

        if (player.playersStats.movement < player.playersStats.maxMove)
        {
            //buffPanel.SetActive(true);
            player.msPromptPanel.SetActive(true);
        }
        else
        {
            //gm.Pass();
        }
    }
}
