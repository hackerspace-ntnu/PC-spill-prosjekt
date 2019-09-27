using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wallClip : MonoBehaviour {

    private Collider col;

    public bool clip;

	void Start () {
        col = null;
	}

    private void disableColliders()
    {
        //sets wallcollider to trigger. Has to be rethinked for what to happen with enemies. Maybe set Player collider to trigger instead.
        transform.GetChild(0).GetComponent<BoxCollider2D>().enabled = false;
        transform.GetChild(0).GetComponent<TriggerDetector>().enabled = false;
        transform.GetChild(1).GetComponent<BoxCollider2D>().enabled = false;
        transform.GetChild(1).GetComponent<TriggerDetector>().enabled = false;
        transform.GetChild(2).GetComponent<BoxCollider2D>().isTrigger = true;
        transform.GetChild(2).GetComponent<GreatCollider>().enabled = false;
    }

    private void enableColliders()
    {
        transform.GetChild(0).GetComponent<BoxCollider2D>().enabled = true;
        transform.GetChild(0).GetComponent<TriggerDetector>().enabled = true;
        transform.GetChild(1).GetComponent<BoxCollider2D>().enabled = true;
        transform.GetChild(1).GetComponent<TriggerDetector>().enabled = true;
        transform.GetChild(2).GetComponent<BoxCollider2D>().isTrigger = false;
        transform.GetChild(2).GetComponent<GreatCollider>().enabled = true;

        transform.GetChild(2).GetComponent<GreatCollider>().isColliding = false;
        GetComponent<Movement>().SetGrounded(false);
    }

    public void FixedUpdate()
    {
        if (clip)
        {
            disableColliders();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        enableColliders();
        clip = false;
    }
}
