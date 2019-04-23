﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour {

    private GameObject familiar;
    private FamiliarController fController;

    private bool hasShownText;
    private float textTimer;

    // Use this for initialization
    void Start () {
        familiar = GameObject.FindGameObjectWithTag("Familiar");
        fController = familiar.GetComponent<FamiliarController>();
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    private void OnTriggerStay2D(Collider2D collision) {
        if(collision.gameObject.tag == "Player") {
            familiar.GetComponent<FamiliarController>().AttachToObject(transform.gameObject, new Vector2(0.0f, 1.0f));
            fController.ActivateSpeecBubble(true, "I can help light the way!");
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        familiar.GetComponent<FamiliarController>().ResetAttachment();
        fController.ActivateSpeecBubble(false);
    }
}