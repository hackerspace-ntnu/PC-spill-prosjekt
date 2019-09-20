using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_animation_event : MonoBehaviour {

    public bool Debug_Enabled = false;

    public AK.Wwise.Event Footstep;
    public AK.Wwise.Event Jump;
    public AK.Wwise.Event Double_Jump;

    // Use this for initialization
    public void PlayFootstepSound () {
        if (Debug_Enabled) { Debug.Log("Footstep sound triggered."); }
        Footstep.Post(gameObject);
	}

    public void PlayJumpSound()
    {
        if (Debug_Enabled) { Debug.Log("Jump sound triggered."); }
        Double_Jump.Post(gameObject);
    }

    public void PlayDoubleJumpSound()
    {
        if (Debug_Enabled) { Debug.Log("Jump sound triggered."); }
        Double_Jump.Post(gameObject);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
