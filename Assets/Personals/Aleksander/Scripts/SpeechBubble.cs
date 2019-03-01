using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechBubble : MonoBehaviour {

    GameObject parent;

	// Use this for initialization
	void Start () {
		parent = transform.parent.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
