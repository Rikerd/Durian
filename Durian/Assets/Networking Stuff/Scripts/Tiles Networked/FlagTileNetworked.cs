using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagTileNetworked : BoardTileNetworked
{
    public override void tileEffect(NetworkedPlayerController player)
    {
        if (FlagStatus.FlagAvaliable)
        {
            player.playersStats.grabFlag();

            FlagStatus.FlagAvaliable = false;
        }
        else
        {
            return;
        }
    }
}
