using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class AttackAction : MonoBehaviour
{

    private Rigidbody2D rb;

    public GameObject LDust;
    public GameObject RDust;

    Vector3 weponPos;//�U�艺�낷�ۂ̈ʒu���擾����
    Vector3 startPos = new Vector3(-1.3f, 0.45f, 0);//�������p�̈ʒu���擾

    float movePower = 0.8f;

    public int attackTime = 0;
    public int attackFullTime = 21;//�U���̃��Z�b�g�̎���
    int comboTime = 0;
    int comboMaxTime = 200;
    public int comboCount = 0;
    public int comboCountMax = 4;

    public bool isAttack;
    public bool isDashAttack;
    bool isCombo;
    bool isFloorHit;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        Attack();
        DashAttack();
        DustCall();
        Combo();
    }

    void Attack()
    {
        ////�{�^������������90�x��]�����ĐU�艺�낷
        if (Input.GetKeyDown(KeyCode.Z) && !isAttack)
        {
            transform.Rotate(0, 0, 90);
            weponPos = transform.position;
            isAttack = true;
            //�R���{�r�؂�鎞�Ԃ̃��Z�b�g
            comboTime = 0;
            comboCount += 1;
        }
        if (isAttack)
        {
            //�U�艺�낵�Ă��鎞�Ԃ��J�E���g
            attackTime += 1;

            transform.position = Vector3.MoveTowards(transform.position, new Vector3(weponPos.x, weponPos.y - movePower, weponPos.z), 2 * Time.deltaTime);

            //�U�艺�낵�I������烊�Z�b�g����
            if (attackTime >= attackFullTime)
            {
                isAttack = false;
                transform.Rotate(0, 0, -90);
                transform.localPosition = startPos;
                attackTime = 0;
            }
        }
    }

    void DashAttack()
    {
        if (Input.GetKeyDown(KeyCode.X) && !isDashAttack)
        {
            weponPos = transform.position;
            isDashAttack = true;
            //�R���{�r�؂�鎞�Ԃ̃��Z�b�g
            comboTime = 0;
            comboCount += 1;
        }
        if (isDashAttack)
        {
            PlayerAction player;
            GameObject obj = GameObject.Find("Player");
            player = obj.GetComponent<PlayerAction>();

            //�U�����Ă��鎞�Ԃ��J�E���g
            attackTime += 1;

            //�v���C���[���E�������Ă�����
            if (player.direction == player.transform.right)
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(weponPos.x + movePower, weponPos.y, weponPos.z), 2 * Time.deltaTime);
            }

            //�v���C���[�����������Ă�����
            if (player.direction == -player.transform.right)
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(weponPos.x - movePower, weponPos.y, weponPos.z), 2 * Time.deltaTime);
            }

            //�U�����I������烊�Z�b�g����
            if (attackTime >= attackFullTime)
            {
                isDashAttack = false;
                transform.localPosition = startPos;
                attackTime = 0;
            }
        }
    }

    void Combo()
    {
        //�Ȃɂ�����U���������Ƃ�
        if (isAttack || isDashAttack)
        {
            isCombo = true;
        }
        if (isCombo)
        {
        Debug.Log(comboCount);
            comboTime += 1;
            //�R���{�I�����Ԃ܂Ŏ��̍U�����Ȃ�������R���{�����Z�b�g����
            if (comboTime >= comboMaxTime)
            {
                isCombo = false;
                comboCount = 0;
            }
            if (comboCount == comboCountMax + 1)
            {
                isCombo = false;
                comboCount = 0;
            }
        }
    }

    //����@������`�����Ăяo��
    void DustCall()
    {

    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            //���킪���ɕt������`�����ł�
            Instantiate(RDust, transform.position, Quaternion.identity);
            Instantiate(LDust, transform.position, Quaternion.identity);

        }
    }
}

