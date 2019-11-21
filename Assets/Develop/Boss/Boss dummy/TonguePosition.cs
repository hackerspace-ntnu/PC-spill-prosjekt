using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TonguePosition : MonoBehaviour
{
    public Transform position;

    // Update is called once per frame
    void Update()
    {
        transform.position = position.position;
    }
}
