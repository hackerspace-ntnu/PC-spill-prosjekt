using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathController : MonoBehaviour
{
    [SerializeField]
    VirusController controller;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        controller.die();
    }
}
