using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class pause : MonoBehaviour {
    [SerializeField] public GameObject pausePanel;

    private bool paused;

    void Start ()
    {
        pausePanel.SetActive(false);
	}
	
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (!pausePanel.activeInHierarchy)
            {
                pauseGame();
            }
            else
            {
                continueGame();
            }
        }
	}

    private void pauseGame()
    {
        if (GetComponent<crouchReplacer>().isCrouching)
        {
            GetComponent<wallClip>().clip = true;
        }
        Time.timeScale = 0;
        pausePanel.SetActive(true);
        GetComponent<Movement>().enabled = false;
        GetComponent<gravityChange>().enabled = false;

        //Disable all other scripts that work while timeskip is 0
    }

    private void continueGame()
    {
        Time.timeScale = 1;
        pausePanel.SetActive(false);
        GetComponent<Movement>().enabled = true;
        GetComponent<gravityChange>().enabled = true;
        //Enable said scripts
    }
}
