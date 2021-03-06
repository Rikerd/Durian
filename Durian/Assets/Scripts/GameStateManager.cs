﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStateManager : MonoBehaviour {
    public GameObject[] players;
    public PlayerStats[] playersStats;
    public GameObject[] playersCurrentTile;
    public GameObject[] playersHomeTile;
    public GameObject[] secondaryMonsterTiles;

    public Renderer flag;
    public Material avaliableFlagMat;
    public Material unavaliableFlagMat;

    public Renderer[] flagSpots;
    public Material flagSpotMat;

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

    public GameObject battleLogPanel;
    public Text battleLogText;

    public GameObject gameOverPanel;
    public Text gameOverText;

    public Text currentTurn;

    public GameObject extraStatPanel;
    
	public GameObject sounds;

    // Game State
    [HideInInspector]
    public GameStates currentState;

    private int currentPlayerIndex;
    private int numberedRolled;
    private bool coroutineStarted;
    private Coroutine moveCoroutine;
    private bool fightAccepted;
    private bool responded;
    private bool left;
    private bool tileEffectPassed;
    private bool secondaryMonsterCheck;
    private int killingPlayerIndex;

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
        if (FlagStatus.FlagAvaliable)
        {
            flag.material = avaliableFlagMat;

            foreach (Renderer flagSpot in flagSpots)
            {
                flagSpot.material = flagSpotMat;
            }
        } else
        {
            flag.material = unavaliableFlagMat;

            foreach (Renderer flagSpot in flagSpots)
            {
                flagSpot.material = unavaliableFlagMat;
            }
        }

        currentTurn.text = "Player " + (currentPlayerIndex + 1) + " turn";

		switch (currentState)
        {
            case (GameStates.PregameSetting):
                currentState = GameStates.Player1Turn;
                break;
            case (GameStates.Player1Turn):
                currentPlayerIndex = 0;

                if (playersStats[currentPlayerIndex].isDead())
                {
                    playersStats[currentPlayerIndex].increaseDeathTimer();
                    currentState = GameStates.Player2Turn;
                }

                rollDiceButton.interactable = true;
                break;
            case (GameStates.Player1Move):
                numberRolledText.text = "Rolled: " + numberedRolled;
                
                if (!coroutineStarted)
                {
                    rollDiceButton.interactable = false;
                    moveCoroutine = StartCoroutine(movePlayer());
                }

                break;
            case (GameStates.Player2Turn):
                currentPlayerIndex = 1;

                if (playersStats[currentPlayerIndex].isDead())
                {
                    playersStats[currentPlayerIndex].increaseDeathTimer();
                    currentState = GameStates.Player3Turn;
                }

                rollDiceButton.interactable = true;
                break;
            case (GameStates.Player2Move):
                numberRolledText.text = "Rolled: " + numberedRolled;

                if (!coroutineStarted)
                {
                    rollDiceButton.interactable = false;
                    moveCoroutine = StartCoroutine(movePlayer());
                }

                break;
            case (GameStates.Player3Turn):
                currentPlayerIndex = 2;

                if (playersStats[currentPlayerIndex].isDead())
                {
                    playersStats[currentPlayerIndex].increaseDeathTimer();
                    currentState = GameStates.Player4Turn;
                }

                rollDiceButton.interactable = true;
                break;
            case (GameStates.Player3Move):
                numberRolledText.text = "Rolled: " + numberedRolled;

                if (!coroutineStarted)
                {
                    rollDiceButton.interactable = false;
                    moveCoroutine = StartCoroutine(movePlayer());
                }

                break;
            case (GameStates.Player4Turn):
                currentPlayerIndex = 3;

                if (playersStats[currentPlayerIndex].isDead())
                {
                    playersStats[currentPlayerIndex].increaseDeathTimer();
                    currentState = GameStates.Player1Turn;
                }

                rollDiceButton.interactable = true;
                break;
            case (GameStates.Player4Move):
                numberRolledText.text = "Rolled: " + numberedRolled;

                if (!coroutineStarted)
                {
                    rollDiceButton.interactable = false;
                    moveCoroutine = StartCoroutine(movePlayer());
                }

                break;
            case (GameStates.GameOver):
                print("Game Over");
                break;
        }
	}

    public void RollDice()
    {
		sounds.GetComponentInChildren<DiceSFXPlayer> ().DiceRollSound ();
        numberedRolled = Random.Range(1, 6);
        //numberedRolled = 6; 

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

    public void ExtraAtkBuff()
    {
        responded = true;

        playersStats[killingPlayerIndex].increaseAtk();

        extraStatPanel.SetActive(false);
    }

    public void ExtraDefBuff()
    {
        responded = true;

        playersStats[killingPlayerIndex].increaseDef();

        extraStatPanel.SetActive(false);
    }

    public void ExtraMoveBuff()
    {
        responded = true;

        playersStats[killingPlayerIndex].increaseMove();

        extraStatPanel.SetActive(false);
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
            if (playersCurrentTile[currentPlayerIndex] == playersHomeTile[i])
            {
                tpButtons[i].interactable = false;
            } else
            {
                tpButtons[i].interactable = true;
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

		sounds.GetComponent<SFXPlayer> ().LevelUpSound ();
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
		yield return new WaitForSeconds(1f);

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
			sounds.GetComponent<SFXPlayer> ().TapSound ();

            for (int defendingIndex = 0; defendingIndex < players.Length; defendingIndex++) {
                if (players[currentPlayerIndex].transform.position == players[defendingIndex].transform.position && currentPlayerIndex != defendingIndex && !playersStats[defendingIndex].isDead())
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
						sounds.GetComponent<SFXPlayer> ().FightSound ();

                        int atkRoll;
                        int defRoll;
                        int dmgTaken;

                        battleLogPanel.SetActive(true);
                        battleLogText.text = "";

                        atkRoll = Random.Range(1, 6);

                        battleLogText.text += players[currentPlayerIndex].name + " attacks w/ " + atkRoll.ToString() + " + " + playersStats[currentPlayerIndex].atk.ToString();

                        yield return new WaitForSeconds(1f);

                        defRoll = Random.Range(1, 6);

                        battleLogText.text += "\n" + players[defendingIndex].name + " defends w/ " + defRoll.ToString() + " + " + playersStats[defendingIndex].def.ToString();

                        yield return new WaitForSeconds(1f);

                        dmgTaken = (atkRoll + playersStats[currentPlayerIndex].atk) - (defRoll + playersStats[defendingIndex].def);

                        if (dmgTaken <= 0)
                        {
                            dmgTaken = 1;
                        }

                        battleLogText.text += "\n" + players[defendingIndex].name + " takes " + dmgTaken.ToString() + " damage";

                        yield return new WaitForSeconds(1f);

                        playersStats[defendingIndex].takeDamage(dmgTaken);


                        if (!playersStats[defendingIndex].isDead())
                        {
                            atkRoll = Random.Range(1, 6);

                            battleLogText.text += "\n" + players[defendingIndex].name + " attacks w/ " + atkRoll.ToString() + " + " + playersStats[defendingIndex].atk.ToString();

                            yield return new WaitForSeconds(1f);

                            defRoll = Random.Range(1, 6);

                            battleLogText.text += "\n" + players[currentPlayerIndex].name + " defends w/ " + defRoll.ToString() + " + " + playersStats[currentPlayerIndex].def.ToString();

                            yield return new WaitForSeconds(1f);

                            dmgTaken = (atkRoll + playersStats[defendingIndex].atk) - (defRoll + playersStats[currentPlayerIndex].def);

                            if (dmgTaken <= 0)
                            {
                                dmgTaken = 1;
                            }

                            battleLogText.text += "\n" + players[currentPlayerIndex].name + " takes " + dmgTaken.ToString() + " damage";

                            yield return new WaitForSeconds(1f);

                            playersStats[currentPlayerIndex].takeDamage(dmgTaken);

                            if (playersStats[currentPlayerIndex].isDead())
                            {
                                battleLogText.text += "\n" + players[currentPlayerIndex].name + " died!";

                                yield return new WaitForSeconds(1f);
                            }

                            if (playersStats[currentPlayerIndex].isDead() && playersStats[currentPlayerIndex].holdingFlag)
                            {
                                battleLogText.text += "\n" + players[defendingIndex].name + " has taken the flag!";

                                playersStats[currentPlayerIndex].holdingFlag = false;
                                playersStats[defendingIndex].holdingFlag = true;
                            }
                        } else
                        {
                            battleLogText.text += "\n" + players[defendingIndex].name + " died!";

                            yield return new WaitForSeconds(1f);

                            if (playersStats[defendingIndex].holdingFlag)
                            {
                                battleLogText.text += "\n" + players[currentPlayerIndex].name + " has taken the flag!";

                                playersStats[currentPlayerIndex].holdingFlag = true;
                                playersStats[defendingIndex].holdingFlag = false;
                            }
                        }

                        yield return new WaitForSeconds(3f);

                        battleLogPanel.SetActive(false);

                        if (playersStats[defendingIndex].isDead())
                        {
                            extraStatPanel.SetActive(true);
                            killingPlayerIndex = defendingIndex;

                            while (!responded)
                            {
                                yield return null;
                            }

                            responded = false;
                        } else if (playersStats[currentPlayerIndex].isDead())
                        {
                            extraStatPanel.SetActive(true);
                            killingPlayerIndex = currentPlayerIndex;

                            while (!responded)
                            {
                                yield return null;
                            }

                            responded = false;
                        }

                        break;
                    }
                }
            }

            if (fightAccepted)
            {
                fightAccepted = false;
                break;
            }

            BoardTile newCurrentTile = playersCurrentTile[currentPlayerIndex].GetComponent<BoardTile>();

            if ((newCurrentTile is ADTile) || (newCurrentTile is MSTile) || (newCurrentTile is HomeTile))
            {
                if (playersCurrentTile[currentPlayerIndex] == playersHomeTile[currentPlayerIndex] && playersStats[currentPlayerIndex].holdingFlag)
                {
                    currentState = GameStates.GameOver;

                    gameOverPanel.SetActive(true);

                    gameOverText.text = players[currentPlayerIndex].name + "wins!";

                    StopCoroutine(moveCoroutine);
                    break;
                }

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
            else if (newCurrentTile is MonsterTile)
            {
				sounds.GetComponent<SFXPlayer> ().MonsterSound ();

                battleLogPanel.SetActive(true);
                battleLogText.text = "";

                int monsterAtk = Random.Range(1, 6);
                //int monsterAtk = 1;

                battleLogText.text += "Monster attacks w/ " + monsterAtk.ToString() + " + 4";

                yield return new WaitForSeconds(1f);

                int playerDef = Random.Range(1, 6);

                battleLogText.text += "\n" + players[currentPlayerIndex].name + " defends w/ " + playerDef.ToString() + " + " + playersStats[currentPlayerIndex].def;

                int dmgTaken = (monsterAtk + 4) - (playerDef + playersStats[currentPlayerIndex].def);

                if (dmgTaken <= 0)
                {
                    dmgTaken = 1;
                }

                battleLogText.text += "\n" + players[currentPlayerIndex].name + " takes " + dmgTaken.ToString() + " damage";

                yield return new WaitForSeconds(1f);

                playersStats[currentPlayerIndex].takeDamage(dmgTaken);

                if (playersStats[currentPlayerIndex].hp > 0)
                {
                    int monsterDef = Random.Range(1, 6);
                    //int monsterDef = 1;

                    battleLogText.text += "\nMonster defends w/ " + monsterDef.ToString() + " + 4";

                    yield return new WaitForSeconds(1f);

                    int playerAtk = Random.Range(1, 6) + playersStats[currentPlayerIndex].atk;

                    battleLogText.text += "\n" + players[currentPlayerIndex].name + " attacks w/ " + playerAtk.ToString() + " + " + playersStats[currentPlayerIndex].atk;

                    yield return new WaitForSeconds(1f);

                    if (monsterDef > playerAtk)
                    {
                        battleLogText.text += "\n" + players[currentPlayerIndex].name + " loses!";
                        yield return new WaitForSeconds(3f);
                        battleLogPanel.SetActive(false);
                        playersCurrentTile[currentPlayerIndex] = currentTile.gameObject;
                        players[currentPlayerIndex].transform.position = playersCurrentTile[currentPlayerIndex].transform.position;

                        break;
                    } else
                    {
                        battleLogText.text += "\n" + players[currentPlayerIndex].name + " wins!";
                        yield return new WaitForSeconds(3f);
                        battleLogPanel.SetActive(false);
                    }
                }
                else
                {
                    battleLogText.text += "\n" + players[currentPlayerIndex].name + " died!";
                    yield return new WaitForSeconds(3f);
                    battleLogPanel.SetActive(false);

                    foreach (GameObject secondaryMonsterTile in secondaryMonsterTiles)
                    {
                        if (playersCurrentTile[currentPlayerIndex] == secondaryMonsterTile)
                        {
                            playersCurrentTile[currentPlayerIndex] = currentTile.NextBoardTiles[0];
                            players[currentPlayerIndex].transform.position = playersCurrentTile[currentPlayerIndex].transform.position;

                            secondaryMonsterCheck = true;

                            if (playersStats[currentPlayerIndex].holdingFlag)
                            {
                                playersStats[currentPlayerIndex].holdingFlag = false;

                                FlagStatus.FlagAvaliable = true;
                            }

                            break;
                        }
                    }

                    if (!secondaryMonsterCheck)
                    {
                        playersCurrentTile[currentPlayerIndex] = currentTile.gameObject;
                        players[currentPlayerIndex].transform.position = playersCurrentTile[currentPlayerIndex].transform.position;
                    }

                    secondaryMonsterCheck = false;

                    break;
                }

            } else if (newCurrentTile is FlagTile) {
                newCurrentTile.tileEffect(playersStats[currentPlayerIndex]);

                break;
            }
            else if (!(newCurrentTile is LUTile) && !(newCurrentTile is LRTile))
            {
                newCurrentTile.tileEffect(playersStats[currentPlayerIndex]);
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
