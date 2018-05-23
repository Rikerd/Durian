using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LRTileNetworked : BoardTileNetworked
{

    public override void tileEffect(NetworkedPlayerController player)
    {
        print("LRTile");
        player.lrPromptPanel.SetActive(true);
    }
}
