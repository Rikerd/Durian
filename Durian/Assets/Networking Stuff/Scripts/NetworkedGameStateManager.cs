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
    public Text[] statLines;
    public Text currentTurn;

    //public GameObject combatPromptPanel;
    //public Text combatPromptText;

    // Game State
    //[HideInInspector]
    [SyncVar]
    public GameStates currentState;

    [SyncVar]
    private int numberedRolled;
    [SyncVar]
    public bool coroutineStarted;
    //private int currentPlayerIndex;
    //private bool fightAccepted;
    //private bool responded;

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
        print("Setting up Game Manager...");

        currentState = GameStates.PregameSetting;

        numberRolledText.text = "Rolled: -";

        coroutineStarted = false;

        networkedPlayers = GameObject.FindGameObjectsWithTag("NetworkedPlayer");
        print("Num of networked players: " + networkedPlayers.Length);
        for (int i = 0; i < networkedPlayers.Length; i++)
        {
            print("Setting up Player " + i);
            networkedPlayers[i].GetComponent<NetworkedPlayerController>().setPlayer(i);
            print("Player " + i + " set");
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Getting all networked players
        if (networkedPlayers.Length != 4)
        {
            networkedPlayers = GameObject.FindGameObjectsWithTag("NetworkedPlayer");
            print("Num of networked players: " + networkedPlayers.Length);
            for (int i = 0; i < networkedPlayers.Length; i++)
            {
                print("Setting up Player " + i);
                networkedPlayers[i].GetComponent<NetworkedPlayerController>().setPlayer(i);
                print("Player " + i + " set");
            }
            print("All players set!");
        }

        //Updating Stat text
        //print("Updating Stats...");
        //print("statsLines.length: " + statLines.Length);
        for (int playerIndex = 0; playerIndex < statLines.Length; playerIndex++)
        {
            //print("playerIndex: " + playerIndex);
            //int netPlayerIndex = 0;///TODO: GET RID OF AND MAKE JUST FLAT OUT playerIndex
            //if (playerIndex == 1 || playerIndex == 3)///
            //    netPlayerIndex = 1;///
            //NetworkedPlayerStats stats = networkedPlayers[netPlayerIndex].GetComponent<NetworkedPlayerController>().playersStats;
            NetworkedPlayerStats stats = networkedPlayers[playerIndex].GetComponent<NetworkedPlayerController>().playersStats;
            statLines[playerIndex].text = "HP: " + stats.hp + "\nAtk: " + stats.atk + "\nDef: " + stats.def + "\nMove: " + stats.movement;
        }

        print("currentState: " + currentState);
        //Setting up the current gameState
        switch (currentState)
        {
            case (GameStates.PregameSetting):
                currentState = GameStates.Player1Turn;
                currentTurn.text = "Player 1 turn";
                break;
            case (GameStates.Player1Turn):
                rollDiceButton.enabled = true;
                break;
            case (GameStates.Player1Move):
                numberRolledText.text = "Rolled: " + numberedRolled;

                if (!coroutineStarted)
                {
                    rollDiceButton.enabled = false;
                    StartCoroutine(movePlayer(0));
                }

                break;
            case (GameStates.Player2Turn):
                rollDiceButton.enabled = true;
                currentTurn.text = "Player 2 turn";
                break;
            case (GameStates.Player2Move):
                numberRolledText.text = "Rolled: " + numberedRolled;

                if (!coroutineStarted)
                {
                    rollDiceButton.enabled = false;
                    StartCoroutine(movePlayer(1));
                }

                break;
            case (GameStates.Player3Turn):
                rollDiceButton.enabled = true;
                currentTurn.text = "Player 3 turn";
                break;
            case (GameStates.Player3Move):
                numberRolledText.text = "Rolled: " + numberedRolled;

                if (!coroutineStarted)
                {
                    rollDiceButton.enabled = false;
                    StartCoroutine(movePlayer(2));
                }

                break;
            case (GameStates.Player4Turn):
                rollDiceButton.enabled = true;
                currentTurn.text = "Player 4 turn";
                break;
            case (GameStates.Player4Move):
                numberRolledText.text = "Rolled: " + numberedRolled;

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
        print(currentState + " - Rolling Dice...");
        
        if (currentState == GameStates.Player1Turn)
        {
            networkedPlayers[0].GetComponent<NetworkedPlayerController>().RollDice();
        }
        else if (currentState == GameStates.Player2Turn)
        {
            networkedPlayers[1].GetComponent<NetworkedPlayerController>().RollDice();
        }
        else if (currentState == GameStates.Player3Turn)
        {
            networkedPlayers[2].GetComponent<NetworkedPlayerController>().RollDice();
        }
        else if (currentState == GameStates.Player4Turn)
        {
            networkedPlayers[3].GetComponent<NetworkedPlayerController>().RollDice();
        }
    }

    IEnumerator movePlayer(int playerIndex)
    {
        print(currentState + " - Player" + (playerIndex + 1) + " Moving: " + numberedRolled);

        //int netPlayerIndex = 0;///TODO: GET RID OF AND MAKE JUST FLAT OUT playerIndex
        //if (playerIndex == 1 || playerIndex == 3)///
        //    netPlayerIndex = 1;///
        //NetworkedPlayerController currentPlayerController = networkedPlayers[netPlayerIndex].GetComponent<NetworkedPlayerController>();
        NetworkedPlayerController currentPlayerController = networkedPlayers[playerIndex].GetComponent<NetworkedPlayerController>();

        coroutineStarted = true;

        for (int i = 0; i < numberedRolled; i++)
        {
            BoardTileNetworked currentTile = playersCurrentTile[playerIndex].GetComponent<BoardTileNetworked>();
            print(currentTile);
            //currentPlayerController.responded = false;
            //currentPlayerController.left = false;
            //currentPlayerController.fightAccepted = false;
            //currentPlayerController.tileEffectPassed = false;

            if (currentTile.NextBoardTiles.Length > 1)
            {
                print("Dealing w/ a Switch Tile");
                if (!currentPlayerController.isLocalPlayer)
                {
                    while (!currentPlayerController.responded)
                    {
                        yield return null;
                    }
                    //break;
                }
                if (currentPlayerController.isLocalPlayer)
                    currentTile.tileEffect(currentPlayerController);

                while (!currentPlayerController.responded)
                {
                    yield return null;
                }

                if (currentPlayerController.left)
                    playersCurrentTile[playerIndex] = currentTile.NextBoardTiles[0];
                else
                    playersCurrentTile[playerIndex] = currentTile.NextBoardTiles[1];

                currentPlayerController.responded = false;
                print("Reponded Reset to: " + currentPlayerController.responded);
                currentPlayerController.luPromptPanel.SetActive(false);
                currentPlayerController.lrPromptPanel.SetActive(false);
            }
            else
            {
                playersCurrentTile[playerIndex] = currentTile.NextBoardTiles[0];
            }
            print("1");
            players[playerIndex].transform.position = playersCurrentTile[playerIndex].transform.position;

            foreach (GameObject player in players)
            {
                if (players[playerIndex].transform.position == player.transform.position && players[playerIndex] != player)
                {
                    if (currentPlayerController.isLocalPlayer)
                    {
                        currentPlayerController.combatPromptPanel.SetActive(true);
                        currentPlayerController.combatPromptText.text = "fight " + player.name + "?";
                    }

                    while (!currentPlayerController.responded)
                    {
                        yield return null;
                    }

                    currentPlayerController.responded = false;
                    currentPlayerController.combatPromptPanel.SetActive(false);

                    if (currentPlayerController.fightAccepted)
                    {
                        NetworkedPlayerStats defenderStats = player.GetComponent<NetworkedPlayerController>().playersStats;
                        defenderStats.takeDamage(calculateDamage(currentPlayerController.playersStats, defenderStats));

                        if (defenderStats.hp >= 0)
                        {
                            currentPlayerController.playersStats.takeDamage(calculateDamage(defenderStats, currentPlayerController.playersStats));
                        }
                    }
                }
            }
            print("2");
            if (currentPlayerController.fightAccepted)
            {
                print("fightAccepted");
                coroutineStarted = false;
                break;
            }
            print("3");
            BoardTileNetworked newCurrentTile = playersCurrentTile[playerIndex].GetComponent<BoardTileNetworked>();
            print("4");
            if (!(newCurrentTile is LUTileNetworked) && !(newCurrentTile is LRTileNetworked) && !(newCurrentTile is BlankTileNetworked))
            {
                print("Current Player isLocal: " + currentPlayerController.isLocalPlayer);
                if (!currentPlayerController.isLocalPlayer)
                {
                    while (!currentPlayerController.responded)
                    {
                        yield return null;
                    }
                }
                if (currentPlayerController.isLocalPlayer)
                {
                    newCurrentTile.tileEffect(currentPlayerController);
                }
                while (!currentPlayerController.responded)
                {
                    yield return null;
                }

                currentPlayerController.adPromptPanel.SetActive(false);
                currentPlayerController.msPromptPanel.SetActive(false);

                print("tile effect passed: " + currentPlayerController.tileEffectPassed);
                if (!currentPlayerController.tileEffectPassed)
                {
                    print("Breaking...");
                    coroutineStarted = false;
                    break;
                }
            }
            print("5");
            yield return new WaitForSeconds(0.2f);
        }

        coroutineStarted = false;
        currentPlayerController.responded = false;

        if (currentState == GameStates.Player1Move)
        {
            coroutineStarted = false;
            currentPlayerController.responded = false;
            currentState = GameStates.Player2Turn;
        }
        else if (currentState == GameStates.Player2Move)
        {
            print("ok....");
            coroutineStarted = false;
            currentPlayerController.responded = false;
            currentState = GameStates.Player3Turn;
        }
        else if (currentState == GameStates.Player3Move)
        {
            coroutineStarted = false;
            currentPlayerController.responded = false;
            currentState = GameStates.Player4Turn;
        }
        else if (currentState == GameStates.Player4Move)
        {
            coroutineStarted = false;
            currentPlayerController.responded = false;
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
                coroutineStarted = false;
                numberedRolled = networkedPlayers[0].GetComponent<NetworkedPlayerController>().rolledNum;
                break;
            case (GameStates.Player2Turn):
                currentState = GameStates.Player2Move;
                coroutineStarted = false;
                numberedRolled = networkedPlayers[1].GetComponent<NetworkedPlayerController>().rolledNum;
                break;
            case (GameStates.Player3Turn):
                currentState = GameStates.Player3Move;
                coroutineStarted = false;
                numberedRolled = networkedPlayers[2].GetComponent<NetworkedPlayerController>().rolledNum;
                break;
            case (GameStates.Player4Turn):
                currentState = GameStates.Player4Move;
                coroutineStarted = false;
                numberedRolled = networkedPlayers[3].GetComponent<NetworkedPlayerController>().rolledNum;
                break;
        }
        print("currentState now: " + currentState);
    }

    private int calculateDamage(NetworkedPlayerStats atkPlayerStats, NetworkedPlayerStats defPlayerStats)
    {
        int atkRoll = Random.Range(1, 6) + atkPlayerStats.atk;
        int defRoll = Random.Range(1, 6) + defPlayerStats.def;

        int dmgTaken = atkRoll - defRoll;

        if (dmgTaken <= 0)
        {
            dmgTaken = 1;
        }

        return dmgTaken;
    }
}
