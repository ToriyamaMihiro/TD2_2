using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossAction : MonoBehaviour
{

    int xCount;//x��������󂯂��񐔁A�c�ɐL�т�
    int yCount;//y��������󂯂��񐔁A���ɐL�т�

    int deformationCount = 3;//�ό`����܂ł̉�
    int life = 100;
    int dustDamage = 1;
    int attackDamage = 2;
    public int deformationTime;

    int currentHp;
    //Slider������
    public Slider slider;

    float knockBackPower = 1;

    public bool isXDeformation;//X�����ɕό`������
    public bool isYDeformation;//Y�����ɕό`������
    bool isHit;
    bool isFloor;
    bool isWall;
    public bool isDead;
    bool isKnockBack;

    // Start is called before the first frame update
    void Start()
    {
        //Slider�𖞃^���ɂ���B
        slider.value = 1;
        //���݂�HP���ő�HP�Ɠ����ɁB
        currentHp = life;
    }

    // Update is called once per frame
    void Update()
    {
        Deformation();
        HitRest();
        Dead();
        Range();
        //HP�v�Z
        slider.value = (float)currentHp / (float)life;
    }

    //�m�b�N�o�b�N�ŊO�ɍs���Ȃ��悤�ɂ���
    void Range()
    {
        //���݂̃|�W�V������ێ�����
        Vector3 currentPos = transform.position;

        //Mathf.Clamp��X,Y�̒l���ꂼ�ꂪ�ŏ��`�ő�͈͓̔��Ɏ��߂�B
        //���������̂���isTrigger�ɂ��������A���͓˂������Ăق����Ȃ��̂Ŗ������͈͂����߂ė����Ȃ��悤�ɂ���
        currentPos.x = Mathf.Clamp(currentPos.x, -7.5f, 7.5f);

        //position��currentPos�ɂ���
        transform.position = currentPos;

    }

    //�ό`
    void Deformation()
    {
        //�ό`������J�E���g�����Z�b�g
        if (isXDeformation || isYDeformation)
        {
            xCount = 0;
            yCount = 0;
            deformationTime += 1;
        }

        //�{�X�̍U�����łȂ���Ό��ɖ߂�
        BossAttackAction bossAttack = GetComponent<BossAttackAction>();
        if (bossAttack.isDeformationFinish)
        {
            isXDeformation = false;
            isYDeformation = false;
            deformationTime = 0;
        }
        //���ꂼ��̕����֕ό`
        if (isXDeformation)
        {
            transform.localScale = new Vector3(1.5f, 4, 1);
        }
        if (isYDeformation)
        {
            transform.localScale = new Vector3(4, 1.5f, 1);
        }
    }

    //�U���̂��тɃJ�E���g�����邽�߂̃��Z�b�g
    void HitRest()
    {
        if (isHit)
        {
            AttackAction weapon;
            GameObject obj = GameObject.Find("Weapon");
            weapon = obj.GetComponent<AttackAction>();

            //�U�����I������isHit��false�ɂ��čU���̂��тɃJ�E���g�����悤�ɂ���
            if (!weapon.isAttack && !weapon.isDashAttack)
            {
                isHit = false;
            }
        }
    }

    void Dead()
    {
        if (currentHp <= 0)
        {
            isDead = true;
        }
    }

    //�ό`�̏���
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Weapon")
        {
            AttackAction weapon;
            GameObject obj = GameObject.Find("Weapon");
            weapon = obj.GetComponent<AttackAction>();

            if (!isHit)
            {
                //�R���{���}�b�N�X�����U�������������ɂ����ꍇ�m�b�N�o�b�N����
                if (weapon.comboCount == weapon.comboCountMax && weapon.isDashAttack && isFloor)
                {
                    //�R���{�ł̃m�b�N�o�b�N
                    Vector3 distination = (transform.position - collision.transform.position).normalized;

                    transform.Translate(distination.x * knockBackPower, 0f, 0f);
                    weapon.comboCount = 0;
                }

                //�ό`�̃J�E���g
                if (weapon.isAttack)
                {
                    if (!isXDeformation && !isYDeformation)
                    {
                        yCount += 1;
                    }
                    currentHp = currentHp - attackDamage;
                    isHit = true;
                }
                if (weapon.isDashAttack)
                {
                    if (!isXDeformation && !isYDeformation)
                    {
                        xCount += 1;
                    }
                    currentHp = currentHp - attackDamage;
                    isHit = true;
                }
            }
            if (xCount == deformationCount)
            {
                isXDeformation = true;
            }
            if (yCount == deformationCount)
            {
                isYDeformation = true;
            }
        }
    }

    //Dust�̃_���[�W����
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Dust")
        {
            currentHp = currentHp - dustDamage;
            //���������I�u�W�F�N�g���폜����
            Destroy(collision.gameObject);
        }
    }
    //���ɋ�����m�b�N�o�b�N����
    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            if (!isFloor)
            {
                isFloor = true;
            }
        }
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            if (isFloor)
            {
                isFloor = false;
            }
        }
    }
}
