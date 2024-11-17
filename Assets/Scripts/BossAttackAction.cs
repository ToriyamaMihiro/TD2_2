using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BossAttackAction : MonoBehaviour
{
    public GameObject Bullet;

    Vector2 defaultPos = new Vector2(-6.5f, 3f);

    float moveSpeed;

    public float MoveSpeed = 5;//���ړ��̑���

    bool isFloorHit;
    public bool isFloor;//�ʃX�N���v�g�Ŏg�p����m�b�N�o�b�N�ł��邩�����߂�ϐ�
    bool isFinish;//�U�����I�������
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


    struct Side
    {
        public float LeftTime;//���ːi����܂ł̒ǔ��̎���
        public float RightTime;//���ːi����܂ł̒ǔ��̎���

        public int TrackWaitTime;//�ǔ���̏�ł̑ҋ@����

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

    int patternCount;//���̃p�^�[���Ɉړ�����Ƃ��Ɏg��

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
                if (move.time >= 201)
                {
                    move = default;
                    patternCount += 1;
                }
                break;

            case ActionMode.UpDown: // �v���C���[����莞�Ԓǔ���v���X�U��


                upDown.TrackTime += 1;
                if (upDown.TrackTime <= 200 && !upDown.isTrackWait)
                {

                    //�v���C���[����莞�Ԓǔ�
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(player.transform.position.x, transform.position.y, transform.position.z), moveSpeed + 5 * Time.deltaTime);
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
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, defaultPos.y, transform.position.z), moveSpeed + 5 * Time.deltaTime);
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
                    patternCount += 1;
                }

                break;

            case ActionMode.SideTackle:

                side.LeftTime += 1;
                //�ǔ�
                if (side.LeftTime <= 200)
                {
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(defaultPos.x, player.transform.position.y + 1, transform.position.z), moveSpeed + 5 * Time.deltaTime);
                    side.isLeft = true;
                }

                //�ːi
                if (side.LeftTime >= 201 && side.isLeft)
                {
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(-defaultPos.x, transform.position.y, transform.position.z), moveSpeed + 5 * Time.deltaTime);
                }
                //�E�[�ɂ�����
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
                    //�ǔ�
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(-defaultPos.x, player.transform.position.y + 2, transform.position.z), moveSpeed + 5 * Time.deltaTime);
                }
                if (side.RightTime >= 201 && side.isRight)
                {
                    //���ɓːi
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

            case ActionMode.TrackBullet: //��������`�����ǔ��̒e���o��

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
