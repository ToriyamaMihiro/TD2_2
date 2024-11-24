using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DustIconManager : MonoBehaviour
{

    [SerializeField] GameObject[] icon=new GameObject[6];

    bool[] isSet = new bool[6];
    [SerializeField] int leftIconNum;
    [SerializeField] int rightIconNum;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //IconRandom();
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
        //それぞれの数の場合のアイコン決める
        if (isSet[0])
        {
            //isSet[0] = true;
            icon[0].transform.localScale = new Vector3(0, 0, 1);
        }
        if (isSet[1])
        {
            //isSet[1] = true;
            icon[1].transform.localScale = new Vector3(0, 0, 1);
        }
        if (isSet[2])
        {
            //isSet[2] = true;
            icon[2].transform.localScale = new Vector3(0, 0, 1);
        }
        if (isSet[3])
        {
            //isSet[3] = true;
            icon[3].transform.localScale = new Vector3(0, 0, 1);
        }
        if (isSet[4])
        {
            //isSet[4] = true;
            icon[4].transform.localScale = new Vector3(0, 0, 1);
        }
        if (isSet[5])
        {
            //isSet[5] = true;
            icon[5].transform.localScale = new Vector3(0, 0, 1);
        }
    }

}
