using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class SpeechBubble : MonoBehaviour {

    public FamiliarController familiar;
    public GameObject panel;
    public Text bubbleText;

    private RectTransform textboxPanel;
    private Direction direction;

    // Use this for initialization
    void Start () {
        panel.SetActive(false);
        direction = Direction.RIGHT;
        textboxPanel = panel.GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	void Update () {
        
	}


    // Flip side of speechBubble if directions are different.
    public void Flip(Direction dir) {
        if(dir != direction) {
            textboxPanel.anchoredPosition = new Vector2(textboxPanel.anchoredPosition.x * -1.0f, textboxPanel.anchoredPosition.y);
            direction = dir;
        }
    }

    public void ActivatePanel(bool active) {
        panel.SetActive(active);
    }

    public void ActivatePanel(bool active, string text) {
        panel.SetActive(active);
        bubbleText.text = text;
    }
}
