using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlitchMask : MonoBehaviour
{
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
         
    }
    private void LateUpdate()
    {
        // Reduce mask size and destroy mask
        this.transform.localScale -= new Vector3(0.01f,0.001f,0f);
        
        if(this.transform.localScale.x < 2.7)
        {
            Destroy(this.gameObject);
        }
    }
}
