using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossJump : MonoBehaviour
{
    private Rigidbody2D rgb;
    private Collider2D col;

    public float jumpHeight;
    public float jumpDuration = 1f;
    public float lickDuration = 0.3f;
    private int jumpFrames;
    

    public Transform playerTransform;

    private void OnEnable()
    {
        col.enabled = true;
        rgb.simulated = true;
        jumpFrames = (int)(jumpDuration * 100 / Time.fixedDeltaTime);

    }

    private void FixedUpdate()
    {

    }

    private IEnumerator Jump()
    {
        float point = playerTransform.position.x;
        for (int i = 0; i < jumpFrames; i++)
        {

        }
        yield return null;
    }

    private IEnumerator Lick()
    {


        yield return null;
    }

    // Start is called before the first frame update
    void Start()
    {
        rgb = GetComponent<Rigidbody2D>();
        col = GetComponentInChildren<Collider2D>();
    }

    // Update is called once per frame

    public void Disable()
    {

    }

    private void OnDisable()
    {
        col.enabled = false;
        rgb.simulated = false;
        StopAllCoroutines();
    }
}
