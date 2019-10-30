using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpitController : MonoBehaviour
{
    public GameObject spitGoo;
    public float spitSpeed = 0.3f;
    public Vector2 target;

    void Start()
    {
        target = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        var angle = Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    void FixedUpdate()
    {
        transform.Translate(Vector3.right * spitSpeed);
    }

    // Detect collisions on stuffs
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag != "Boss")
        {
            if (col.tag == "Player")
            {
                print("Hit Player!");
            }
            else
            {
                print(col);
                Instantiate(spitGoo, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }
}
