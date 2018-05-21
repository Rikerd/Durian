using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStateManager : MonoBehaviour {
    public GameObject[] players;
    public PlayerStats[] playersStats;
    public GameObject[] playersCurrentTile;
    public GameObject[] playersHomeTile;

    // UI VARIABLES
    public Text numberRolledText;

    public Button rollDiceButton;

    public GameObject combatPromptPanel;
    public Text combatPromptText;

    public GameObject luPromptPanel;
    public GameObject lrPromptPanel;

    public GameObject adPromptPanel;

    public GameObject movePromptPanel;

    public GameObject homePromptPanel;
    public GameObject tpPromptPanel;

    public Button[] tpButtons;
    
    // Game State
    [HideInInspector]
    public GameStates currentState;

    private int currentPlayerIndex;
    private int numberedRolled;
    private bool coroutineStarted;
    private bool fightAccepted;
    private bool responded;
    private bool left;
    private bool tileEffectPassed;

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

        tileEffectPassed = false;
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
                    currentPlayerIndex = 0;
                    rollDiceButton.enabled = false;
                    StartCoroutine(movePlayer());
                }

                break;
            case (GameStates.Player2Turn):
                rollDiceButton.enabled = true;
                break;
            case (GameStates.Player2Move):
                numberRolledText.text = "Rolled: " + numberedRolled;

                if (!coroutineStarted)
                {
                    currentPlayerIndex = 1;
                    rollDiceButton.enabled = false;
                    StartCoroutine(movePlayer());
                }

                break;
            case (GameStates.Player3Turn):
                rollDiceButton.enabled = true;
                break;
            case (GameStates.Player3Move):
                numberRolledText.text = "Rolled: " + numberedRolled;

                if (!coroutineStarted)
                {
                    currentPlayerIndex = 2;
                    rollDiceButton.enabled = false;
                    StartCoroutine(movePlayer());
                }

                break;
            case (GameStates.Player4Turn):
                rollDiceButton.enabled = true;
                break;
            case (GameStates.Player4Move):
                numberRolledText.text = "Rolled: " + numberedRolled;

                if (!coroutineStarted)
                {
                    currentPlayerIndex = 3;
                    rollDiceButton.enabled = false;
                    StartCoroutine(movePlayer());
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

    public void MoveLeft()
    {
        responded = true;

        left = true;

        luPromptPanel.SetActive(false);
        lrPromptPanel.SetActive(false);
    }

    public void MoveUpOrRight()
    {
        responded = true;

        left = false;

        luPromptPanel.SetActive(false);
        lrPromptPanel.SetActive(false);
    }

    public void AktBuff()
    {
        responded = true;

        tileEffectPassed = false;

        playersStats[currentPlayerIndex].increaseAtk();

        adPromptPanel.SetActive(false);
    }

    public void DefBuff()
    {
        responded = true;

        tileEffectPassed = false;

        playersStats[currentPlayerIndex].increaseDef();

        adPromptPanel.SetActive(false);
    }

    public void MoveBuff()
    {
        responded = true;

        tileEffectPassed = false;

        playersStats[currentPlayerIndex].increaseMove();

        movePromptPanel.SetActive(false);
    }

    public void Heal()
    {
        responded = true;

        tileEffectPassed = false;

        if (playersCurrentTile[currentPlayerIndex] == playersHomeTile[currentPlayerIndex])
        {
            playersStats[currentPlayerIndex].fullHeal();
        } else
        {
            playersStats[currentPlayerIndex].halfHeal();
        }

        homePromptPanel.SetActive(false);
    }

    public void ActivateTp()
    {
        homePromptPanel.SetActive(false);
        tpPromptPanel.SetActive(true);

        for (int i = 0; i < tpButtons.Length; i++)
        {
            if (i == currentPlayerIndex)
            {
                tpButtons[i].enabled = false;
            } else
            {
                tpButtons[i].enabled = true;
            }
        }
    }

    public void TpHome(int homeIndex)
    {
        responded = true;

        tileEffectPassed = false;

        playersCurrentTile[currentPlayerIndex] = playersHomeTile[homeIndex];

        players[currentPlayerIndex].transform.position = playersCurrentTile[currentPlayerIndex].transform.position;

        tpPromptPanel.SetActive(false);
    }

    public void DeactiveTp()
    {
        tpPromptPanel.SetActive(false);
        homePromptPanel.SetActive(true);
    }

    public void Pass()
    {
        responded = true;

        tileEffectPassed = true;

        adPromptPanel.SetActive(false);
        movePromptPanel.SetActive(false);
        homePromptPanel.SetActive(false);
    }

    IEnumerator movePlayer()
    {
        coroutineStarted = true;

        for (int i = 0; i < numberedRolled + playersStats[currentPlayerIndex].movement; i++)
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

            BoardTile newCurrentTile = playersCurrentTile[currentPlayerIndex].GetComponent<BoardTile>();

            if (!(newCurrentTile is LUTile) && !(newCurrentTile is LRTile) && !(newCurrentTile is BlankTile))
            {
                newCurrentTile.tileEffect(playersStats[currentPlayerIndex]);

                while (!responded)
                {
                    yield return null;
                }

                responded = false;

                if (!tileEffectPassed)
                {
                    break;
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
