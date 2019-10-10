using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostTrail : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitToDie());
    }
    IEnumerator WaitToDie()
    {
        byte opacity = 200;
        while(opacity > 0)
        {
            yield return new WaitForSeconds(0.01f);
            opacity -= 5;
            this.GetComponent<SpriteRenderer>().color = new Color32(180,180,230,opacity);
        }
        Destroy(this.gameObject);
    }

    
}
