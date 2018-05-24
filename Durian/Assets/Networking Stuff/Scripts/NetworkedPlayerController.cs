using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class NetworkedPlayerController : NetworkBehaviour
{
    public int playerNum; //will determine which turn is this player's
    [SyncVar]
    public int rolledNum;
    public NetworkedPlayerStats playersStats;

    public GameObject combatPromptPanel;
    public Text combatPromptText;
    public GameObject luPromptPanel;
    public GameObject lrPromptPanel;
    public GameObject adPromptPanel;
    public GameObject msPromptPanel;

    [SyncVar]
    public bool responded;
    [SyncVar]
    public bool fightAccepted;
    [SyncVar]
    public bool left;
    [SyncVar]
    public bool tileEffectPassed;

    // Use this for initialization
    void Start()
    {
        combatPromptPanel.SetActive(false);
        fightAccepted = false;
        responded = false;
        //playersStats = new NetworkedPlayerStats();
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
        print(netId + " is local player: " + isLocalPlayer);
        if (!isLocalPlayer)
            return;

        int roll = Random.Range(1, 6);
        print("roll is: " + roll);
        CmdSetNumberedRolled(roll);
    }

    [Command]
    void CmdSetNumberedRolled(int roll)
    {
        //print("sending rolled num to server: " + roll);
        rolledNum = roll;
        GameObject.FindGameObjectWithTag("Game Manager").GetComponent<NetworkedGameStateManager>().nextState();
    }

    [Command]
    public void CmdFightAccepted()
    {
        print("Fight accepted!");
        responded = true;
        fightAccepted = true;
    }

    [Command]
    public void CmdFightDeclined()
    {
        print("Fight declined!");
        responded = true;
        fightAccepted = false;
    }

    [Command]
    public void CmdMoveLeft()
    {
        print("Moved Left");

        responded = true;
        left = true;

        RpcMoveLeft();

        //luPromptPanel.SetActive(false);
        //lrPromptPanel.SetActive(false);
    }

    [ClientRpc]
    public void RpcMoveLeft()
    {
        print("Client Moved Left");

        responded = true;
        left = true;
    }

    [Command]
    public void CmdMoveUpOrRight()
    {
        print("Moved Up or Right");

        responded = true;
        left = false;

        RpcMoveUpOrRight();

        //luPromptPanel.SetActive(false);
        //lrPromptPanel.SetActive(false);
    }

    [ClientRpc]
    public void RpcMoveUpOrRight()
    {
        print("Client Moved Up or Right");

        responded = true;
        left = false;
    }

    [Command]
    public void CmdAktBuff()
    {
        print("Attack Buff Obtained");

        responded = true;
        tileEffectPassed = false;

        //playersStats.increaseAtk();
        playersStats.atk += 1;
        RpcAktBuff();
        //adPromptPanel.SetActive(false);
    }

    [ClientRpc]
    public void RpcAktBuff()
    {
        if (isServer)
            return;
        print("Client Attack Buff Obtained");
        playersStats.atk += 1;
    }

    [Command]
    public void CmdDefBuff()
    {
        print("Defense Buff Obtained");
        responded = true;
        tileEffectPassed = false;

        //playersStats.increaseDef();
        playersStats.def += 1;
        RpcDefBuff();
        //adPromptPanel.SetActive(false);
    }

    [ClientRpc]
    public void RpcDefBuff()
    {
        if (isServer)
            return;
        print("Client Attack Buff Obtained");
        playersStats.def += 1;
    }

    [Command]
    public void CmdMoveBuff()
    {
        print("Move Buff Obtained");
        responded = true;
        tileEffectPassed = false;

        playersStats.movement += 1;
        RpcMoveBuff();
        //movePromptPanel.SetActive(false);
    }

    [ClientRpc]
    public void RpcMoveBuff()
    {
        if (isServer)
            return;
        print("Client Attack Buff Obtained");
        playersStats.movement += 1;
    }

    [Command]
    public void CmdPass()
    {
        print("Tile Passed");
        responded = true;

        tileEffectPassed = true;

        //adPromptPanel.SetActive(false);
        //movePromptPanel.SetActive(false);
    }
}
