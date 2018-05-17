using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour {
    public GameObject rollDiceUI;

    [HideInInspector]
    public GameStates currentState;

    public enum GameStates
    {
        PregameSetting,
        Player1Turn,
        Player1Move,
        Player2Turn,
        Player2Move,
        Combat,
        GameOver
    }

	// Use this for initialization
	void Start () {
        currentState = GameStates.PregameSetting;
	}
	
	// Update is called once per frame
	void Update () {
		switch (currentState)
        {
            case (GameStates.PregameSetting):
                currentState = GameStates.Player1Turn;
                break;
            case (GameStates.Player1Turn):
                rollDiceUI.SetActive(true);
                break;
            case (GameStates.Player1Move):
                rollDiceUI.SetActive(false);
                currentState = GameStates.Player2Turn;
                break;
            case (GameStates.Player2Turn):
                rollDiceUI.SetActive(true);
                break;
            case (GameStates.Player2Move):
                rollDiceUI.SetActive(false);
                currentState = GameStates.Player1Turn;
                break;
            case (GameStates.Combat):
                break;
            case (GameStates.GameOver):
                break;
        }
	}
}
