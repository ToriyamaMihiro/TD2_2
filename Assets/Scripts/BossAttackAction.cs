using UnityEngine;
using UnityEngine.TestTools;

public class BossAttackAction : MonoBehaviour
{
    public GameObject Bullet;
    public GameObject RNeedle;
    public GameObject LNeedle;
    public GameObject CounterParticle;

    Vector2 defaultPos = new Vector2(-7.5f, 4.2f);
    Vector2 objectScale;

    public float MoveSpeed = 5;//横移動の速さ
    float XRange = 7.5f;
    float YRange = 3.5f;
    int wallTime;
    int randomValue;

    public int deformationTime = 200;//変形から元に戻るまでの時間

    bool isFloorHit;
    public bool isFloor;//別スクリプトで使用するノックバックできるかを決める変数
    bool isFinish;//攻撃が終わったか
    bool isWall;
    public bool isDeformationFinish;
    public bool isShake;
    bool isLifeLow;//ボスのHPが少なくなっているか

    float moveRange = 0.2f;

    Vector3 randomPosition;

    //ボスサイズ
    public Vector3 bossSize = new Vector3(3, 3, 1);
    //アニメ
    private Animator animator;
    bool isAttackAnime;
    //音
    private AudioSource audioSource;
    public AudioClip arartAudio;
    public AudioClip bulletAudio;
    public AudioClip wallCollideAudio;
    float arartTimer = 0;

    struct Move
    {
        public int rightTime;
        public int leftTime;
        public int waitTime;
        public bool isRight;
        public bool isLeft;
        public bool isWait;

    }
    Move move;

    Vector2 rightPos = new Vector2(-6, 0);
    Vector2 leftPos = new Vector2(6, 0);
    int moveMaxCount = 1;
    int moveCount;
    int moveTime = 200;
    int moveWaitTime = 100;

    struct UpDown
    {
        public float TrackTime;//UpDownするまでの追尾の時間

        public int WaitTime;//次の攻撃までの待機時間
        public int TrackWaitTime;//追尾後の上での待機時間

        public bool isTrackWait;
        public bool isUp;
        public bool isWait;
        public bool isDeformation;
    }
    UpDown upDown;
    int upDownCount;
    int upDownCountMax = 2;
    int upDownMoveTime = 150;
    int upWaitTime = 100;
    int downWaitTime = 200;
    float downSpeed = 9;
    public bool isNeedleSpawn;


    struct Side
    {
        public float LeftTime;//横突進するまでの追尾の時間
        public float RightTime;//横突進するまでの追尾の時間

        public int TrackWaitTime;//追尾後の上での待機時間


        public bool isTrackWait;
        public bool isRight;
        public bool isLeft;
        public bool isWait;
        public bool isStartPos;//最初の位置にきたか
    }

    Side side;
    int sideCount;
    int sideCountMax = 2;
    int sideWaitTime = 150;
    int sideAttackWaitTime = 220;
    int sideMoveSpeed = 8;


    //TrackBullet
    struct TrackBullet
    {
        public int IntervalTime;
        public bool isWait;

    }
    TrackBullet trackBullet;

    int trackBulletTime = 220;
    int trackBulletCount;
    int trackBulletCountMax = 3;

    struct CounterAttack
    {
        public int FrontTime;//カウンター前の溜め
        public int Time;//カウンターをしている時間
    }
    CounterAttack counter;

    public bool isCounterFront;
    public bool isCounter;
    public int counterMaxTime = 200;
    int counterFrontMaxTime = 100;


    ActionMode nowMode;

    int needleRandom;
    float needlePosInterval = 2;
    public bool isNeedle;//すでにでているか


    enum ActionMode
    {
        Moving,
        UpDown,
        SideTackle,
        TrackBullet,
    }
    ActionMode actionMode;

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

        //randomValue = Random.Range(0, 101);
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        BossAction boss = GetComponent<BossAction>();
        //アニメ
        animator.SetBool("isAttack", isAttackAnime);//アニメ変更
       
        if (!boss.isDead)
        {

            Deformation();
            AttackManager();
            Attack();
            WallStan();
            Range();
            AttackChange();
            objectScale = transform.localScale;
        }
        else
        {
            isAttackAnime = false;
        }
    }

    //ノックバックで外に行かないようにする
    void Range()
    {
        //現在のポジションを保持する
        Vector3 currentPos = transform.position;

        if (nowMode != ActionMode.UpDown)
        {
            BossAction boss = GetComponent<BossAction>();
            //大きさによって範囲の決定
            if (boss.isXDeformation)
            {
                XRange = 8.2f;
            }
            else if (boss.isYDeformation)
            {
                YRange = 3.5f;
            }
            else
            {
                XRange = 7.5f;
                YRange = 3.5f;
            }
        }
        if (nowMode == ActionMode.UpDown)
        {
            //UpDownのときも変形したら範囲を変える
            BossAction boss2 = GetComponent<BossAction>();
            if (boss2.isXDeformation)
            {
                // XRange = 8.1f;
            }
            if (boss2.isYDeformation)
            {
                YRange = 3.5f;
            }
        }
        //Mathf.ClampでX,Yの値それぞれが最小～最大の範囲内に収める。
        //物理挙動のあるisTriggerにしたいが、床は突き抜けてほしくないので無理やり範囲を決めて落ちないようにする
        currentPos.x = Mathf.Clamp(currentPos.x, -XRange, XRange);
        currentPos.y = Mathf.Clamp(currentPos.y, -YRange, YRange);


        //positionをcurrentPosにする
        transform.position = currentPos;

    }

    void AttackRandom()
    {
        BossAction boss = GetComponent<BossAction>();

        //縦に変形
        if (boss.isXDeformation)
        {
            if (randomValue < 61)
            {
                pattern = AttackPattern.two;
            }
            else if (randomValue < 81)
            {
                pattern = AttackPattern.one;
            }
            else
            {
                pattern = AttackPattern.three;
            }
        }
        //横に変形していく
        else if (boss.isYDeformation)
        {
            if (randomValue < 61)
            {
                pattern = AttackPattern.three;
            }
            else if (randomValue < 81)
            {
                pattern = AttackPattern.two;
            }
            else
            {
                pattern = AttackPattern.one;
            }
        }
        //変形してないとき
        else
        {
            if (randomValue < 35)
            {
                pattern = AttackPattern.one;
            }
            else if (randomValue < 70)
            {
                pattern = AttackPattern.two;
            }
            else
            {
                pattern = AttackPattern.three;
            }
        }


    }

    void AttackManager()
    {
        if (patternCount == 4)
        {
            randomValue = Random.Range(0, 101);
            AttackRandom();
            patternCount = 0;
        }
        switch (pattern)
        {
            case AttackPattern.one:

                if (patternCount == 0)
                {
                    nowMode = ActionMode.UpDown;
                    isFinish = false;
                }
                if (patternCount == 1)
                {
                    nowMode = ActionMode.SideTackle;
                    isFinish = false;
                }
                if (patternCount == 2)
                {
                    nowMode = ActionMode.TrackBullet;
                    isFinish = false;
                }
                if (patternCount == 3)
                {
                    nowMode = ActionMode.Moving;
                    isFinish = false;
                }

                break;

            case AttackPattern.two:

                if (patternCount == 0)
                {
                    nowMode = ActionMode.TrackBullet;
                    isFinish = false;
                }
                if (patternCount == 1)
                {
                    nowMode = ActionMode.UpDown;
                    isFinish = false;
                }
                if (patternCount == 2)
                {
                    nowMode = ActionMode.SideTackle;
                    isFinish = false;
                }
                if (patternCount == 3)
                {
                    nowMode = ActionMode.UpDown;
                    isFinish = false;
                }
                break;

            case AttackPattern.three:

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
                    nowMode = ActionMode.SideTackle;
                    isFinish = false;
                }
                if (patternCount == 3)
                {
                    nowMode = ActionMode.Moving;
                    isFinish = false;
                }
                break;
        }
    }

    //ボスの攻撃中でなければ元に戻す
    void Deformation()
    {
        BossAction boss = GetComponent<BossAction>();
        if (boss.deformationTime <= 1 && !isFinish)
        {
            isDeformationFinish = false;
        }
        if (boss.deformationTime >= deformationTime && isFinish)
        {
            transform.localScale = bossSize;
            isDeformationFinish = true;
            isFinish = false;
        }
    }

    void Attack()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        BossAction boss = GetComponent<BossAction>();
        

        //アラート音
        if (isAttackAnime && arartTimer == 0)
        {
            arartTimer++;
            //音

            audioSource.PlayOneShot(arartAudio, 0.8f);
        }
        else if (!isAttackAnime)
        {
            arartTimer = 0;
        }

        switch (nowMode)
        {
            case ActionMode.Moving: // 移動中

                move.rightTime += 1;
                if (move.rightTime <= moveTime)
                {
                    //右に行く
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(rightPos.x, defaultPos.y - objectScale.y / 2, transform.position.z), MoveSpeed * Time.deltaTime);
                }
                else if (move.rightTime >= moveTime + 1 && !move.isRight && !move.isLeft)
                {
                    move.isRight = true;
                    move.isWait = true;
                }
                //待ち時間になったら一旦止まる
                if (move.isWait && move.isRight)
                {
                    move.waitTime += 1;
                    if (move.waitTime >= moveWaitTime)
                    {
                        move.isWait = false;
                        move.isRight = false;
                        move.isLeft = true;
                        move.waitTime = 0;
                    }
                }
                if (move.isLeft)
                {
                    move.leftTime += 1;
                }
                //左に移動
                if (move.isLeft && !move.isWait && move.leftTime < moveTime)
                {
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(leftPos.x, transform.position.y, transform.position.z), MoveSpeed * Time.deltaTime);
                }
                else if (move.leftTime > moveTime + 1)
                {
                    move.isWait = true;
                    if (move.isWait && move.isLeft)
                    {
                        move.waitTime += 1;
                        if (move.waitTime >= moveWaitTime)
                        {
                            moveCount += 1;
                            move = default;
                        }
                    }
                }
                if (moveCount == moveMaxCount)
                {
                    patternCount += 1;
                    //move = default;
                }
                break;

            case ActionMode.UpDown: // プレイヤーを一定時間追尾後プレス攻撃

                upDown.TrackTime += 1;
                //上まで着いたら追尾の幅を限定する
                if (transform.position.y >= defaultPos.y - objectScale.y / 2)
                {
                    XRange = 5.2f;
                }

                if (upDown.TrackTime <= upDownMoveTime && !upDown.isTrackWait)
                {
                    //プレイヤーを一定時間追尾
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(player.transform.position.x, defaultPos.y - objectScale.y / 2, transform.position.z), MoveSpeed * Time.deltaTime);
                }

                else
                {
                    upDown.isTrackWait = true;
                }
                //それが終ったら上で待機
                if (upDown.isTrackWait && !upDown.isWait)
                {
                    upDown.TrackWaitTime += 1;
                    if (upDown.TrackWaitTime >= upWaitTime / 4 && !isFloorHit && !upDown.isUp)
                    {
                        isAttackAnime = true;//ボスアニメ変更
                    }
                    if (upDown.TrackWaitTime >= upWaitTime && !isFloorHit && !upDown.isUp)
                    {
                        //待機時間を過ぎたら下に下がる
                        transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, -4, transform.position.z), downSpeed * Time.deltaTime);

                        isAttackAnime = false;//ボスアニメ変更
                    }
                }
                //床に当たったら
                if (isFloorHit)
                {
                    upDown.isWait = true;
                    XRange = 7.5f;
                }
                //隙時間
                if (upDown.isWait)
                {
                    upDown.WaitTime += 1;
                    if (upDown.WaitTime == 2 && upDownCount == 0)
                    {
                        //シェイクさせる
                        isShake = true;
                        if (!isNeedleSpawn)
                        {
                            //針をだす
                            NeedleSpawn();
                            isNeedleSpawn = true;
                        }
                    }
                    //隙時間が終ったら上に戻す
                    if (upDown.WaitTime >= downWaitTime)
                    {
                        Counter();
                        if (isCounter)
                        {
                            counter.Time += 1;
                            if (counter.Time >= counterMaxTime)
                            {
                                upDown.isUp = true;
                                isFloorHit = false;
                                upDown.isWait = false;
                                counter = default;
                                isCounter = false;
                            }
                        }

                    }
                    //もしこの間に変形したらそれに合わせる
                    if (boss.isXDeformation)
                    {
                        transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, -4 + objectScale.y / 2, transform.position.z), MoveSpeed + 5 * Time.deltaTime);
                    }
                    if (boss.isYDeformation)
                    {
                        transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, -4f + objectScale.y / 2, transform.position.z), MoveSpeed + 5 * Time.deltaTime);
                    }
                }

                if (upDown.isUp)
                {
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, defaultPos.y - objectScale.y / 2, transform.position.z), MoveSpeed * Time.deltaTime);

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
                    upDownCount = 0;
                    isFinish = true;
                }

                break;

            case ActionMode.SideTackle:

                Vector2 sidePos = new Vector2(9f, 0);
                //isAttackAnime = false;
                //追尾
                if (transform.position.x <= defaultPos.x + objectScale.x / 2)
                {
                    side.isStartPos = true;
                }
                //スタートの位置についてから時間を測る
                if (!isWall && side.isStartPos)
                {
                    side.LeftTime += 1;
                }
                if (side.LeftTime <= sideAttackWaitTime && !isWall)
                {
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(defaultPos.x + objectScale.x / 2, player.transform.position.y + objectScale.y / 2, transform.position.z), MoveSpeed * Time.deltaTime);
                    side.isLeft = true;
                }
                if (side.LeftTime >= sideAttackWaitTime - sideAttackWaitTime / 2.5f && !isWall && !side.isRight)
                {
                    isAttackAnime = true;//ボスアニメ変更

                }

                //壁に当たるまで突進
                if (side.LeftTime >= sideAttackWaitTime + 1 && side.isLeft)
                {
                    isAttackAnime = false;//ボスアニメ変更
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(sidePos.x - objectScale.x / 2, transform.position.y, transform.position.z), sideMoveSpeed * Time.deltaTime);
                }
                //右端にきて壁に当たってスタンが終ったら
                if (transform.position.x >= sidePos.x - objectScale.x / 2)
                {
                    side.isRight = true;
                    side.isLeft = false;
                }

                WallStan();

                if (side.isRight && !isWall)
                {
                    side.RightTime += 1;
                }
                if (side.RightTime <= sideAttackWaitTime && side.isRight && !isWall)
                {

                    //追尾
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(-defaultPos.x - objectScale.x / 2, player.transform.position.y + objectScale.y / 2, transform.position.z), MoveSpeed * Time.deltaTime);
                }
                if (side.RightTime >= sideAttackWaitTime - sideAttackWaitTime / 2.5f && side.isRight && !isWall)
                {
                    isAttackAnime = true;//ボスアニメ変更

                }
                if (side.RightTime >= sideAttackWaitTime + 1 && side.isRight && !isWall)
                {
                    isAttackAnime = false;//ボスアニメ変更
                    //左に突進
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(-sidePos.x + objectScale.x / 2, transform.position.y, transform.position.z), sideMoveSpeed * Time.deltaTime);

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
                    sideCount = 0;
                    isFinish = true;
                }
                break;

            case ActionMode.TrackBullet: //放物線を描いた追尾の弾を出す

                trackBullet.IntervalTime += 1;
                isAttackAnime = false;
                if (!trackBullet.isWait)
                {
                    //地面に当たるまで下に行く
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, -4, transform.position.z), MoveSpeed * Time.deltaTime);

                }
                if (isFloorHit)
                {
                    trackBullet.isWait = true;


                    //もしこの間に変形したらそれに合わせる
                    if (boss.isXDeformation)
                    {
                        transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, -4 + objectScale.y / 2, transform.position.z), MoveSpeed + 5 * Time.deltaTime);
                    }
                    if (boss.isYDeformation)
                    {
                        transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, -4 + objectScale.y / 2, transform.position.z), MoveSpeed + 5 * Time.deltaTime);
                    }
                }

                if (trackBullet.IntervalTime >= 200 - (200 / 2.5f) && trackBulletCount < trackBulletCountMax)
                {

                    isAttackAnime = true;//ボスアニメ変更
                }

                //時間になったら弾を出す
                if (trackBullet.IntervalTime >= 200 && trackBulletCount < trackBulletCountMax)
                {
                    Instantiate(Bullet, transform.position, Quaternion.identity);
                    trackBullet.IntervalTime = 0;
                    trackBulletCount += 1;
                    isAttackAnime = false;//ボスアニメ変更
                    //音
                    audioSource.PlayOneShot(bulletAudio);
                }

                //弾を吐くと同時に動くのを防ぐため
                if (trackBullet.IntervalTime >= 200 && trackBulletCount == trackBulletCountMax)
                {
                    trackBulletCount += 1;
                }
                if (trackBulletCount == trackBulletCountMax + 1)
                {
                    patternCount += 1;
                    isFinish = true;
                    isFloorHit = false;
                    trackBulletCount = 0;
                    trackBullet = default;
                }
                break;
        }
    }

    //ボスのHPに応じてTimeを変える
    void AttackChange()
    {
        BossAction boss = GetComponent<BossAction>();
        //HPが4割になったら
        if (boss.currentHp <= boss.life / 3 && !isLifeLow)
        {
            downWaitTime -= 100;
            trackBulletTime -= 120;
            moveWaitTime -= 60;
            sideAttackWaitTime -= 50;
            isLifeLow = true;
        }
    }

    void NeedleSpawn()
    {
        Vector2 needlePos = new Vector2(9.4f, 2);
        //左側
        for (int i = 0; i < 4; i++)
        {
            Instantiate(LNeedle, new Vector2(-needlePos.x, -needlePos.y + needlePosInterval * i), Quaternion.identity);
        }
        //右側
        for (int i = 0; i < 4; i++)
        {
            Instantiate(RNeedle, new Vector2(needlePos.x, -needlePos.y + needlePosInterval * i), Quaternion.identity);
        }
    }

    void Counter()
    {
        counter.FrontTime += 1;
        if (counter.FrontTime == 1)
        {
            for (int i = 0; i < 20; i++)
            {
                randomPosition = new Vector2(Random.Range(-moveRange, moveRange),
              Random.Range(-moveRange, moveRange));
                Instantiate(CounterParticle, transform.position + randomPosition, Quaternion.identity);
            }
        }
        //カウンター前か
        if (counter.FrontTime <= counterFrontMaxTime)
        {
            isCounterFront = true;
        }
        //タメが終わったら攻撃
        if (counter.FrontTime >= counterFrontMaxTime + 1)
        {
            isCounterFront = false;
            isCounter = true;
        }
    }

    void WallStan()
    {
        if (isWall)
        {
            wallTime += 1;
            if (wallTime == 2)
            {
                isShake = true;
            }
            //sideWaitTime以上になったら動き出す
            if (wallTime >= sideWaitTime)
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
            //音
            audioSource.PlayOneShot(wallCollideAudio);
            if (!isFloorHit)
            {
                isFloorHit = true;
            }
        }
        if (collision.gameObject.tag == "Wall")
        {
            //音
            audioSource.PlayOneShot(wallCollideAudio);
            if (!isWall)
            {
                isWall = true;
            }
        }
    }

}
