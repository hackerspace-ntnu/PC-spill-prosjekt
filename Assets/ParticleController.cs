using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    private Color32 noteColor;
    
    // Start is called before the first frame update
    void Start()
    {
        noteColor = new Color32(255, 255, 255, 255);
        StartCoroutine(switchColor());
    }

    IEnumerator switchColor()
    {
        Random rnd = new Random();
        ParticleSystem p = this.GetComponent<ParticleSystem>();
        while (true)
        {
            byte r, g, b;
            r = (byte)Random.Range(0, 255);
            g = (byte)Random.Range(0, 255);
            b = (byte)Random.Range(0, 255);
            noteColor = new Color32(r, g, b, 255);
            p.startColor = noteColor;
            yield return new WaitForSeconds(0.5f);

        }
            
    }
}
