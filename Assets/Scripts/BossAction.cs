using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class BossAction : MonoBehaviour
{
    SpriteRenderer bossRenderer;

    int xCount;//x方向から受けた回数、縦に伸びる
    int yCount;//y方向から受けた回数、横に伸びる

    int deformationCount = 5;//変形するまでの回数
    public int life = 150;
    int dustDamage = 1;
    int needleDamage = 1;
    int attackDamage = 2;
    public int deformationTime;
    int damegeTime;

    public int currentHp;
    //Sliderを入れる
    public Slider slider;

    int hitTime;

    float knockBackPower = 1;

    public bool isXDeformation;//X方向に変形したか
    public bool isYDeformation;//Y方向に変形したか
    public bool isDamageHit;
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

    //イージング
    public Ease ease;
    //演出用変数
    public bool isDamage;
    //アニメ
    private Animator animator;
    private SpriteRenderer render;
    // フェードアウトするまでの時間(0.5sec)
    public float fadeTime = 1f;
    private float time;


    // Start is called before the first frame update
    void Start()
    {


        //登場イージング
        transform.DOScale(new Vector3(3, 3, 1), 1.5f).SetEase(ease);
        if (SceneManager.GetActiveScene().name == "Game")
        {
            //Sliderを満タンにする。
            slider.value = 1;
        }
        //現在のHPを最大HPと同じに。
        currentHp = life;
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        render = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Deformation();
        HitRest();
        Dead();
        if (SceneManager.GetActiveScene().name == "Tyutorial")
        {
            deformationCount = 3;
        }
        if (SceneManager.GetActiveScene().name == "Game")
        {
            //HP計算
            slider.value = (float)currentHp / (float)life;

            deformationCount = 5;
        }
        //ダメージを受けたときに白くする
        if (isDamageHit)
        {
            damegeTime += 1;
            if (damegeTime == 12)
            {
                isDamageHit = false;
            }
        }
        else
        {
            damegeTime = 0;
        }
    }

    //変形
    void Deformation()
    {
        if (SceneManager.GetActiveScene().name == "Game")
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
        if (SceneManager.GetActiveScene().name == "Game")
        {
            animator.SetBool("isDead", isDead);//アニメに変更
        }
        if (currentHp <= 0)
        {
            isDead = true;
        }
        if (isDead) 
        {
            time += Time.deltaTime;
            if (time < fadeTime)
            {
                float alpha = 1.0f - time / fadeTime;
                UnityEngine.Color color = render.color;
                color.a = alpha;
                render.color = color;
            }
            else
            {
                render.color = new UnityEngine.Color(1, 1, 1, 0);
            }

            gameObject.transform.localScale = new Vector3(4, 4, 1);
        }
    }

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
                    //演出オン
                    isDamage = true;
                    transform.Translate(distination.x * knockBackPower, 0f, 0f);
                    weapon.comboCount = 0;
                }

                //変形のカウント
                if (weapon.isAttack)
                {
                    //白くする
                    isDamageHit = true;
                    if (!isXDeformation && !isYDeformation)
                    {
                        yCount += 1;
                    }
                    //演出オン
                    isDamage = true;
                    currentHp = currentHp - attackDamage;
                    isHit = true;
                }
                if (weapon.isDashAttack)
                {
                    //白くする
                    isDamageHit = true;
                    if (!isXDeformation && !isYDeformation)
                    {
                        xCount += 1;
                    }
                    //演出オン
                    isDamage = true;
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
            //演出オン
            isDamage = true;
            currentHp = currentHp - dustDamage;
            isDamageHit = true;
            //当たったオブジェクトを削除する
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "Needle")
        {
            //音
            audioSource.PlayOneShot(damageAudio);
            //演出オン
            isDamage = true;
            currentHp = currentHp - needleDamage;
            isDamageHit = true;
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
