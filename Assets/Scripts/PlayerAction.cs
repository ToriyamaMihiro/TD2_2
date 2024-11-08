using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    private Rigidbody2D rb;



    Vector3 direction;

    public float moveSpeed = 3;
    public float jumpPower = 5;




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
        Attack();
    }

    void Move()
    {
        AttackAction weapon;
        GameObject obj = GameObject.Find("Weapon");
        weapon = obj.GetComponent<AttackAction>();

        this.GetComponent<Rigidbody2D>().velocity = new Vector2(0, rb.velocity.y);
        //攻撃中でなければ移動できる
        if (!weapon.isAttack)
        {

            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);//y方向は今のvelicityを入れる
                transform.rotation = Quaternion.Euler(0, 0, 0);//左向いてる
            }

            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
                transform.rotation = Quaternion.Euler(0, 180, 0);//右向いてる
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

    void Attack()
    {

    }


}
