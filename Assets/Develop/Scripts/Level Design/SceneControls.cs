using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControls : MonoBehaviour
{
    private bool fading = false;
    public void FadeOutAndChangeScene(int scene)
    {
        if(!fading)
        {
            Debug.Log("Changing scene");
            fading = true;
            StartCoroutine(fadeOutAndChange(scene));
        }
        
    }
    public void FadeIn()
    {
        StartCoroutine(fadeIn());
    }

    
    IEnumerator fadeOutAndChange(int scene)
    {
        SpriteRenderer sr = this.GetComponent<SpriteRenderer>();
        Color32 c = sr.color;
        byte alpha = 0;
        while(alpha < 255)
        {
            alpha += 15;
            
            c = new Color32(0, 0, 0, alpha);
            sr.color = c;

            yield return new WaitForEndOfFrame();
        }
        SceneManager.LoadSceneAsync(scene);
    }
    IEnumerator fadeIn()
    {
        SpriteRenderer sr = this.GetComponent<SpriteRenderer>();
        Color32 c = sr.color;
        byte alpha = 255;
        while (alpha > 0)
        {
            alpha -= 15;
            c = new Color32(0, 0, 0, alpha);
            yield return new WaitForEndOfFrame();
        }
    }

}
