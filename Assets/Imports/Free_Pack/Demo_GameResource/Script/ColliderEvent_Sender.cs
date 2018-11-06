using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderEvent_Sender : MonoBehaviour {
    private CharacterController_2D m_parent;
    void Start()
    {

        m_parent = this.transform.root.transform.GetComponent<CharacterController_2D>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
    
        if (m_parent.Once_Attack)
            return;

        other.GetComponent<WoodDoll_Mgr>().Sword_Hitted();
        Debug.Log("hit::" + other.name);

        if (this.GetComponent<BoxCollider2D>().enabled)
        {
            m_parent.Once_Attack = true;

        }

    }


}
