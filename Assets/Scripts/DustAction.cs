using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;

public class DustAction : MonoBehaviour
{
    private Rigidbody2D rb;

    int time;//このオブジェクトが呼び出されてからの時間

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
        //現在のポジションを保持する
        Vector3 currentPos = transform.position;

        //Mathf.ClampでX,Yの値それぞれが最小～最大の範囲内に収める。
        //物理挙動のあるisTriggerにしたいが、床は突き抜けてほしくないので無理やり範囲を決めて落ちないようにする
        currentPos.y = Mathf.Clamp(currentPos.y, -3.6f, yUpRange);

        //positionをcurrentPosにする
        transform.position = currentPos;

    }

    //最初の飛び出し
    void JumpOut()
    {
        time += 1;

        //一度だけ力を加える
        if (time == 2)
        {
            //右か左に行くのかオブジェクト名から判別
            if (gameObject.name == "RDust(Clone)")
            {
                rb.velocity = new Vector2(power, power);
            }
            if (gameObject.name == "LDust(Clone)")
            {
                rb.velocity = new Vector2(-power, power);
            }

        }
        //時間になったら移動をやめる
        //0にしないと横にスライドしていってしまう
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

    //時間でだんだん消えていく
    void FadeOut()
    {
        //当たっていなかったらだんだん消えるが当たったら上に飛んでいく
        if (!isHit)
        {
            if (time >= 60)
            {
            //点滅かだんだん消えていく

            }
            if (time >= 120)
            {
                Destroy(gameObject);
            }
        }
    }

    //ボスに当たらず上に行ったら消す
    void Delete()
    {
        //指定の座標以上に行ったら削除する
        if(transform.position.y >= yUpRange)
        {
            Destroy(gameObject);
        }
    }

    //地面に当たったら飛ばせるようにするにした方がいいかも
    //ボスも同じく当たってからボスにも当たるようにしたほうがよさげ

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
