using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossStartParticle : MonoBehaviour
{
    public GameObject winParticle;
    public Vector3[] pos = new Vector3[8];
    [SerializeField]
    int timer;
    [SerializeField]
    float intervalTime=10;
    [SerializeField]
    int count;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("instantiateWin",0,intervalTime);
        // 3秒後に全てのInvokeをキャンセル（止める）
        Invoke("StopRepeating", 2.5f);
    }

    // Update is called once per frame
    void Update()
    {


        //if (timer % intervalTime == 0)
        //{

        //    count++;
        //}

        ////8個出したら終わり
        //if (count > 8)
        //{
        //    timer = 0;
        //    count = 8;
        //}
        //else
        //{
        //    timer++;
        //}

    }

    void StopRepeating()
    {
        // 全てのInvokeRepeatingをキャンセル
        CancelInvoke("instantiateWin");
    }

    void instantiateWin()
    {
        Instantiate(winParticle, pos[count], Quaternion.identity);
        count++;
    }
}
