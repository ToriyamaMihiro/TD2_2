using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossAction : MonoBehaviour
{

    int xCount;//x方向から受けた回数、縦に伸びる
    int yCount;//y方向から受けた回数、横に伸びる

    int deformationCount = 5;//変形するまでの回数
    public int life = 150;
    int dustDamage = 1;
    int needleDamage = 1;
    int attackDamage = 2;
    public int deformationTime;

    public int currentHp;
    //Sliderを入れる
    public Slider slider;

    float knockBackPower = 1;

    public bool isXDeformation;//X方向に変形したか
    public bool isYDeformation;//Y方向に変形したか
    bool isHit;
    bool isFloor;
    bool isWall;
    public bool isDead;
    bool isKnockBack;

    //ボス変形サイズ
    public Vector3 varticalBossSize = new Vector3(1.5f, 4, 1);
    public Vector3 besideBossSize = new Vector3(4, 1.5f, 1);

    //音
    private AudioSource audioSource;
    public AudioClip damageAudio;

    // Start is called before the first frame update
    void Start()
    {
        //Sliderを満タンにする。
        slider.value = 1;
        //現在のHPを最大HPと同じに。
        currentHp = life;
        audioSource = GetComponent<AudioSource>();
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


        //ボスの攻撃中でなければ元に戻す
        BossAttackAction bossAttack = GetComponent<BossAttackAction>();

        if (bossAttack.isDeformationFinish)
        {
            isXDeformation = false;
            isYDeformation = false;
            deformationTime = 0;
            attackDamage = 2;
        }
        //それぞれの方向へ変形
        if (isXDeformation)
        {
            transform.localScale = varticalBossSize;
            attackDamage = 3;
        }
        if (isYDeformation)
        {
            transform.localScale = besideBossSize;
        }
        //変形したらカウントをリセット
        if (isXDeformation || isYDeformation)
        {
            xCount = 0;
            yCount = 0;
            deformationTime += 1;
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
        //床に居たらノックバックする
        if (collision.gameObject.tag == "Floor")
        {
            if (!isFloor)
            {
                isFloor = true;
            }
        }

        if (collision.gameObject.tag == "Weapon")
        {
            AttackAction weapon;
            GameObject obj = GameObject.Find("Weapon");
            weapon = obj.GetComponent<AttackAction>();

            if (!isHit)
            {
                //コンボがマックスかつ横攻撃だったかつ床にいた場合ノックバックする
                if (weapon.comboCount == weapon.comboCountMax && weapon.isDashAttack && isFloor)
                {
                    //コンボでのノックバック
                    Vector3 distination = (transform.position - collision.transform.position).normalized;
                    //音
                    audioSource.PlayOneShot(damageAudio);
                    transform.Translate(distination.x * knockBackPower, 0f, 0f);
                    weapon.comboCount = 0;
                }

                //変形のカウント
                if (weapon.isAttack)
                {
                    if (!isXDeformation && !isYDeformation)
                    {
                        yCount += 1;
                    }
                    currentHp = currentHp - attackDamage;
                    isHit = true;
                }
                if (weapon.isDashAttack)
                {
                    if (!isXDeformation && !isYDeformation)
                    {
                        xCount += 1;
                    }
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
            //音
            audioSource.PlayOneShot(damageAudio);
            currentHp = currentHp - dustDamage;
            //当たったオブジェクトを削除する
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "Needle")
        {
            //音
            audioSource.PlayOneShot(damageAudio);
            currentHp = currentHp - needleDamage;
            //当たったオブジェクトを削除する
            Destroy(collision.gameObject);
        }
    }


    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            if (isFloor)
            {
                isFloor = false;
            }
        }
    }
}
