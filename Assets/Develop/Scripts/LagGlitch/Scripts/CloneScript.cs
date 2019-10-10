using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneScript : MonoBehaviour
{
    public SpriteLoad spriteLoad;
    public LagGlitch lGlitch;
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.H))
        {
            if(lGlitch.ghostActive)
                this.GetComponent<SpriteRenderer>().sprite = spriteLoad.playerRegular;
        }
    }
}
