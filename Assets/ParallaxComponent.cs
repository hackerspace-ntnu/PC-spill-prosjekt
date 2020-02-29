using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxComponent : MonoBehaviour
{
    public float distance;
    private float x;
    private Transform camTransform;
    private float prevX;
    // Start is called before the first frame update
    void Start()
    {
        camTransform = Camera.main.transform;
        x = this.transform.position.x - Camera.main.transform.position.x;
        prevX = camTransform.position.x;
    }

    // Update is called once per frame
    float speed = 10f;
    float thisOffset = 0;
    void LateUpdate()
    {
        float newX = camTransform.position.x;
        if(newX!=prevX)
        {
            float offset = (newX-prevX) * distance;
            this.transform.position = new Vector3(Mathf.Lerp(this.transform.position.x,this.transform.position.x + offset,0.15f), this.transform.position.y, this.transform.position.z);
        }
        prevX = newX;
    }
}
