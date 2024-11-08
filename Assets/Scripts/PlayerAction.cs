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
    }

    void Move()
    {
        this.GetComponent<Rigidbody2D>().velocity = new Vector2(0, rb.velocity.y);

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);//y方向は今のvelicityを入れる
            direction = -transform.right;//左を向いている
        }

        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
            direction = transform.right;//右を向いている
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
