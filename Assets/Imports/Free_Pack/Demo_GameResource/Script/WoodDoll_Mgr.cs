using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodDoll_Mgr : MonoBehaviour {


    public int sortingOrder = 0;
    public int sortingOrderOrigine = 0;
    public SpriteRenderer[] m_SpriteGroup;

    Animator m_Animator;
    // Use this for initialization
    void Start () {
        m_Animator = this.transform.GetComponent<Animator>();
        m_SpriteGroup = this.transform.GetComponentsInChildren<SpriteRenderer>(true);

        spriteOrder_Controller();

    }


    void spriteOrder_Controller()
    {
        sortingOrder = Mathf.RoundToInt(this.transform.position.y * 100);
        //Debug.Log("y::" + this.transform.position.y);
        //  Debug.Log("sortingOrder::" + sortingOrder);
        for (int i = 0; i < m_SpriteGroup.Length; i++)
        {

            m_SpriteGroup[i].sortingOrder = sortingOrderOrigine - sortingOrder;

        }

    }

    public void  Sword_Hitted()
    {
        m_Animator.Play("Hit");
  
    }

  

}
