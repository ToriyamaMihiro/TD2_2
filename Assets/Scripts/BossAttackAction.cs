using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BossAttackAction : MonoBehaviour
{
    public GameObject Bullet;

    Vector2 defaultPos = new Vector2(-7.5f, 3.1f);
    Vector2 objectScale;

    float moveSpeed;

    public float MoveSpeed = 5;//横移動の速さ
    int wallTime;

    bool isFloorHit;
    public bool isFloor;//別スクリプトで使用するノックバックできるかを決める変数
    bool isFinish;//攻撃が終わったか
    bool isWall;
    public bool isDeformationFinish;

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
    int sideCountMax = 2;

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
       public bool isWait;

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

    int patternCount=2;//次のパターンに移動するときに使う

    // Start is called before the first frame update
    void Start()
    {
        nowMode = ActionMode.TrackBullet;
    }

    // Update is called once per frame
    void Update()
    {
        Deformation();
        AttackManager();
        Attack();
        WallStan();
        objectScale = transform.localScale;
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
                    isFinish = false;
                }
                if (patternCount == 1)
                {
                    nowMode = ActionMode.UpDown;
                    isFinish = false;
                }
                if (patternCount == 2)
                {
                    nowMode = ActionMode.TrackBullet;
                    isFinish = false;
                }
                break;
        }
    }

    //ボスの攻撃中でなければ元に戻す
    void Deformation()
    {
        BossAction boss = GetComponent<BossAction>();
        if (boss.deformationTime <= 1 && isFinish)
        {
            isDeformationFinish = false;
        }
        if (boss.deformationTime >= 200 && isFinish)
        {
            transform.localScale = new Vector3(2, 2, 1);
            isDeformationFinish = true;
            isFinish = false;
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
                    isFinish = true;
                }
                break;

            case ActionMode.UpDown: // プレイヤーを一定時間追尾後プレス攻撃


                upDown.TrackTime += 1;
                if (upDown.TrackTime <= 200 && !upDown.isTrackWait)
                {
                    //プレイヤーを一定時間追尾
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(player.transform.position.x, defaultPos.y - objectScale.y / 2, transform.position.z), moveSpeed + 5 * Time.deltaTime);
                }
                else
                {
                    upDown.isTrackWait = true;
                }
                //それが終ったら上で待機
                if (upDown.isTrackWait && !upDown.isWait)
                {
                    upDown.TrackWaitTime += 1;
                    if (upDown.TrackWaitTime >= 100&&!isFloorHit)
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
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, defaultPos.y - objectScale.y / 2, transform.position.z), moveSpeed + 5 * Time.deltaTime);
                    //定位置まで戻ったら
                    if (transform.position.y >= defaultPos.y - objectScale.y / 2)
                    {
                        upDownCount += 1;
                        //初期化して構造体の中の値を0に戻す
                        upDown = default;
                    }
                }


                if (upDownCount == upDownCountMax)
                {
                    patternCount += 1;
                    isFinish = true;
                }

                break;

            case ActionMode.SideTackle:

                side.LeftTime += 1;
                Vector2 sidePos = new Vector2(8.5f, 0);
                //追尾
                if (side.LeftTime <= 220 && !isWall)
                {
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(defaultPos.x + objectScale.x / 2, player.transform.position.y + objectScale.y / 2, transform.position.z), moveSpeed + 5 * Time.deltaTime);
                    side.isLeft = true;
                }

                //壁に当たるまで突進
                if (side.LeftTime >= 221 && side.isLeft)
                {
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(sidePos.x - objectScale.x / 2, transform.position.y, transform.position.z), moveSpeed + 5 * Time.deltaTime);
                }
                //右端にきて壁に当たってスタンが終ったら
                if (transform.position.x >= sidePos.x - objectScale.x / 2)
                {
                    side.isRight = true;
                    side.isLeft = false;
                }
                if (side.isRight && !isWall)
                {
                    side.RightTime += 1;
                }
                WallStan();
                if (side.RightTime <= 220 && side.isRight && !isWall)
                {
                    //追尾
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(-defaultPos.x - objectScale.x / 2, player.transform.position.y + objectScale.y / 2, transform.position.z), moveSpeed + 5 * Time.deltaTime);
                }
                if (side.RightTime >= 221 && side.isRight && !isWall)
                {
                    //左に突進
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(-sidePos.x + objectScale.x / 2, transform.position.y, transform.position.z), moveSpeed + 5 * Time.deltaTime);
                }
                WallStan();
                if (transform.position.x <= -sidePos.x + objectScale.x / 2 && side.isRight)
                {
                    side = default;
                    sideCount += 1;
                }
                if (sideCount == sideCountMax)
                {
                    patternCount += 1;
                    isFinish = true;
                }
                break;

            case ActionMode.TrackBullet: //放物線を描いた追尾の弾を出す

                trackBullet.IntervalTime += 1;
                if (!trackBullet.isWait)
                {
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, -4, transform.position.z), moveSpeed + 5 * Time.deltaTime);

                }
                if (isFloorHit)
                {
                    trackBullet.isWait = true;
                }
                if (trackBullet.IntervalTime >= 200)
                {
                    Instantiate(Bullet, transform.position, Quaternion.identity);
                    trackBullet.IntervalTime = 0;
                    trackBulletCount += 1;
                }
                if (trackBulletCount == trackBulletCountMax)
                {
                    patternCount += 1;
                    isFinish = true;
                    isFloorHit = false;
                }
                break;
        }
    }

    void WallStan()
    {
        if (isWall)
        {
            wallTime += 1;
            if (wallTime >= 100)
            {
                isWall = false;
                wallTime = 0;
            }
        }
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            if (!isFloorHit)
            {
                isFloorHit = true;
            }
        }
        if (collision.gameObject.tag == "Wall")
        {
            if (!isWall)
            {
                isWall = true;
            }
        }
    }

}
