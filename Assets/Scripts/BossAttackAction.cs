using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackAction : MonoBehaviour
{
    Vector2 defaultPos = new Vector2(0, 3f);
    float moveSpeed;

    public float MoveSpeed = 5;//横移動の速さ

    bool isFloorHit;
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

    struct Move
    {
        public int time;
        public bool isRight;
        public bool isLeft;

    }
    Move move;

    Vector2 rightPos = new Vector2(-4, 0);
    Vector2 leftPos = new Vector2(4, 0);

    ActionMode nowMode;

    enum ActionMode
    {
        Moving,
        UpDown,
        TrackBullet,
    }


    // Start is called before the first frame update
    void Start()
    {
        nowMode = ActionMode.UpDown;
    }

    // Update is called once per frame
    void Update()
    {
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
                if(move.time >= 201)
                {
                    move = default;
                    //次のシーン
                }
                break;

            case ActionMode.UpDown: // プレイヤーを一定時間追尾後プレス攻撃


                upDown.TrackTime += 1;
                if (upDown.TrackTime <= 200 && !upDown.isTrackWait)
                {
                    GameObject player = GameObject.FindGameObjectWithTag("Player");
                    //プレイヤーを一定時間追尾
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(player.transform.position.x,transform.position.y, transform.position.z), moveSpeed + 5 * Time.deltaTime);
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
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, 3, transform.position.z), moveSpeed + 5 * Time.deltaTime);
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
                    //別の動きに変える
                }

                break;

            case ActionMode.TrackBullet: //緩い追尾の弾を出す


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
