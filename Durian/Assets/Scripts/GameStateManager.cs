using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStateManager : MonoBehaviour {
    public GameObject[] players;
    public PlayerStats[] playersStats;
    public GameObject[] playersCurrentTile;

    // UI VARIABLES
    public Text numberRolledText;

    public Button rollDiceButton;

    public GameObject combatPromptPanel;
    public Text combatPromptText;

    public GameObject luPromptPanel;
    public GameObject lrPromptPanel;
    
    // Game State
    [HideInInspector]
    public GameStates currentState;

    private int numberedRolled;
    private bool coroutineStarted;
    private bool fightAccepted;
    private bool responded;
    private bool left;

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

        fightAccepted = false;

        responded = false;

        left = false;
	}
	
	// Update is called once per frame
	void Update () {
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

    public void moveLeft()
    {
        responded = true;

        left = true;

        luPromptPanel.SetActive(false);
        lrPromptPanel.SetActive(false);
    }

    public void moveUpOrRight()
    {
        responded = true;

        left = false;

        luPromptPanel.SetActive(false);
        lrPromptPanel.SetActive(false);
    }

    IEnumerator movePlayer(int currentPlayerIndex)
    {
        coroutineStarted = true;

        for (int i = 0; i < numberedRolled; i++)
        {
            BoardTile currentTile = playersCurrentTile[currentPlayerIndex].GetComponent<BoardTile>();

            if (currentTile.NextBoardTiles.Length > 1)
            {
                currentTile.tileEffect(playersStats[currentPlayerIndex]);

                while (!responded)
                {
                    yield return null;
                }

                if (left)
                {
                    playersCurrentTile[currentPlayerIndex] = currentTile.NextBoardTiles[0];
                } else
                {
                    playersCurrentTile[currentPlayerIndex] = currentTile.NextBoardTiles[1];
                }

                responded = false;
            } else
            {
                playersCurrentTile[currentPlayerIndex] = currentTile.NextBoardTiles[0];
            }
            
            players[currentPlayerIndex].transform.position = playersCurrentTile[currentPlayerIndex].transform.position;

            for (int defendingIndex = 0; defendingIndex < players.Length; defendingIndex++) {
                if (players[currentPlayerIndex].transform.position == players[defendingIndex].transform.position && currentPlayerIndex != defendingIndex)
                {
                    combatPromptPanel.SetActive(true);
                    combatPromptText.text = "fight " + players[defendingIndex].name + "?";

                    while (!responded)
                    {
                        yield return null;
                    }

                    responded = false;
                    combatPromptPanel.SetActive(false);

                    if (fightAccepted)
                    {
                        playersStats[defendingIndex].takeDamage(calculateDamage(currentPlayerIndex, defendingIndex));

                        if (playersStats[defendingIndex].hp >= 0)
                        {
                            playersStats[currentPlayerIndex].takeDamage(calculateDamage(defendingIndex, currentPlayerIndex));
                        }

                        break;
                    }
                }
            }

            if (fightAccepted)
            {
                break;
            }

            //playersCurrentTile[currentPlayerIndex].GetComponent<BoardTile>().tileEffect(playersStats[currentPlayerIndex]);

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

    private int calculateDamage(int atkPlayerIndex, int defPlayerIndex)
    {
        int atkRoll = Random.Range(1, 6) + playersStats[atkPlayerIndex].atk;
        int defRoll = Random.Range(1, 6) + playersStats[defPlayerIndex].def;

        int dmgTaken = atkRoll - defRoll;

        if (dmgTaken <= 0)
        {
            dmgTaken = 1;
        }

        return dmgTaken;
    }
}
