susing System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class AttackAction : MonoBehaviour
{

    private Rigidbody2D rb;

    public GameObject LDust;
    public GameObject RDust;

    Vector3 weponPos;//振り下ろす際の位置を取得する
    Vector3 startPos = new Vector3(-1.3f, 0.45f, 0);//初期化用の位置を取得

    float movePower = 0.8f;

    public int attackTime = 0;
    public int attackFullTime = 21;//攻撃のリセットの時間
    int comboTime = 0;
    int comboMaxTime = 200;
    public int comboCount = 0;
    public int comboCountMax = 4;

    public bool isAttack;
    public bool isDashAttack;
    bool isCombo;
    bool isFloorHit;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        Attack();
        DashAttack();
        DustCall();
        Combo();
    }

    void Attack()
    {
        ////ボタンを押したら90度回転させて振り下ろす
        if (Input.GetKeyDown(KeyCode.Z) && !isAttack)
        {
            transform.Rotate(0, 0, 90);
            weponPos = transform.position;
            isAttack = true;
            //コンボ途切れる時間のリセット
            comboTime = 0;
            comboCount += 1;
        }
        if (isAttack)
        {
            //振り下ろしている時間をカウント
            attackTime += 1;

            transform.position = Vector3.MoveTowards(transform.position, new Vector3(weponPos.x, weponPos.y - movePower, weponPos.z), 2 * Time.deltaTime);

            //振り下ろし終わったらリセットする
            if (attackTime >= attackFullTime)
            {
                isAttack = false;
                transform.Rotate(0, 0, -90);
                transform.localPosition = startPos;
                attackTime = 0;
            }
        }
    }

    void DashAttack()
    {
        if (Input.GetKeyDown(KeyCode.X) && !isDashAttack)
        {
            weponPos = transform.position;
            isDashAttack = true;
            //コンボ途切れる時間のリセット
            comboTime = 0;
            comboCount += 1;
        }
        if (isDashAttack)
        {
            PlayerAction player;
            GameObject obj = GameObject.Find("Player");
            player = obj.GetComponent<PlayerAction>();

            //攻撃している時間をカウント
            attackTime += 1;

            //プレイヤーが右を向いていたら
            if (player.direction == player.transform.right)
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(weponPos.x + movePower, weponPos.y, weponPos.z), 2 * Time.deltaTime);
            }

            //プレイヤーが左を向いていたら
            if (player.direction == -player.transform.right)
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(weponPos.x - movePower, weponPos.y, weponPos.z), 2 * Time.deltaTime);
            }

            //攻撃が終わったらリセットする
            if (attackTime >= attackFullTime)
            {
                isDashAttack = false;
                transform.localPosition = startPos;
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
        }
        if (isCombo)
        {
        Debug.Log(comboCount);
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

        }
    }
}

