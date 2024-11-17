using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BossAttackAction : MonoBehaviour
{
    public GameObject Bullet;

    Vector2 defaultPos = new Vector2(-6.5f, 3f);

    float moveSpeed;

    public float MoveSpeed = 5;//横移動の速さ

    bool isFloorHit;
    public bool isFloor;//別スクリプトで使用するノックバックできるかを決める変数
    bool isFinish;//攻撃が終わったか
    struct UpDown
    {
        public float TrackTime;//UpDownするまでの追尾の時間

        public int WaitTime;//次の攻撃までの待機時間
        public int TrackWaitTime;//追尾後の上での待機時間

        public bool isTrackWait;
        public bool isUp;
        public bool isWait;
    }
    UpDown upDown;
    int upDownCount;
    int upDownCountMax = 3;


    struct Side
    {
        public float LeftTime;//横突進するまでの追尾の時間
        public float RightTime;//横突進するまでの追尾の時間

        public int TrackWaitTime;//追尾後の上での待機時間

        public bool isTrackWait;
        public bool isRight;
        public bool isLeft;
        public bool isWait;
    }

    Side side;
    int sideCount;
    int sideCountMax = 3;

    struct Move
    {
        public int time;
        public bool isRight;
        public bool isLeft;

    }
    Move move;

    Vector2 rightPos = new Vector2(-4, 0);
    Vector2 leftPos = new Vector2(4, 0);

    //TrackBullet
    struct TrackBullet
    {
        public int IntervalTime;

    }
    TrackBullet trackBullet;

    int trackBulletCount;
    int trackBulletCountMax = 3;

    ActionMode nowMode;

    enum ActionMode
    {
        Moving,
        UpDown,
        SideTackle,
        TrackBullet,
    }

    enum AttackPattern
    {
        one,
        two,
        three,
        max
    }
    AttackPattern pattern;

    int patternCount;//次のパターンに移動するときに使う

    // Start is called before the first frame update
    void Start()
    {
        pattern = AttackPattern.one;
    }

    // Update is called once per frame
    void Update()
    {
        AttackManager();
        Attack();
    }
    void AttackManager()
    {
        if (patternCount == 3)
        {
            int random = Random.Range(0, (int)AttackPattern.max);
            pattern = (AttackPattern)random;
        }
        switch (pattern)
        {
            case AttackPattern.one:

                if (patternCount == 0)
                {
                    nowMode = ActionMode.SideTackle;
                }
                if (patternCount == 1)
                {
                    nowMode = ActionMode.UpDown;
                }
                if (patternCount == 2)
                {
                    nowMode = ActionMode.TrackBullet;
                }
                break;
        }
    }


    void Attack()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        switch (nowMode)
        {
            case ActionMode.Moving: // 移動中

                move.time += 1;
                if (move.time <= 200)
                {

                    //右にいるとき
                    if (transform.position.x >= rightPos.x)
                    {
                        move.isRight = true;
                        move.isLeft = false;
                    }
                    if (move.isRight)
                    {
                        transform.position = Vector2.MoveTowards(transform.position, new Vector2(rightPos.x, transform.position.y), moveSpeed * Time.deltaTime);
                        // move.isRight = false;
                    }
                    //左にいるとき
                    if (transform.position.x >= leftPos.x)
                    {
                        move.isRight = false;
                        move.isLeft = true;
                    }
                    if (move.isLeft)
                    {
                        transform.position = Vector2.MoveTowards(transform.position, new Vector2(leftPos.x, transform.position.y), moveSpeed * Time.deltaTime);
                        // move.isRight = true;
                    }
                }
                if (move.time >= 201)
                {
                    move = default;
                    patternCount += 1;
                }
                break;

            case ActionMode.UpDown: // プレイヤーを一定時間追尾後プレス攻撃


                upDown.TrackTime += 1;
                if (upDown.TrackTime <= 200 && !upDown.isTrackWait)
                {

                    //プレイヤーを一定時間追尾
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(player.transform.position.x, transform.position.y, transform.position.z), moveSpeed + 5 * Time.deltaTime);
                }
                else
                {
                    upDown.isTrackWait = true;
                }
                //それが終ったら上で待機
                if (upDown.isTrackWait && !upDown.isWait)
                {
                    upDown.TrackWaitTime += 1;
                    if (upDown.TrackWaitTime >= 100)
                    {
                        //待機時間を過ぎたら下に下がる
                        transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, -4, transform.position.z), moveSpeed + 5 * Time.deltaTime);
                    }
                }
                //床に当たったら
                if (isFloorHit)
                {
                    upDown.isWait = true;
                }
                //隙時間
                if (upDown.isWait)
                {
                    upDown.WaitTime += 1;
                    //隙時間が終ったら上に戻す
                    if (upDown.WaitTime >= 100)
                    {
                        upDown.isUp = true;
                        isFloorHit = false;

                    }
                }

                if (upDown.isUp)
                {
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, defaultPos.y, transform.position.z), moveSpeed + 5 * Time.deltaTime);
                    //定位置まで戻ったら
                    if (transform.position.y >= defaultPos.y)
                    {
                        upDownCount += 1;
                        //初期化して構造体の中の値を0に戻す
                        upDown = default;
                    }
                }


                if (upDownCount == upDownCountMax)
                {
                    patternCount += 1;
                }

                break;

            case ActionMode.SideTackle:

                side.LeftTime += 1;
                //追尾
                if (side.LeftTime <= 200)
                {
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(defaultPos.x, player.transform.position.y + 1, transform.position.z), moveSpeed + 5 * Time.deltaTime);
                    side.isLeft = true;
                }

                //突進
                if (side.LeftTime >= 201 && side.isLeft)
                {
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(-defaultPos.x, transform.position.y, transform.position.z), moveSpeed + 5 * Time.deltaTime);
                }
                //右端にきたら
                if (transform.position.x >= -defaultPos.x)
                {
                    side.isRight = true;
                    side.isLeft = false;
                }
                if (side.isRight)
                {
                    side.RightTime += 1;
                }
                if (side.RightTime <= 200 && side.isRight)
                {
                    //追尾
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(-defaultPos.x, player.transform.position.y + 2, transform.position.z), moveSpeed + 5 * Time.deltaTime);
                }
                if (side.RightTime >= 201 && side.isRight)
                {
                    //左に突進
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(defaultPos.x, transform.position.y, transform.position.z), moveSpeed + 5 * Time.deltaTime);
                }
                if (transform.position.x <= defaultPos.x && side.isRight)
                {
                    side = default;
                    sideCount += 1;
                }
                if (sideCount == sideCountMax)
                {
                    patternCount += 1;
                }
                break;

            case ActionMode.TrackBullet: //放物線を描いた追尾の弾を出す

                trackBullet.IntervalTime += 1;
                if (trackBullet.IntervalTime >= 200)
                {
                    Instantiate(Bullet, transform.position, Quaternion.identity);
                    trackBullet.IntervalTime = 0;
                    trackBulletCount += 1;
                }
                if (trackBulletCount == trackBulletCountMax)
                {
                    patternCount += 1;
                }
                break;
        }
    }



    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            if (!isFloorHit)
            {
                isFloorHit = true;
            }
        }
    }

}
