using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAction : MonoBehaviour
{

    int xCount;//x��������󂯂��񐔁A�c�ɐL�т�
    int yCount;//y��������󂯂��񐔁A���ɐL�т�

    int deformationCount = 3;//�ό`����܂ł̉�

    bool isXDeformation;//X�����ɕό`������
    bool isYDeformation;//Y�����ɕό`������
    bool isHit;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Deformation();
        HitRest();
    }

    //�ό`
    void Deformation()
    {
        //�ό`������J�E���g�����Z�b�g
        if (isXDeformation || isYDeformation)
        {
            xCount = 0;
            yCount = 0;
        }

        //���ꂼ��̕����֕ό`
        if (isXDeformation)
        {
            transform.localScale = new Vector3(1.5f, 4, 1);
        }
        if (isYDeformation)
        {
            transform.localScale = new Vector3(4, 1.5f, 1);
        }
    }

    //�U���̂��тɃJ�E���g�����邽�߂̃��Z�b�g
    void HitRest()
    {
        if (isHit)
        {
            AttackAction weapon;
            GameObject obj = GameObject.Find("Weapon");
            weapon = obj.GetComponent<AttackAction>();

            //�U�����I������isHit��false�ɂ��čU���̂��тɃJ�E���g�����悤�ɂ���
            //isHit��false�ɂ�������weapon.attackTime��0�ɂȂ��Ă��܂��̂�-1���Ă���
            if (weapon.attackTime >= weapon.attackFullTime - 1)
            {
                isHit = false;
            }
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Weapon")
        {
            AttackAction weapon;
            GameObject obj = GameObject.Find("Weapon");
            weapon = obj.GetComponent<AttackAction>();

            if (!isHit)
            {
                if (weapon.isAttack)
                {
                    yCount += 1;
                    isHit = true;
                }
                if (weapon.isDashAttack)
                {
                    xCount += 1;
                    isHit = true;
                }
            }
            if (xCount == deformationCount)
            {
                isXDeformation = true;
            }
            if (yCount == deformationCount)
            {
                isYDeformation = true;
            }
        }
    }
}
