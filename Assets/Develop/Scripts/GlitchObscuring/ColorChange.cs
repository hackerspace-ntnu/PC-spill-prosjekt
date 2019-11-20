using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChange : MonoBehaviour
{
    Color32[] colors;
    // Start is called before the first frame update
    void Start()
    {
        // Lager en ny liste med tilfeldige farger
        colors = new Color32[10];
        for (int i = 0; i<10; i++)
        {
            byte r = (byte)Random.Range(0, 255);
            byte g = (byte)Random.Range(0, 255);
            byte b = (byte)Random.Range(0, 255);
            Color32 random = new Color32(r, g, b, 1);
            colors[i] = random;
        }
        StartCoroutine(SwitchSkyboxColor());

    }
    public IEnumerator SwitchSkyboxColor()
    {
        while(true) // Alltid (TODO: ha noe i global script som styrer når dette skal skje)
        {
            this.gameObject.GetComponent<Camera>().backgroundColor = colors[Random.Range(0, 9)]; // Skift skybox-farge
            yield return new WaitForSeconds(2f); // Vent i 2 sekunder 
        }
    }
}
