using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GlitchObscuring : MonoBehaviour
{   
    public GameObject glitchMask;
    public Transform glitchMeter;
    public GlobalScript globalScript;

    float maskSpawnRate; // increase to increase glitch-effect
    float sliderValue;
    float timer = 0; // mask spawn counter
    float spawnUpperLimit = 10; // decrease to increase glitch-effect

    private void Start()
    {
        sliderValue = 0;
        maskSpawnRate = 15;
        StartCoroutine(ReduceGlitchmeter());
        StartCoroutine(MaskTimer());
    }

    public IEnumerator MaskTimer()
    {
        float diff = 1.0f;
        while(true)
        {
            yield return new WaitForSeconds(diff/maskSpawnRate);
            if(sliderValue > 0)
            {
                timer += diff;
                print("Timer: " + timer);
            }
            if (timer > spawnUpperLimit)
            {
                SpawnCircle();
                timer = 0;
            }
        }
    }
    public void setSpawnUpperLimit(float sliderValue)
    {
        // low slider value = high spawn upper limit
        float inverseSliderValue = 100 - sliderValue;
        this.spawnUpperLimit = inverseSliderValue / maskSpawnRate;
    }
    


    public void ChangeGlitchValue(int val)
    {
        sliderValue += val;
        if (sliderValue > 100)
            sliderValue = 100;
        setSpawnUpperLimit(sliderValue);
    }
   
    public IEnumerator ReduceGlitchmeter() // Reduces glitch meter value every 0.4 seconds if no glitch is active and slidervalue is above 0
    {
        int sliderReductionRate = 5; // how fast the slider goes down
        while(true)
        {
            yield return new WaitForSeconds(0.4f);
            if (sliderValue <= 4)
            {
                sliderValue = 0; // set slider value to 0
                timer = 0; // reset timer
            }

            if (sliderValue > 0 && !globalScript.glitchActive) // if the slider value is above 0 and a glitch is not being used
            {
                sliderValue-= sliderReductionRate;
                Debug.Log(sliderValue);
            }
                
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
