using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLick : MonoBehaviour
{
    private BossController bc;

    public Rigidbody2D rgb;
    public Collider2D col;

    public float jumpHeight;
    public float jumpDuration = 1f;
    public float lickLag = 0.3f;
    public float lickChargeup = 0.5f;
    

    private int jumpFrames;
    private Vector2 target;


    public GameObject tongueTipPref;
    public Vector3 nuzzle;

    public Transform playerTransform;

    private void OnEnable()
    {
        transform.rotation = Quaternion.Euler(0,0,0);
        col.enabled = true;
        rgb.simulated = true;
        jumpFrames = (int)(jumpDuration / Time.fixedDeltaTime);
    }

    private void FixedUpdate()
    {

    }

    private IEnumerator Lick()
    {
        target = ((Vector2)playerTransform.position).normalized;
        

        GameObject tongue = Instantiate(tongueTipPref, nuzzle, Quaternion.identity);
        TongueController tongueController = tongue.GetComponent<TongueController>();
        tongueController.target = target;

        Disable();

        yield return null;
    }

    public IEnumerator Jump()
    {
        // yield return new WaitForSeconds(0.1f);
        rgb.simulated = false;
        col.enabled = false;
        yield return new WaitForSeconds(0.5f);

        float yPos = transform.position.y;
        float distance = playerTransform.position.x - transform.position.x;

        float axel = -8 * (jumpHeight / (jumpFrames * jumpFrames));
        // float vel = -16 * jumpHeight / (jumpFrames * jumpFrames * jumpFrames);
        float vel = -jumpFrames * axel / 2;

        for (int i = 0; i < jumpFrames-1; i++)
        {
            vel = vel + axel;
            transform.position += new Vector3(distance / jumpFrames, vel);
            yield return new WaitForFixedUpdate();
        }

        StartCoroutine("Lick");

        yield return null;
    }

    // Start is called before the first frame update
    void Start()
    {
        bc = GetComponent<BossController>();
    }

    // Update is called once per frame

    public void Disable()
    {
        bc.updateState(this);
        enabled = false;
    }

    private void OnDisable()
    {
        col.enabled = false;
        rgb.simulated = false;
        StopAllCoroutines();
    }
}
