using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class AttackAction : MonoBehaviour
{

    private Rigidbody2D rb;

    Vector3 weponPos;//振り下ろす際の位置を取得する
    Vector3 startPos = new Vector3(-1.3f, 0.45f, 0);//初期化用の位置を取得

    float movePower = 0.8f;

    public int attackTime = 0;
    public int attackFullTime = 201;//攻撃のリセットの時間

    public bool isAttack;
    public bool isDashAttack;

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
    }

    void Attack()
    {
        ////ボタンを押したら90度回転させて振り下ろす
        if (Input.GetKeyDown(KeyCode.Z) && !isAttack)
        {
            transform.Rotate(0, 0, 90);
            weponPos = transform.position;
            isAttack = true;
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
        if (Input.GetKeyDown(KeyCode.C) && !isDashAttack)
        {
            weponPos = transform.position;
            isDashAttack = true;
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
}

