using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagTile : BoardTile
{
    public override void tileEffect(PlayerStats player)
    {
        if (FlagStatus.FlagAvaliable)
        {
            player.grabFlag();

            FlagStatus.FlagAvaliable = false;
        }
        else
        {
            return;
        }
    }
}
