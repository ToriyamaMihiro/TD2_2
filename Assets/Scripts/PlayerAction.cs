using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    private Rigidbody2D rb;
    public SpriteRenderer playerRenderer;


    public Vector3 direction;

    public float moveSpeed = 3;
    public float jumpPower = 5;
    public float dashPower = 5;

    public int dashTime;
    int life = 10;

    public bool isDash;
    public bool isDead;
    bool isHit;



    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        playerRenderer = gameObject.GetComponent<SpriteRenderer>();
        direction = transform.right;//初期の向いている向き
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Jump();
        isGround();
        DashAttack();
        Damage();
    }

    void Move()
    {
        AttackAction weapon;
        GameObject obj = GameObject.Find("Weapon");
        weapon = obj.GetComponent<AttackAction>();

        //ダッシュ時もxの値を0にし続けるとxを動かすダッシュができなくなってしまうため
        if (!isDash)
        {
            this.GetComponent<Rigidbody2D>().velocity = new Vector2(0, rb.velocity.y);
        }
        //攻撃中でなければ移動できる
        if (!weapon.isAttack && !isDash)
        {

            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);//y方向は今のvelicityを入れる
                transform.rotation = Quaternion.Euler(0, 0, 0);//見た目を左向かせる
                direction = -transform.right;//値的に左を向いている
            }

            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
                transform.rotation = Quaternion.Euler(0, 180, 0);//見た目を右向かせる
                direction = transform.right;//値的に右を向いている
            }
        }
    }

    void Jump()
    {
        //もし地面についていたらジャンプできる
        if (Input.GetKeyDown(KeyCode.Space) && isGround())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
        }

        //神ゲーつくった
    }

    //地面に付いているかの判定
    bool isGround()
    {
        int layerMask = 1 << 6;
        //オブジェクトのレイヤーに該当するLayerをつけ忘れないように
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.6f, layerMask);
        return hit.collider != null;

    }

    void DashAttack()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            //もし右を向いて居たら右にダッシュ
            if (direction == transform.right)
            {
                rb.velocity = new Vector2(dashPower, rb.velocity.y);
            }
            //もし左を向いて居たら左にダッシュ
            if (direction == -transform.right)
            {
                rb.velocity = new Vector2(-dashPower, rb.velocity.y);
            }

            isDash = true;
        }
        //ダッシュのクールタイム
        if (isDash)
        {
            dashTime += 1;
            if (dashTime >= 10)
            {
                isDash = false;
                dashTime = 0;
            }
        }
    }

    void Damage()
    {
        if (isHit)
        {
            //プレイヤーの色を点滅させて無敵時間だと分かりやすくする
            float level = Mathf.Abs(Mathf.Sin(Time.time * 10));
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, level);
        }
    }

    //無敵時間
    IEnumerator WaitForIt()
    {
        // 3�b�ԏ������~�߂�
        yield return new WaitForSeconds(1.0f);

        //�P�b��_���[�W�t���O��false�ɂ��ē_�ł�߂�
        isHit = false;
        //プレイヤーの色を元に戻す
        playerRenderer.color = new Color(1f, 1f, 1f, 1f);
    }

    void Dead()
    {
        if (life <= 0)
        {
            isDead = true;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Boss")
        {
            if (!isHit)
            {
                //一定時間無敵になる
                StartCoroutine("WaitForIt");
                life -= 1;
                isHit = true;
            }
        }
    }
}
