using UnityEngine;
using System.Collections;

public class NewBehaviourScript1 : MonoBehaviour
{
    private BossController bc;

    private float fallDuration = 2f;
    private float preFallPopup = 10;
    private int fallFrames;
    

    private void OnEnable()
    {
        fallFrames = (int)(fallDuration / Time.fixedDeltaTime);
    }

    // Use this for initialization
    void Start()
    {
        bc = GetComponent<BossController>();
    }

    //Yes, most of this code is stolen from BossJump
    public IEnumerator Fall()
    {
        // yield return new WaitForSeconds(0.1f);
        yield return new WaitForSeconds(0.5f);

        float yPos = transform.position.y;
        // float distance = CENTER_OF_STAGE.position.x - transform.position.x;
        float distance = -transform.position.x;

        float axel = -8 * (preFallPopup / (fallFrames * fallFrames));
        float vel = -fallFrames * axel / 2;

        for (int i = 0; i < fallFrames - 1; i++)
        {
            vel = vel + axel;
            transform.position += new Vector3(distance / fallFrames, vel);
            yield return new WaitForFixedUpdate();
        }

        Disable();

        yield return null;
    }

    public void Disable()
    {
        bc.updateState(this);
        enabled = false;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        // Spawn new boss here with
        // Instatiate(newBoss);

        // Disable BossController of this boss?
    }
}
