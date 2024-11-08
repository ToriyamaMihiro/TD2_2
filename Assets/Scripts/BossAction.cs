using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAction : MonoBehaviour
{

    int xCount;//x方向から受けた回数、縦に伸びる
    int yCount;//y方向から受けた回数、横に伸びる

    int deformationCount = 3;//変形するまでの回数

    bool isXDeformation;//X方向に変形したか
    bool isYDeformation;//Y方向に変形したか
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

    //変形
    void Deformation()
    {
        //変形したらカウントをリセット
        if (isXDeformation || isYDeformation)
        {
            xCount = 0;
            yCount = 0;
        }

        //それぞれの方向へ変形
        if (isXDeformation)
        {
            transform.localScale = new Vector3(1.5f, 4, 1);
        }
        if (isYDeformation)
        {
            transform.localScale = new Vector3(4, 1.5f, 1);
        }
    }

    //攻撃のたびにカウントさせるためのリセット
    void HitRest()
    {
        if (isHit)
        {
            AttackAction weapon;
            GameObject obj = GameObject.Find("Weapon");
            weapon = obj.GetComponent<AttackAction>();

            //攻撃が終ったらisHitをfalseにして攻撃のたびにカウントされるようにする
            //isHitをfalseにするより先にweapon.attackTimeが0になってしまうので-1している
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
