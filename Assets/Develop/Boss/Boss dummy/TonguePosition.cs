using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TonguePosition : MonoBehaviour
{
    public Transform position;

    void Update()
    {
        transform.position = position.position;
    }
}
