using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;

public class DustAction : MonoBehaviour
{
    private Rigidbody2D rb;

    int time;//���̃I�u�W�F�N�g���Ăяo����Ă���̎���

    float power = 3;
    float attackSpeed = 10;
    float yUpRange = 6.5f;

    bool isHit;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        Range();
        JumpOut();
        Attack();
        Delete();
        FadeOut();
    }

    void Range()
    {
        //���݂̃|�W�V������ێ�����
        Vector3 currentPos = transform.position;

        //Mathf.Clamp��X,Y�̒l���ꂼ�ꂪ�ŏ��`�ő�͈͓̔��Ɏ��߂�B
        //���������̂���isTrigger�ɂ��������A���͓˂������Ăق����Ȃ��̂Ŗ������͈͂����߂ė����Ȃ��悤�ɂ���
        currentPos.y = Mathf.Clamp(currentPos.y, -3.6f, yUpRange);

        //position��currentPos�ɂ���
        transform.position = currentPos;

    }

    //�ŏ��̔�яo��
    void JumpOut()
    {
        time += 1;

        //��x�����͂�������
        if (time == 2)
        {
            //�E�����ɍs���̂��I�u�W�F�N�g�����画��
            if (gameObject.name == "RDust(Clone)")
            {
                rb.velocity = new Vector2(power, power);
            }
            if (gameObject.name == "LDust(Clone)")
            {
                rb.velocity = new Vector2(-power, power);
            }

        }
        //���ԂɂȂ�����ړ�����߂�
        //0�ɂ��Ȃ��Ɖ��ɃX���C�h���Ă����Ă��܂�
        if (time == 30)
        {
            rb.velocity = new Vector2(0, 0);
        }
    }

    void Attack()
    {
        if (isHit)
        {
            rb.velocity = new Vector2(0, attackSpeed);
        }
    }

    //���Ԃł��񂾂�����Ă���
    void FadeOut()
    {
        //�������Ă��Ȃ������炾�񂾂�����邪�����������ɔ��ł���
        if (!isHit)
        {
            if (time >= 60)
            {
            //�_�ł����񂾂�����Ă���

            }
            if (time >= 120)
            {
                Destroy(gameObject);
            }
        }
    }

    //�{�X�ɓ����炸��ɍs���������
    void Delete()
    {
        //�w��̍��W�ȏ�ɍs������폜����
        if(transform.position.y >= yUpRange)
        {
            Destroy(gameObject);
        }
    }

    //�n�ʂɓ����������΂���悤�ɂ���ɂ���������������
    //�{�X���������������Ă���{�X�ɂ�������悤�ɂ����ق����悳��

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {

            if (time >= 60 && !isHit)
            {
                isHit = true;
            }
        }
    }
}
