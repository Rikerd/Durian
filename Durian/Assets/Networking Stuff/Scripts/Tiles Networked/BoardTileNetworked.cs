using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

abstract public class BoardTileNetworked : NetworkBehaviour
{

    public GameObject[] NextBoardTiles;

    public abstract void tileEffect(NetworkedPlayerController player);

}
