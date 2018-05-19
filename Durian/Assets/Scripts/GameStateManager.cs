using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStateManager : MonoBehaviour {
    public GameObject[] players;
    public GameObject[] playersCurrentTile;

    // UI VARIABLES
    public Text numberRolledText;
    public Text currentTurnText;

    public Button rollDiceButton;

    public GameObject combatPromptPanel;
    
    // Game State
    [HideInInspector]
    public GameStates currentState;

    private int numberedRolled;
    private bool coroutineStarted;

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
	void Start () {
        currentState = GameStates.PregameSetting;

        numberRolledText.text = "Rolled: -";

        combatPromptPanel.SetActive(false);

        coroutineStarted = false;
	}
	
	// Update is called once per frame
	void Update () {
		switch (currentState)
        {
            case (GameStates.PregameSetting):
                currentState = GameStates.Player1Turn;
                break;
            case (GameStates.Player1Turn):
                currentTurnText.text = "Player 1 Turn";
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
                currentTurnText.text = "Player 2 Turn";
                rollDiceButton.enabled = true;
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
                currentTurnText.text = "Player 3 Turn";
                rollDiceButton.enabled = true;
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
                currentTurnText.text = "Player 2 Turn";
                rollDiceButton.enabled = true;
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
        numberedRolled = Random.Range(1, 6);

        if (currentState == GameStates.Player1Turn)
        {
            currentState = GameStates.Player1Move;
        }
        else if (currentState == GameStates.Player2Turn)
        {
            currentState = GameStates.Player2Move;
        }
        else if (currentState == GameStates.Player3Turn)
        {
            currentState = GameStates.Player3Move;
        }
        else if (currentState == GameStates.Player4Turn)
        {
            currentState = GameStates.Player4Move;
        }
    }

    IEnumerator movePlayer(int playerIndex)
    {
        coroutineStarted = true;

        for (int i = 0; i < numberedRolled; i++)
        {
            playersCurrentTile[playerIndex] = playersCurrentTile[playerIndex].GetComponent<BoardTile>().NextBoardTiles[0];
            players[playerIndex].transform.position = playersCurrentTile[playerIndex].transform.position;

            foreach (GameObject player in players) {
                if (players[playerIndex].transform.position == player.transform.position && players[playerIndex] != player)
                {
                    combatPromptPanel.SetActive(true);
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
}
