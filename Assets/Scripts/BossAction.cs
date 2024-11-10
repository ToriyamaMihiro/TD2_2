using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossAction : MonoBehaviour
{

    int xCount;//x方向から受けた回数、縦に伸びる
    int yCount;//y方向から受けた回数、横に伸びる

    int deformationCount = 3;//変形するまでの回数
    int life = 100;
    int dustDamage = 1;
    int attackDamage = 2;

    int currentHp;
    //Sliderを入れる
    public Slider slider;

    bool isXDeformation;//X方向に変形したか
    bool isYDeformation;//Y方向に変形したか
    bool isHit;
    public bool isDead;

    // Start is called before the first frame update
    void Start()
    {
        //Sliderを満タンにする。
        slider.value = 1;
        //現在のHPを最大HPと同じに。
        currentHp = life;
    }

    // Update is called once per frame
    void Update()
    {
        Deformation();
        HitRest();
        Dead();
        //HP計算
        slider.value = (float)currentHp / (float)life;
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
            if (!weapon.isAttack && !weapon.isDashAttack)
            {
                isHit = false;
            }
        }
    }

    void Dead()
    {
        if (currentHp <= 0)
        {
            isDead = true;
        }
    }

    //変形の処理
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Weapon")
        {
            AttackAction weapon;
            GameObject obj = GameObject.Find("Weapon");
            weapon = obj.GetComponent<AttackAction>();

            //変形のカウント
            if (!isHit)
            {
                if (weapon.isAttack)
                {
                    yCount += 1;
                    currentHp = currentHp - attackDamage;
                    isHit = true;
                }
                if (weapon.isDashAttack)
                {
                    xCount += 1;
                    currentHp = currentHp - attackDamage;
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

    //Dustのダメージ処理
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Dust")
        {
            currentHp = currentHp - dustDamage;
            //当たったオブジェクトを削除する
            Destroy(collision.gameObject);
        }
    }
}
