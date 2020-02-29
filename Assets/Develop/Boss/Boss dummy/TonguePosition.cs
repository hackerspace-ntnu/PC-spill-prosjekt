using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TongueController : MonoBehaviour
{
    public Transform position;
    public Vector2

    // Update is called once per frame
    void Update()
    {
        transform.position = position.position;
    }
}
