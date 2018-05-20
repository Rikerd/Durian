using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class NetworkedGameStateManager : NetworkBehaviour
{
    public GameObject[] players; //For the player objects (on server end)
    public GameObject[] playersCurrentTile;
    public GameObject[] networkedPlayers; //For the player prefabs from client end

    // UI VARIABLES
    public Text numberRolledText;

    public Button rollDiceButton;

    public GameObject combatPromptPanel;
    public Text combatPromptText;

    // Game State
    //[HideInInspector]
    [SyncVar]
    public GameStates currentState;

    [SyncVar]
    private int numberedRolled;
    private bool coroutineStarted;
    private bool fightAccepted;
    private bool responded;

    public enum GameStates
    {
        PregameSetting,
        Player1Turn,
        Player1Move,
        Player2Turn,
        Player2Move,
        Player3Turn,
        Player3Move,
        Player4Turn,
        Player4Move,
        Combat,
        GameOver
    }

    // Use this for initialization
    void Start()
    {
        currentState = GameStates.PregameSetting;

        numberRolledText.text = "Rolled: -";

        combatPromptPanel.SetActive(false);

        coroutineStarted = false;

        fightAccepted = false;

        responded = false;

        networkedPlayers = GameObject.FindGameObjectsWithTag("NetworkedPlayer");
        print("Num of networked players: " + networkedPlayers.Length);
    }

    // Update is called once per frame
    void Update()
    {
        //Getting all networked players
        if (networkedPlayers.Length != 2) //TODO: change so that it's 4
        {
            networkedPlayers = GameObject.FindGameObjectsWithTag("NetworkedPlayer");
            print("Num of networked players: " + networkedPlayers.Length);
            for (int i=0; i < networkedPlayers.Length; i++)
            {
                networkedPlayers[i].GetComponent<NetworkedPlayerController>().setPlayer(i);
            }
        }

        //Setting up the current gameState
        switch (currentState)
        {
            case (GameStates.PregameSetting):
                currentState = GameStates.Player1Turn;
                break;
            case (GameStates.Player1Turn):
                rollDiceButton.enabled = true;
                break;
            case (GameStates.Player1Move):
                numberRolledText.text = "Rolled: " + numberedRolled;

                if (!isServer)
                    return;
                if (!coroutineStarted)
                {
                    rollDiceButton.enabled = false;
                    StartCoroutine(movePlayer(0));
                }

                break;
            case (GameStates.Player2Turn):
                rollDiceButton.enabled = true;
                break;
            case (GameStates.Player2Move):
                numberRolledText.text = "Rolled: " + numberedRolled;

                if (!isServer)
                    return;
                if (!coroutineStarted)
                {
                    rollDiceButton.enabled = false;
                    StartCoroutine(movePlayer(1));
                }

                break;
            case (GameStates.Player3Turn):
                rollDiceButton.enabled = true;
                break;
            case (GameStates.Player3Move):
                numberRolledText.text = "Rolled: " + numberedRolled;

                if (!isServer)
                    return;
                if (!coroutineStarted)
                {
                    rollDiceButton.enabled = false;
                    StartCoroutine(movePlayer(2));
                }

                break;
            case (GameStates.Player4Turn):
                rollDiceButton.enabled = true;
                break;
            case (GameStates.Player4Move):
                numberRolledText.text = "Rolled: " + numberedRolled;

                if (!isServer)
                    return;
                if (!coroutineStarted)
                {
                    rollDiceButton.enabled = false;
                    StartCoroutine(movePlayer(3));
                }

                break;
            case (GameStates.Combat):
                break;
            case (GameStates.GameOver):
                break;
        }
    }

    public void RollDice()
    {
        //numberedRolled = Random.Range(1, 6);

        if (currentState == GameStates.Player1Turn)
        {
            networkedPlayers[0].GetComponent<NetworkedPlayerController>().RollDice();
            numberedRolled = networkedPlayers[0].GetComponent<NetworkedPlayerController>().rolledNum;
            //currentState = GameStates.Player1Move;
        }
        else if (currentState == GameStates.Player2Turn)
        {
            networkedPlayers[1].GetComponent<NetworkedPlayerController>().RollDice();
            numberedRolled = networkedPlayers[1].GetComponent<NetworkedPlayerController>().rolledNum;
            //currentState = GameStates.Player2Move;
        }
        else if (currentState == GameStates.Player3Turn)
        {
            networkedPlayers[0].GetComponent<NetworkedPlayerController>().RollDice();
            numberedRolled = networkedPlayers[0].GetComponent<NetworkedPlayerController>().rolledNum;
            //currentState = GameStates.Player3Move;
        }
        else if (currentState == GameStates.Player4Turn)
        {
            networkedPlayers[1].GetComponent<NetworkedPlayerController>().RollDice();
            numberedRolled = networkedPlayers[1].GetComponent<NetworkedPlayerController>().rolledNum;
            //currentState = GameStates.Player4Move;
        }
    }

    public void FightAccepted()
    {
        responded = true;

        fightAccepted = true;
    }

    public void FightDeclined()
    {
        responded = true;

        fightAccepted = false;
    }

    IEnumerator movePlayer(int playerIndex)
    {
        print(currentState + " - Player" + playerIndex + " Moving: " + numberedRolled);
        coroutineStarted = true;

        for (int i = 0; i < numberedRolled; i++)
        {
            playersCurrentTile[playerIndex] = playersCurrentTile[playerIndex].GetComponent<BoardTile>().NextBoardTiles[0];
            players[playerIndex].transform.position = playersCurrentTile[playerIndex].transform.position;

            foreach (GameObject player in players)
            {
                if (players[playerIndex].transform.position == player.transform.position && players[playerIndex] != player)
                {
                    combatPromptPanel.SetActive(true);
                    combatPromptText.text = "fight " + player.name + "?";

                    while (!responded)
                    {
                        yield return null;
                    }

                    responded = false;
                    combatPromptPanel.SetActive(false);

                    if (fightAccepted)
                    {
                        break;
                    }
                }
            }

            yield return new WaitForSeconds(0.2f);
        }

        coroutineStarted = false;


        if (currentState == GameStates.Player1Move)
        {
            currentState = GameStates.Player2Turn;
        }
        else if (currentState == GameStates.Player2Move)
        {
            currentState = GameStates.Player3Turn;
        }
        else if (currentState == GameStates.Player3Move)
        {
            currentState = GameStates.Player4Turn;
        }
        else if (currentState == GameStates.Player4Move)
        {
            currentState = GameStates.Player1Turn;
        }
    }

    public void nextState()
    {
        print("Changing currentState...");
        switch (currentState)
        {
            case (GameStates.PregameSetting):
                currentState = GameStates.Player1Turn;
                break;
            case (GameStates.Player1Turn):
                currentState = GameStates.Player1Move;
                break;
            case (GameStates.Player2Turn):
                currentState = GameStates.Player2Move;
                break;
            case (GameStates.Player3Turn):
                currentState = GameStates.Player3Move;
                break;
            case (GameStates.Player4Turn):
                currentState = GameStates.Player4Move;
                break;
        }
        print("currentState now: " + currentState);
    }
}
