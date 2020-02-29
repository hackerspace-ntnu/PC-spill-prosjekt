using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraColliderIgnore : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        BoxCollider2D collider = this.GetComponent<BoxCollider2D>();
        Physics.IgnoreLayerCollision(12, 0, true);
    }
}
