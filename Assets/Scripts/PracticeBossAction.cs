using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.Mathematics;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class PracticeBossAction : MonoBehaviour
{
    public GameObject Boss2;

    int time;

    int moveTime;

    // Start is called before the first frame update
    void Start()
    {
        //�o��C�[�W���O
        //  transform.DOScale(new Vector3(3, 3, 1), 3f).SetEase(Ease.InOutCubic).SetLink(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

        //���߂̗��K�p�̓G�͏���

        TyutorialManager tyutorial;
        GameObject objT = GameObject.Find("TyutorialManager");
        tyutorial = objT.GetComponent<TyutorialManager>();

        if (tyutorial.isXAttack)
        {
            time += 1;
        }

        BossAction boss;
        GameObject obj = GameObject.Find("PracticeBoss(Clone)");
        boss = obj.GetComponent<BossAction>();
        if (time == 49)
        {
            //�܂��ό`���Ă��܂��̂ŏ��������Ă���
            boss.isXDeformation = false;
            boss.isYDeformation = false;
            boss.deformationTime = 0;
            moveTime = 0;

        }

        if (time == 50)
        {
            //�ޏ�C�[�W���O
            //�ό`�����0�܂ōs���Ă���Ȃ�
            transform.localScale = new Vector3(3, 3, 1);
            transform.position = new Vector3(0, -2.5f, 0);
            transform.DOScale(new Vector3(0, 0, 0), 0.5f).SetEase(Ease.InOutCubic).SetLoops(2, LoopType.Yoyo).SetLink(gameObject);
        }
        if (boss.isXDeformation)
        {
            moveTime += 1;
            if (moveTime == 1)
            {
                //�����ɂȂ����珰�ɒ����悤�Ɉړ�
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), 100 * Time.deltaTime);
               
            }
        }


       else if (boss.isYDeformation)
        {
            moveTime += 1;
            if (moveTime == 1)
            {
                //�����ɂȂ����珰�ɒ����悤�Ɉړ�
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, transform.position.y - 0.8f, transform.position.z), 100 * Time.deltaTime);
            }
        }
    }
}
