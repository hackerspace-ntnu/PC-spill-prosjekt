using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightchainFlicker : MonoBehaviour
{
    private SpriteRenderer renderer;
    private Color32 color;
    private byte alpha = 255;

    void Start()
    {
        renderer = this.GetComponent<SpriteRenderer>();
        StartCoroutine(flicker());
    }

    private IEnumerator flicker()
    {
        byte speed = 3;
        while(true)
        {
            while(alpha>190)
            {
                alpha-=speed;
                yield return new WaitForEndOfFrame();
                color = new Color32(255, 255, 255, alpha);
                renderer.color = color;
            }
            while(alpha<255)
            {
                alpha+=speed;
                yield return new WaitForEndOfFrame();
                color = new Color32(255, 255, 255, alpha);
                renderer.color = color;
            }
            yield return new WaitForSeconds(1f);
        }
    }
    
}
