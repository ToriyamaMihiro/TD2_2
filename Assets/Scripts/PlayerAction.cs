using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    private Rigidbody2D rb;

    public float moveSpeed = 3;
    public float jumpPower = 5;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Jump();
    }

    void Move()
    {
        this.GetComponent<Rigidbody2D>().velocity = new Vector2(0, rb.velocity.y);

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);//y方向は今のvelicityを入れる
        }

        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {

            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
        }
    }

    void Jump()
    {
        //もし地面についていたらジャンプできる
        if (Input.GetKeyDown(KeyCode.Space) && isGround())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
        }

        //神ゲー作ろう
    }

    //地面に付いているかの判定
    bool isGround()
    {
        //下にレイを飛ばしてそのレイにオブジェクトが当たったら地面についている判定を返す
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1f);
        return hit.collider != null;
    }
}
