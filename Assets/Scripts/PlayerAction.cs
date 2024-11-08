using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    private Rigidbody2D rb;



    public Vector3 direction;

    public float moveSpeed = 3;
    public float jumpPower = 5;
    public float dashPower = 10;

    public int dashTime;

    public bool isDash;




    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        direction = transform.right;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Jump();
        isGround();
        DashAttack();
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
        if (Input.GetKeyDown(KeyCode.X))
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
            if (dashTime >= 40)
            {
                isDash = false;
                dashTime = 0;
            }
        }
    }


}
