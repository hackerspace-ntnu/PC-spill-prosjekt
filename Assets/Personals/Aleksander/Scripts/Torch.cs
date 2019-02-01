using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour {

    public GameObject familiar;
    public GameObject player;
    public float minDistance;

    private Renderer mRenderer;
    private float distanceToPlayer;

    // Use this for initialization
    void Start () {
        familiar = GameObject.FindGameObjectWithTag("Familiar");
        player = GameObject.FindGameObjectWithTag("Player");
        mRenderer = GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
		if(mRenderer.isVisible) {
            distanceToPlayer = (transform.position - player.transform.position).magnitude;

            if(distanceToPlayer <= minDistance) {
                familiar.GetComponent<FollowObject>().AttachToObject(transform.gameObject, new Vector2(0.0f, 1.0f));
            }
        }

        if(!mRenderer.isVisible || distanceToPlayer > minDistance) {
            familiar.GetComponent<FollowObject>().ResetAttachment();
        }

	}
}
