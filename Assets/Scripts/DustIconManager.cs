using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DustIconManager : MonoBehaviour
{

    [SerializeField] GameObject[] icon=new GameObject[6];

    public bool[] isSet = new bool[6];
    [SerializeField] int leftIconNum;
    [SerializeField] int rightIconNum;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        //DustIcon();
    }

    void IconRandom()
    {
        AttackAction playerscript;
        GameObject obj = GameObject.Find("Weapon");
        playerscript = obj.GetComponent<AttackAction>();
        if (playerscript.isDust)
        {
            int leftNum = Random.Range(0, 6);
            int rightNum = Random.Range(0, 6);
            //表示中じゃない数字が出るまで繰り返す
            while (isSet[leftNum])//ひだり
            {
                if (!isSet[leftNum])
                {
                    leftIconNum = leftNum;
                    isSet[leftNum] = true;
                    break;
                }
                leftNum = Random.Range(0, 6);
            }

            while (isSet[rightNum])//みぎ
            {
                if (!isSet[rightNum])
                {
                    rightIconNum = rightNum;
                    isSet[rightNum] = true;
                    break;
                }
                rightNum = Random.Range(0, 6);
            }

            playerscript.isDust = false;
        }

    }

   

    void DustIcon()
    {
        int[] time= new int[5];
        //それぞれの数の場合のアイコン決める
        if (isSet[0])
        {
            icon[0].transform.localScale = new Vector3(0, 0, 1);

            time[0] += 1;
            if(time[0] >= 120)
            {
                isSet[0] = false;
                time[0] = 0;
            }
        }
        if (isSet[1])
        {
            icon[1].transform.localScale = new Vector3(0, 0, 1);
            
            time[1] += 1;
            if (time[1] >= 120)
            {
                isSet[1] = false;
                time[1] = 0;
            }
        }
        if (isSet[2])
        {
            
            icon[2].transform.localScale = new Vector3(0, 0, 1);
            
            time[2] += 1;
            if (time[2] >= 120)
            {
                isSet[2] = false;
                time[2] = 0;
            }
        }
        if (isSet[3])
        {
            
            icon[3].transform.localScale = new Vector3(0, 0, 1);
            
            time[3] += 1;
            if (time[3] >= 120)
            {
                isSet[3] = false;
                time[3] = 0;
            }
        }
        if (isSet[4])
        {
            
            icon[4].transform.localScale = new Vector3(0, 0, 1);
    
            time[4] += 1;
            if (time[4] >= 120)
            {
                isSet[4] = false;
                time[4] = 0;
            }
        }
        if (isSet[5])
        {
            
            icon[5].transform.localScale = new Vector3(0, 0, 1);
            time[5] += 1;
            if (time[5] >= 120)
            {
                isSet[5] = false;
                time[5] = 0;
            }
        }
    }

}
