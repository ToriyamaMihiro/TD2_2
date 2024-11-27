using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class NeedleAction : MonoBehaviour
{
    int time;//���̎���
    public int maxTime = 700;//������܂ł̎���

    float startMovePos = 8.6f;
    float endMovePos = 9.6f;

    // Start is called before the first frame update
    void Start()
    {
      
        //�E�����ɍs���̂��I�u�W�F�N�g�����画��
        if (gameObject.name == "RNeedle(Clone)")
        {
            transform.DOLocalMoveX(startMovePos, 0.7f).SetEase(Ease.InOutQuart).SetLink(gameObject);
        }
        if (gameObject.name == "LNeedle(Clone)")
        {
            transform.DOLocalMoveX(-startMovePos, 0.7f).SetEase(Ease.InOutQuart).SetLink(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {

        BossAttackAction boss;
        GameObject obj = GameObject.Find("Boss");
        boss = obj.GetComponent<BossAttackAction>();
        time += 1;
        if (time == maxTime)
        {
            boss.isNeedleSpawn = false;
            //�E�����ɍs���̂��I�u�W�F�N�g�����画��
            if (gameObject.name == "RNeedle(Clone)")
            {
                transform.DOLocalMoveX(endMovePos, 0.7f).SetEase(Ease.InOutQuart).SetLink(gameObject);
            }
            if (gameObject.name == "LNeedle(Clone)")
            {
                transform.DOLocalMoveX(-endMovePos, 0.7f).SetEase(Ease.InOutQuart).SetLink(gameObject);
            }
            //�ړ����I��邱��ɏ���
            Invoke("Delete", 1);
        }
    }

    void Delete()
    {
            Destroy(this.gameObject);

    }
}
