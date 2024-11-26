using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDamageAction : MonoBehaviour
{
    SpriteRenderer bossRenderer;

    int hitTime;
    float level;

    // Start is called before the first frame update
    void Start()
    {
        bossRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Damage();
    }
    void Damage()
    {
        BossAction boss;
        GameObject obj = GameObject.Find("Boss");
        boss = obj.GetComponent<BossAction>();

        if (boss.isDamageHit)
        {
            //�v���C���[�̐F��_�ł����Ė��G���Ԃ��ƕ�����₷������
            gameObject.GetComponent<SpriteRenderer>().color = new UnityEngine.Color(1, 1, 1, 0.5f);

            //if (level <= 0.5)
            //{
            //    level += 0.05f;
            //}

            //���t���[���Ăяo�����Ȃ�����
          
        }
        else
        {
            bossRenderer.color = new UnityEngine.Color(1f, 1f, 1f, 0f);
            hitTime = 0;
        }
    }

    void WaitFor()
    {
        //�{�X�̐F�����ɖ߂�
        bossRenderer.color = new UnityEngine.Color(1f, 1f, 1f, 0f);
        hitTime = 0;
    }
}
