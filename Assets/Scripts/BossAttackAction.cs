using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BossAttackAction : MonoBehaviour
{
    public GameObject Bullet;

    Vector2 defaultPos = new Vector2(-7.5f, 3.1f);
    Vector2 objectScale;

    public float MoveSpeed = 5;//���ړ��̑���
    int wallTime;
    int randomValue;

    bool isFloorHit;
    public bool isFloor;//�ʃX�N���v�g�Ŏg�p����m�b�N�o�b�N�ł��邩�����߂�ϐ�
    bool isFinish;//�U�����I�������
    bool isWall;
    public bool isDeformationFinish;
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
    int upDownCountMax = 1;


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
    int sideCountMax = 1;


    //TrackBullet
    struct TrackBullet
    {
        public int IntervalTime;
        public bool isWait;

    }
    TrackBullet trackBullet;

    int trackBulletCount;
    int trackBulletCountMax = 1;

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
        nowMode = ActionMode.Moving;
        //randomValue = Random.Range(0, 101);
    }

    // Update is called once per frame
    void Update()
    {
        //AttackRandom();
        Deformation();
        AttackManager();
        Attack();
        WallStan();
        objectScale = transform.localScale;
    }

    void AttackRandom()
    {
        BossAction boss = GetComponent<BossAction>();

        //���ɕό`
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
        //�c�ɕό`���Ă���
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
        //�ό`���ĂȂ��Ƃ�
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
        if (patternCount == 3)
        {
            randomValue = Random.Range(0, 101);
            patternCount = 0;
        }
        switch (pattern)
        {
            case AttackPattern.one:

                if (patternCount == 0)
                {
                    nowMode = ActionMode.Moving;
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

            case AttackPattern.two:

                if (patternCount == 0)
                {
                    nowMode = ActionMode.UpDown;
                    isFinish = false;
                }
                if (patternCount == 1)
                {
                    nowMode = ActionMode.TrackBullet;
                    isFinish = false;
                }
                if (patternCount == 2)
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
                break;
        }
    }


    //�{�X�̍U�����łȂ���Ό��ɖ߂�
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
            case ActionMode.Moving: // �ړ���

                move.rightTime += 1;
                if (move.rightTime <= 200)
                {
                    //�E�ɍs��
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(rightPos.x, transform.position.y, transform.position.z), MoveSpeed * Time.deltaTime);
                }
                else if(move.rightTime >= 201&&!move.isRight&&!move.isLeft)
                {
                    move.isRight = true;
                    move.isWait = true;
                }
                //�҂����ԂɂȂ������U�~�܂�
                if (move.isWait && move.isRight)
                {
                    move.waitTime += 1;
                    if (move.waitTime >= 100)
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
                //���Ɉړ�
                if (move.isLeft && !move.isWait && move.leftTime < 100)
                {
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(leftPos.x, transform.position.y, transform.position.z), MoveSpeed * Time.deltaTime);
                }
                else if (move.leftTime > 101)
                {
                    move.isWait = true;
                    if (move.isWait && move.isLeft)
                    {
                        move.waitTime += 1;
                        if (move.waitTime >= 100)
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

            case ActionMode.UpDown: // �v���C���[����莞�Ԓǔ���v���X�U��


                upDown.TrackTime += 1;
                if (upDown.TrackTime <= 200 && !upDown.isTrackWait)
                {
                    //�v���C���[����莞�Ԓǔ�
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(player.transform.position.x, defaultPos.y - objectScale.y / 2, transform.position.z), MoveSpeed * Time.deltaTime);
                }
                else
                {
                    upDown.isTrackWait = true;
                }
                //���ꂪ�I�������őҋ@
                if (upDown.isTrackWait && !upDown.isWait)
                {
                    upDown.TrackWaitTime += 1;
                    if (upDown.TrackWaitTime >= 100 && !isFloorHit)
                    {
                        //�ҋ@���Ԃ��߂����牺�ɉ�����
                        transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, -4, transform.position.z), MoveSpeed * Time.deltaTime);
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
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, defaultPos.y - objectScale.y / 2, transform.position.z), MoveSpeed * Time.deltaTime);
                    //��ʒu�܂Ŗ߂�����
                    if (transform.position.y >= defaultPos.y - objectScale.y / 2)
                    {
                        upDownCount += 1;
                        //���������č\���̂̒��̒l��0�ɖ߂�
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

                side.LeftTime += 1;
                Vector2 sidePos = new Vector2(8.5f, 0);
                //�ǔ�
                if (side.LeftTime <= 220 && !isWall)
                {
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(defaultPos.x + objectScale.x / 2, player.transform.position.y + objectScale.y / 2, transform.position.z), MoveSpeed * Time.deltaTime);
                    side.isLeft = true;
                }

                //�ǂɓ�����܂œːi
                if (side.LeftTime >= 221 && side.isLeft)
                {
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(sidePos.x - objectScale.x / 2, transform.position.y, transform.position.z), MoveSpeed * Time.deltaTime);
                }
                //�E�[�ɂ��ĕǂɓ������ăX�^�����I������
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
                    //�ǔ�
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(-defaultPos.x - objectScale.x / 2, player.transform.position.y + objectScale.y / 2, transform.position.z), MoveSpeed * Time.deltaTime);
                }
                if (side.RightTime >= 221 && side.isRight && !isWall)
                {
                    //���ɓːi
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(-sidePos.x + objectScale.x / 2, transform.position.y, transform.position.z), MoveSpeed * Time.deltaTime);
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

            case ActionMode.TrackBullet: //��������`�����ǔ��̒e���o��

                trackBullet.IntervalTime += 1;
                if (!trackBullet.isWait)
                {
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, -4, transform.position.z), MoveSpeed * Time.deltaTime);

                }
                if (isFloorHit)
                {
                    trackBullet.isWait = true;
                }
                if (trackBullet.IntervalTime >= 200 && trackBulletCount < trackBulletCountMax)
                {
                    Instantiate(Bullet, transform.position, Quaternion.identity);
                    trackBullet.IntervalTime = 0;
                    trackBulletCount += 1;
                }
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
