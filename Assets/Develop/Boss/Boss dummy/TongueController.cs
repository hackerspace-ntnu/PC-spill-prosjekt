using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TonguePhase
{
    FIRST,
    SECOND,
    THIRD
}

public class TongueController : MonoBehaviour
{
    public Vector2 target;
    public float speed = 0.5f;
    public GameObject tStart;
    public GameObject tEnd;

    private Vector2 translation;
    private TonguePhase tongueFase = TonguePhase.FIRST;

    private void Start()
    {
        Vector2 translation = (target - (Vector2)tStart.transform.position).normalized * speed;


    }
    
    void Update()
    {
        if (tongueFase == TonguePhase.FIRST)
        {
            tStart.transform.position += new Vector3(translation.x, translation.y, 0); 
            // TODO: SpriteRenderer.
        }
        else if(tongueFase == TonguePhase.SECOND)
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
