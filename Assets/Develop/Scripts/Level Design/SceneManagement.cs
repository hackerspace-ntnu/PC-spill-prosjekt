using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManagement : MonoBehaviour
{
    public SceneControls sceneControls;
    private void OnTriggerEnter2D(Collider2D other)
    {
        sceneControls.FadeOutAndChangeScene(1);
    }
    
}
