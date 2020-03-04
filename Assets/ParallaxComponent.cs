using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxComponent : MonoBehaviour
{
    public float distance;
    private Transform camTransform;
    private float prevX;
    private float prevY;
    // Start is called before the first frame update
    void Start()
    {
        camTransform = Camera.main.transform;
        prevX = camTransform.position.x;
        prevY = camTransform.position.y;
    }

    // Update is called once per frame
    float speed = 10f;
    float thisOffset = 0;
    void LateUpdate()
    {
        float newX = camTransform.position.x;
        float newY = camTransform.position.y;
        if(newX!=prevX || newY!=prevY)
        {
            float offsetX = (newX-prevX) * distance;
            float offsetY = (newY - prevY) * distance;
            this.transform.position = new Vector3(Mathf.Lerp(this.transform.position.x,this.transform.position.x + offsetX,0.15f), Mathf.Lerp(this.transform.position.y, this.transform.position.y + offsetY, 0.15f), this.transform.position.z);
        }
        prevX = newX;
        prevY = newY;
    }
}
