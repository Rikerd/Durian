using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LUTile : BoardTile
{
    public GameObject switchPanel;

    public override void tileEffect(PlayerStats player)
    {
        switchPanel.SetActive(true);
    }
}
