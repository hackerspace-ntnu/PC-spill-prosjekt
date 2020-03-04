using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GentleCameraMovement : MonoBehaviour
{
    private int direction = 0;
    private Vector3 movementVector;

    void Start()
    {
        StartCoroutine(changeDirection());   
    }

    public IEnumerator changeDirection()
    {
        float speed = 0.005f;
        while (true)
        {
            direction += 1;
            if(direction > 3)
            {
                direction = 0;
            }
            switch (direction)
            {
                case 0:
                    movementVector = new Vector3(speed, 0, 0);
                    break;
                case 1:
                    movementVector = new Vector3(0, speed, 0);
                    break;
                case 2:
                    movementVector = new Vector3(-speed, 0, 0);
                    break;
                case 3:
                    movementVector = new Vector3(0, -speed, 0);
                    break;
            }
            yield return new WaitForSeconds(3f);
        }
    }

    void Update()
    {
        this.transform.position += movementVector;
    }
}
