using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pause : MonoBehaviour {
    [SerializeField] public GameObject pausePanel;
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
        Time.timeScale = 0;
        pausePanel.SetActive(true);
        GetComponent<MovementV2>().enabled = false;
        GetComponent<gravityChange_Terraria>().enabled = false;
        //Disable all other scripts that work while timeskip is 0
    }

    private void continueGame()
    {
        Time.timeScale = 1;
        pausePanel.SetActive(false);
        GetComponent<MovementV2>().enabled = true;
        GetComponent<gravityChange_Terraria>().enabled = true;
        //Enable said scripts
    }
}
