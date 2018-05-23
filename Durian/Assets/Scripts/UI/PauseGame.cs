using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour {
    public GameObject pauseMenu;

    private GameStateManager gm;
    private bool paused;

    void Awake()
    {
        pauseMenu.SetActive(false);
        paused = false;
        gm = GetComponent<GameStateManager>();
    }

    // Update is called once per frame
    void Update() {
	    if (Input.GetKeyDown(KeyCode.Escape) && gm.currentState != GameStateManager.GameStates.GameOver)
        {
            Pause();
        }
	}

    public void Pause()
    {
        if (!paused)
        {
            paused = true;
            pauseMenu.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            paused = false;
            pauseMenu.SetActive(false);
            Time.timeScale = 1;
        }
    }
}
