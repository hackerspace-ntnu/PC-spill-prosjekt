using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCameramovement : MonoBehaviour
{
    Camera maincam;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        maincam = Camera.main;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 playerpos = new Vector3(player.transform.position.x, player.transform.position.y, -10);

        this.transform.position = Vector3.Lerp(maincam.transform.position, playerpos, 0.1f);
    }
}
