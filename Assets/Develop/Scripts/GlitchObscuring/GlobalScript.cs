using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalScript : MonoBehaviour
{
    public bool glitchActive = false;
    private void Start()
    {
        Physics2D.IgnoreLayerCollision(10, 10, true); // Necessary for secret walls. Layer 10 is the secret wall layer
    }
}
