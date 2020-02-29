using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TongueController : MonoBehaviour
{
    public Vector2 target;
    public float speed = 0.5f;
    public GameObject tStart;
    public GameObject tEnd;

    private Vector2 translation;
    private int tongueFase = 1;

    private void Start()
    {
        Vector2 translation = (target - (Vector2)tStart.transform.position).normalized * speed;


    }

    // Update is called once per frame
    void Update()
    {
        if (tongueFase == 1)
        {
            tStart.transform.position += new Vector3(translation.x, translation.y, 0);
            SpriteRenderer.
        }
        else if(tongueFase == 2)
        {

        }
    }

    void DrawLine(Vector3 start, Vector3 end, Color color, float duration = 0.2f)
    {
        GameObject myLine = new GameObject();
        myLine.transform.position = start;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
        lr.SetColors(color, color);
        lr.SetWidth(0.1f, 0.1f);
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        GameObject.Destroy(myLine, duration);
    }
}
