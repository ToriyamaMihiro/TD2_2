using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackAction : MonoBehaviour
{
    Vector2 defaultPos = new Vector2(0, 3f);
    float moveSpeed;

    public float MoveSpeed = 5;//���ړ��̑���

    bool isFloorHit;
    struct UpDown
    {
        public float TrackTime;//UpDown����܂ł̒ǔ��̎���

        public int WaitTime;//���̍U���܂ł̑ҋ@����
        public int TrackWaitTime;//�ǔ���̏�ł̑ҋ@����

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
            case ActionMode.Moving: // �ړ���

                move.time += 1;
                if (move.time <= 200)
                {

                    //�E�ɂ���Ƃ�
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
                    //���ɂ���Ƃ�
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
                    //���̃V�[��
                }
                break;

            case ActionMode.UpDown: // �v���C���[����莞�Ԓǔ���v���X�U��


                upDown.TrackTime += 1;
                if (upDown.TrackTime <= 200 && !upDown.isTrackWait)
                {
                    GameObject player = GameObject.FindGameObjectWithTag("Player");
                    //�v���C���[����莞�Ԓǔ�
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(player.transform.position.x,transform.position.y, transform.position.z), moveSpeed + 5 * Time.deltaTime);
                }
                else
                {
                    upDown.isTrackWait = true;
                }
                //���ꂪ�I�������őҋ@
                if (upDown.isTrackWait && !upDown.isWait)
                {
                    upDown.TrackWaitTime += 1;
                    if (upDown.TrackWaitTime >= 100)
                    {
                        //�ҋ@���Ԃ��߂����牺�ɉ�����
                        transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, -4, transform.position.z), moveSpeed + 5 * Time.deltaTime);
                    }
                }
                //���ɓ���������
                if (isFloorHit)
                {
                    upDown.isWait = true;
                }
                //������
                if (upDown.isWait)
                {
                    upDown.WaitTime += 1;
                    //�����Ԃ��I�������ɖ߂�
                    if (upDown.WaitTime >= 100)
                    {
                        upDown.isUp = true;
                        isFloorHit = false;

                    }
                }

                if (upDown.isUp)
                {
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, 3, transform.position.z), moveSpeed + 5 * Time.deltaTime);
                    //��ʒu�܂Ŗ߂�����
                    if (transform.position.y >= defaultPos.y)
                    {
                        upDownCount += 1;
                        //���������č\���̂̒��̒l��0�ɖ߂�
                        upDown = default;
                    }
                }


                if (upDownCount == upDownCountMax)
                {
                    //�ʂ̓����ɕς���
                }

                break;

            case ActionMode.TrackBullet: //�ɂ��ǔ��̒e���o��


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
