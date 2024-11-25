using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;
using DG.Tweening;

public class AttackAction : MonoBehaviour
{

    private Rigidbody2D rb;

    public GameObject LDust;
    public GameObject RDust;

    private TD2_2 inputAcution;

    Vector3 weponPos;//振り下ろす際の位置を取得する
    Vector3 startPos = new Vector3(-1f, 0.5f, 0);//初期化用の位置を取得
    Vector3 weaponStartPos;//振り下ろす前の初期位置

    float moveSpeed = 7;
    public float upPos = 1f;//どれだけ高いところから手を振り下ろすか
    public float sidePos = 0.8f;//どれだけ高いところから手を振り下ろすか
    public float attackMovePower = 1.3f;
    public float dashAttackMovePower = 1.3f;

    public int attackTime = 0;
    public int attackFullTime = 10;//攻撃のリセットの時間
    int comboTime = 0;
    int comboMaxTime = 200;
    public int comboCount = 0;
    public int comboCountMax = 2;

    public int XCount;
    public int YCount;

    public bool isAttack;
    public bool isDashAttack;
    bool isCombo;
    bool isFloorHit;

    Tweener tween;

    //シェイク用
    public bool isAttackShake = false;
    //音
    private AudioSource audioSource;
    public AudioClip hitAudio;
    public AudioClip flutterAudio;
    //塵アイコン用
    public bool isDust;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        inputAcution = new TD2_2();
        inputAcution.Enable();
        //手を上にあげた分下までの力を変化させるため
        attackMovePower += upPos;
        dashAttackMovePower += sidePos;

        tween = transform.DOLocalMoveY(0.9f, 1.5f).SetLoops(-1, LoopType.Yoyo);
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Attack();
        DashAttack();
        DustCall();
        Combo();
    }


    //演出上の上下
    void Move()
    {
        tween = transform.DOLocalMoveY(1f, 1.5f).SetLoops(-1, LoopType.Yoyo);
    }

    //潰す
    void Attack()
    {
        PlayerAction player;
        GameObject obj = GameObject.Find("Player");
        player = obj.GetComponent<PlayerAction>();
        ////ボタンを押したら90度回転させて振り下ろす
        if (inputAcution.Player.Attack.IsPressed() && !isAttack && !player.isJump)
        {
            transform.Rotate(0, 0, 90);
            transform.localPosition = startPos;
            //武器の初期位置を上目にする
            weaponStartPos = transform.position;
            weaponStartPos.y += upPos;
            transform.position = weaponStartPos;
            //武器の位置の取得
            weponPos = transform.position;

            isAttack = true;

            XCount += 1;
            //コンボ途切れる時間のリセット
            comboTime = 0;
            comboCount += 1;
        }
        if (isAttack)
        {
            //振り下ろしている時間をカウント
            attackTime += 1;

            //攻撃している間は歩かない
            player.nowSpeed = 0;

            //武器の移動
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(weponPos.x, weponPos.y - attackMovePower, weponPos.z), moveSpeed * Time.deltaTime);

            //振り下ろし終わったらリセットする
            if (attackTime >= attackFullTime)
            {
                isAttack = false;
                transform.Rotate(0, 0, -90);
                transform.localPosition = startPos;
                Move();
                attackTime = 0;
            }
        }
    }
    //押す
    void DashAttack()
    {
        PlayerAction player;
        GameObject obj = GameObject.Find("Player");
        player = obj.GetComponent<PlayerAction>();
        if (inputAcution.Player.DashAttack.IsPressed() && !isDashAttack && !player.isJump)
        {
            //音
            audioSource.PlayOneShot(flutterAudio);

            transform.localPosition = startPos;
            //武器の初期位置を上目にする
            weaponStartPos = transform.position;
            if (player.direction == player.transform.right)
            {
                //武器の移動
                weaponStartPos.x -= sidePos;

            }

            //プレイヤーが左を向いていたら
            if (player.direction == -player.transform.right)
            {
                //武器の移動
                weaponStartPos.x += sidePos;
            }
            transform.position = weaponStartPos;
            weponPos = transform.position;
            isDashAttack = true;
            YCount += 1;

            //コンボ途切れる時間のリセット
            comboTime = 0;
            comboCount += 1;
        }
        if (isDashAttack)
        {

            //攻撃している時間をカウント
            attackTime += 1;

            //攻撃している間は歩かない
            player.nowSpeed = 0;

            //プレイヤーが右を向いていたら
            if (player.direction == player.transform.right)
            {
                //武器の移動
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(weponPos.x + dashAttackMovePower, weponPos.y, weponPos.z), moveSpeed * Time.deltaTime);
            }

            //プレイヤーが左を向いていたら
            if (player.direction == -player.transform.right)
            {
                //武器の移動
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(weponPos.x - dashAttackMovePower, weponPos.y, weponPos.z), moveSpeed * Time.deltaTime);
            }

            //攻撃が終わったらリセットする
            if (attackTime >= attackFullTime)
            {
                isDashAttack = false;
                transform.localPosition = startPos;
                Move();
                attackTime = 0;
            }
        }
    }

    void Combo()
    {
        //なにかしら攻撃をしたとき
        if (isAttack || isDashAttack)
        {
            isCombo = true;
            tween.Kill();
        }
        if (isCombo)
        {

            comboTime += 1;
            //コンボ終了時間まで次の攻撃がなかったらコンボをリセットする
            if (comboTime >= comboMaxTime)
            {
                isCombo = false;
                comboCount = 0;
            }
            if (comboCount == comboCountMax + 1)
            {
                isCombo = false;
                comboCount = 0;
            }
        }
    }

    //床を叩いたらチリを呼び出す
    void DustCall()
    {

    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            //武器が床に付いたらチリがでる
            Instantiate(RDust, transform.position, Quaternion.identity);
            Instantiate(LDust, transform.position, Quaternion.identity);
            //塵出た瞬間か
            isDust = true;
            //シェイクする
            isAttackShake = true;
            //音
            audioSource.PlayOneShot(hitAudio);
        }
    }
}

