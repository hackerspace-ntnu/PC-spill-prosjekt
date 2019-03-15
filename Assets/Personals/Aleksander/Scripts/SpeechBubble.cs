using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechBubble : MonoBehaviour {

    private GameObject parent;

    public FollowObject familiar;

	// Use this for initialization
	void Start () {
		parent = transform.parent.gameObject;
        familiar = parent.GetComponent<FollowObject>();
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = familiar.GetTargetPos();
	}
}
