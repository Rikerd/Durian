using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkedPlayerController : NetworkBehaviour
{
    public int playerNum; //will determine which turn is this player's
    [SyncVar]
    public int rolledNum;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void setPlayer(int num)
    {
        playerNum = num;
    }

    public void RollDice()
    {
        print(netId);
        if (!isLocalPlayer)
            return;

        int roll = Random.Range(1, 6);
        //print("roll is: " + roll);
        CmdSetNumberedRolled(roll);
    }

    [Command]
    void CmdSetNumberedRolled(int roll)
    {
        //print("sending rolled num to server: " + roll);
        rolledNum = roll;
        GameObject.FindGameObjectWithTag("GameController").GetComponent<NetworkedGameStateManager>().nextState();
    }
}
