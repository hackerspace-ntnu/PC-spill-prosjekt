using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Free_GM : MonoBehaviour {

    public GameObject[] WearGroup;


    public float MaxPage = 4;
    public float CurrPage = 1;
    public float MoveSpeed = -7;
    public float clampPower = 10;

    public Text Text_AnimState;
    public Text Text_Page;
    public Animator[] anim;
    public string[] AnimName;

    public int CurrAnimPage = 0;

	// Use this for initialization
	void Start () {

        tmptrans = transform.position;
        string tmpstring = CurrPage.ToString() + "/" + MaxPage.ToString();
        Text_Page.text = tmpstring;

        for (int i = 0; i < anim.Length; i++)
        {
            anim[i].Play(AnimName[0]);

        }

       

    }
    Vector3 tmptrans;
    // Update is called once per frame
    void FixedUpdate () {

       
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (CurrPage <= 1)
            {
               // tmptrans = new Vector3(this.transform.position.x, MaxHeight, this.transform.position.z);
                return;
            }
            CurrPage--;
            string tmpstring = CurrPage.ToString() + "/" + MaxPage.ToString();
            Text_Page.text = tmpstring;
            tmptrans = new Vector3(this.transform.position.x, CurrPage *MoveSpeed, this.transform.position.z);

            Debug.Log("위");

        }
         if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (CurrPage >= MaxPage)
            {
              
                return;
            }

            CurrPage++;
            string tmpstring = CurrPage.ToString() + "/" + MaxPage.ToString();
            Text_Page.text = tmpstring;

            tmptrans = new Vector3(this.transform.position.x, CurrPage * MoveSpeed, this.transform.position.z);

            Debug.Log("아래");



        }

    

        this.transform.position =Vector3.Lerp(this.transform.position,tmptrans,Time.deltaTime*clampPower);


        if (Input.GetKeyDown(KeyCode.RightArrow))
        {

            if (CurrAnimPage >= AnimName.Length-1)
                return;

            Debug.Log("RightArrow");
            CurrAnimPage++;
            for (int i = 0; i < anim.Length; i++)
            {
                anim[i].Play(AnimName[CurrAnimPage]);
                Text_AnimState.text = AnimName[CurrAnimPage ];
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (CurrAnimPage <=0)
                return;

            Debug.Log("LeftArrow");
            CurrAnimPage--;
            for (int i = 0; i < anim.Length; i++)
            {
          
                anim[i].Play(AnimName[CurrAnimPage]);
                Text_AnimState.text = AnimName[CurrAnimPage];
            }

          
       
        }


    }

    private bool b_takeoff = false;
    public Text Text_Takeoff;


    public void TakeOff()
    {

        Debug.Log("TakeOff");


        if (!b_takeoff)
        {
            for (int i = 0; i < WearGroup.Length; i++)
            {
                WearGroup[i].SetActive(false);
                Text_Takeoff.text = "Take ON";

            }
        }
        else
        {
            for (int i = 0; i < WearGroup.Length; i++)
            {
                WearGroup[i].SetActive(true);
                Text_Takeoff.text = "Take OFF";

            }


        }

        b_takeoff = !b_takeoff;

    }
}
