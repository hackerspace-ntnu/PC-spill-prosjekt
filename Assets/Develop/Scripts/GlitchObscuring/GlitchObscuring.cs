using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GlitchObscuring : MonoBehaviour
{   
    public GameObject glitchMask;
    public Transform glitchMeter;
    public GlobalScript globalScript;
    float sliderValue;
    float maskSpawnRate;

    // Start is called before the first frame update
    private void Start()
    {
        sliderValue = 0;
        maskSpawnRate = 20;
        StartCoroutine(ReduceGlitchmeter());
    }


    public void ChangeGlitchValue(int val)
    {
        sliderValue += val;
        if (sliderValue > 100)
            sliderValue = 100;
        UpdateGlitchmeter();
    }
    public void UpdateGlitchmeter()
    {
        StopAllCoroutines();
        //glitchText.text = Mathf.Round(sliderValue).ToString() + "%";
        maskSpawnRate = 1000 / (sliderValue + 1);
        //Debug.Log("slidervalue: " + sliderValue);
        //Debug.Log("Maskspawnrate: " + maskSpawnRate);
        glitchMeter.localScale = new Vector3(0.4f*(sliderValue/100), 0.05f, 1f);
        StartCoroutine(SpawnGlitchMasks());
        StartCoroutine(ReduceGlitchmeter());

    }
    public IEnumerator ReduceGlitchmeter() // Reduces glitch meter value every 0.4 seconds if no glitch is active and slidervalue is above 0
    {
        while(true)
        {
            yield return new WaitForSeconds(0.4f);
            if (sliderValue == 1)
                sliderValue = 0;

            if (sliderValue > 0 && !globalScript.glitchActive)
            {
                sliderValue-=5;
                Debug.Log(sliderValue);
                UpdateGlitchmeter();
            }
                
        }
    }
    public IEnumerator SpawnGlitchMasks()
    {
        int iMax=0;
        if (maskSpawnRate < 10)
            iMax = 13;
        else if (maskSpawnRate < 15)
            iMax = 10; // 0-15
        else if (maskSpawnRate < 20)
            iMax = 8; // 15-30
        else if (maskSpawnRate < 30)
            iMax = 5; // 30-40
        else if (maskSpawnRate < 80)
            iMax = 3;
        else if (maskSpawnRate < 140)
            iMax = 2;
        else
            iMax = 1;
        Debug.Log("maskSpawnrate: " + maskSpawnRate + ", iMax = " + iMax);
        // if the mask spawn rate is low enough to "bother" spawning glitch masks
        while(maskSpawnRate < 100)
        {
            // Spawn iMax masking objects and wait (adjustable)
            for(int i = 0; i<iMax; i++)
            {
                SpawnCircle();
            }
            yield return new WaitForSeconds(maskSpawnRate);
        }
    }
    private void SpawnCircle() // Spawn masking circle (was previously a circle, and thus the function name is SpawnCircle)
    {
        GameObject circle = Instantiate(glitchMask);
        Vector3 maincamPos = Camera.main.transform.position;
        circle.transform.position = new Vector3(maincamPos.x + Random.Range(-7, 7), maincamPos.y + Random.Range(-5, 5), 1); // TODO: ta inn main cam - koordinater
        int rand = Random.Range(0, 2);
        
        if(rand==1)
        {
            circle.transform.Rotate(new Vector3(0,0,180f));
        }
        //Debug.Log(circle.transform.rotation);
        //circle.transform.rotation = new Quaternion(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1));
        circle.SetActive(true);
    }
    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.B))
        {
            ChangeGlitchValue(10);
        }
    }
}
