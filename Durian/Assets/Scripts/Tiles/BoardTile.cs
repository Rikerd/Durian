using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class BoardTile : MonoBehaviour {

    public GameObject[] NextBoardTiles;

    public abstract void tileEffect(PlayerStats player);

}
