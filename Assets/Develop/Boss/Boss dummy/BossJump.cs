using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossJump : MonoBehaviour
{
    public Vector2[] jumpPoints;
    Vector2 startPos;
    Vector2 endPos;


    private void Awake()
    {
        startPos = transform.position;
        while (startPos == endPos)
        {
            endPos = jumpPoints[Random.Range(0, jumpPoints.Length)];
        }
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
