using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.UI;

public class PlayerAction : MonoBehaviour
{
    private Rigidbody2D rb;
    SpriteRenderer playerRenderer;
    private TD2_2 inputAcution;

    public Image image;

    bool isDamage;

    public Vector3 direction;

    Vector2 inputDirection;

    public float moveSpeed = 5;
    public float jumpPower = 5;
    public float dashPower = 5;

    int dashTime;
    public int life = 10;
    int jumpCount;

    public int tyutorialJumpCount;

    bool isDash;
    int hitTime;
    public bool isDead;
    public bool isHit;

    //アニメ
    public float nowSpeed;//判定用のスピード
    public bool isJump;
    private Animator animator;
    //音
    private AudioSource audioSource;
    public AudioClip jumpAudio;
    public AudioClip damageAudio;
    public AudioClip hitAudio;


    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        playerRenderer = gameObject.GetComponent<SpriteRenderer>();
        direction = -transform.right;//初期の向いている向き
        //コントローラーを使うためのもの
        inputAcution = new TD2_2();
        inputAcution.Enable();
        animator = GetComponent<Animator>();
        image = GetComponent<Image>();
        audioSource = GetComponent<AudioSource>();
    }


    // Update is called once per frame
    void Update()
    {
        Move();
        Jump();
        isGround();
        DashAttack();
        Damage();
        Dead();
        Range();
    }

    void Move()
    {
        AttackAction weapon;
        GameObject obj = GameObject.Find("Weapon");
        weapon = obj.GetComponent<AttackAction>();

        animator.SetFloat("Speed", nowSpeed);//移動アニメ

        //ダッシュ時もxの値を0にし続けるとxを動かすダッシュができなくなってしまうため
        if (!isDash)
        {
            this.GetComponent<Rigidbody2D>().velocity = new Vector2(0, rb.velocity.y);
        }
        //攻撃中でなければ移動できる
        if (!weapon.isAttack && !weapon.isDashAttack && !isDash)
        {
            rb.velocity = new Vector2(inputDirection.x * moveSpeed, rb.velocity.y);
            nowSpeed = moveSpeed;

            //ボタン押してなかったら
            if (inputDirection.x == 0)
            {
                nowSpeed = 0;//スピードを０にして立ち止まらせる
            }

            //if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            //{
            //    rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);//y方向は今のvelicityを入れる
            //    transform.rotation = Quaternion.Euler(0, 0, 0);//見た目を左向かせる
            //    direction = -transform.right;//値的に左を向いている
            //    nowSpeed = moveSpeed;//今のスピード
            //}
            //else
            //{
            //    nowSpeed = 0;//今のスピード
            //}

            //    if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            //{
            //    rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
            //    transform.rotation = Quaternion.Euler(0, 180, 0);//見た目を右向かせる
            //    direction = transform.right;//値的に右を向いている
            //    nowSpeed = moveSpeed;//今のスピード
            //}


        }


        if (!weapon.isAttack && !weapon.isDashAttack)
        {
            if (inputDirection.x < 0)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);//見た目を左向かせる
                direction = -transform.right;//値的に左を向いている
            }
            if (inputDirection.x > 0)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);//見た目を右向かせる
                direction = transform.right;//値的に右を向いている
            }
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        //移動方向の入力情報がInputdirectionの中に入るようにする
        inputDirection = context.ReadValue<Vector2>();


    }

    void Jump()
    {
        animator.SetBool("isJump", isJump);//ジャンプアニメに変更

        AttackAction weapon;
        GameObject obj = GameObject.Find("Weapon");
        weapon = obj.GetComponent<AttackAction>();

        //もし地面についていたらジャンプできる
        if (inputAcution.Player.Jump.WasPressedThisFrame() && jumpCount < 1)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
            jumpCount += 1;
            tyutorialJumpCount += 1;
            audioSource.PlayOneShot(jumpAudio);//音
        }

        //アニメ用のジャンプ処理
        if (isGround())
        {
            isJump = false;//ジャンプ
            jumpCount = 0;

        }
        else
        {
            isJump = true;
        }
    }

    //地面に付いているかの判定
    bool isGround()
    {
        int layerMask = 1 << 6;
        //オブジェクトのレイヤーに該当するLayerをつけ忘れないように
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.8f, layerMask);
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

    void Range()
    {
        //現在のポジションを保持する
        Vector3 currentPos = transform.position;

        //Mathf.ClampでX,Yの値それぞれが最小～最大の範囲内に収める。
        //物理挙動のあるisTriggerにしたいが、床は突き抜けてほしくないので無理やり範囲を決めて落ちないようにする
        currentPos.y = Mathf.Clamp(currentPos.y, -3.6f, 6);

        //positionをcurrentPosにする
        transform.position = currentPos;

    }

    void Damage()
    {
        if (isHit)
        {
            //プレイヤーの色を点滅させて無敵時間だと分かりやすくする
            float level = Mathf.Abs(Mathf.Sin(Time.time * 10));
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, level);

            //毎フレーム呼び出させないため
            hitTime += 1;


            if (hitTime == 1)
            {
                Invoke("WaitFor", 1.5f);
            }
        }
    }

    //無敵時間
    IEnumerator WaitForIt()
    {
        // 3�b�ԏ������~�߂�
        yield return new WaitForSeconds(1.5f);

        //�P�b��_���[�W�t���O��false�ɂ��ē_�ł�߂�
        isHit = false;

        //プレイヤーの色を元に戻す
        playerRenderer.color = new Color(1f, 1f, 1f, 1f);

    }

    void WaitFor()
    {
        PlayerHPImage playerHP;
        GameObject obj = GameObject.Find("PlayerHPTop");
        playerHP = obj.GetComponent<PlayerHPImage>();

        isHit = false;
        //該当スクリプト内でするとisDamageがfalseになるより速くまたisHitになってしまうため
        playerHP.isDamage = false;
        //プレイヤーの色を元に戻す
        playerRenderer.color = new Color(1f, 1f, 1f, 1f);
        hitTime = 0;
    }

    void Dead()
    {
        if (life <= 0)
        {
            isDead = true;
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Boss")
        {
            if (!isHit)
            {
                life -= 1;
                //一定時間無敵になる
                isHit = true;
                //音
                audioSource.PlayOneShot(damageAudio);
            }
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        //何かに当たったら消す
        if (collision.gameObject.tag == "Bullet" || collision.gameObject.tag == "Counter")
        {
            if (!isHit)
            {
                //音
                audioSource.PlayOneShot(damageAudio);
                //一定時間無敵になる
                StartCoroutine("WaitForIt");
                life -= 1;
                isHit = true;
            }
        }
    }
}
