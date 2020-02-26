using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObscuringFollow : MonoBehaviour
{
    public bool obscuringActive = true;

    public Camera mainCam;
    public GameObject glitchBg;
    public GameObject regularBg;
    public Camera glitchCam;
    private void Start()
    {
        
    }
    void LateUpdate()
    {
        Vector3 mainPos = mainCam.transform.position;
        if (obscuringActive)
        {
            glitchCam.transform.position = new Vector3(mainPos.x, mainPos.y, glitchCam.transform.position.z);
            glitchBg.transform.position = new Vector3(mainPos.x, mainPos.y, glitchBg.transform.position.z);
            regularBg.transform.position = new Vector3(mainPos.x, mainPos.y, glitchBg.transform.position.z);
        }
    }
}
