using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LUTileNetworked : BoardTileNetworked
{

    public override void tileEffect(NetworkedPlayerController player)
    {
        print("LUTile");
        player.luPromptPanel.SetActive(true);
    }
}
