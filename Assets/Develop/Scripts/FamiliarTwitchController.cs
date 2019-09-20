using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FamiliarTwitchController : MonoBehaviour
{
    public FamiliarController scriptRef;
    public float twitchFactor;
    public float twitchTolerance;

    private Rigidbody2D pRBody;
    private GameObject objToFollow;

    // Start is called before the first frame update
    void Start()
    {
        pRBody = GetComponentInParent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        objToFollow = scriptRef.GetTarget();

        // Set random twitch movment when familiar is close to player position.
        if (objToFollow.tag == "Player" && pRBody.velocity.magnitude < twitchTolerance) {
            transform.localPosition = new Vector3(Random.Range(-twitchFactor, twitchFactor), Random.Range(-twitchFactor, twitchFactor));
        } else {
            transform.localPosition = new Vector3(0.0f, 0.0f);
        }
    }

    private void FixedUpdate() {
        
    }
}
