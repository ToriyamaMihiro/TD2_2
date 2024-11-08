using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class AttackAction : MonoBehaviour
{

    private Rigidbody2D rb;

    Vector3 weponPos;//�U�艺�낷�ۂ̈ʒu���擾����
    Vector3 startPos = new Vector3(-1.3f, 0.45f, 0);//�������p�̈ʒu���擾

    float movePower = 0.8f;

    public int attackTime = 0;
    public int attackFullTime = 201;//�U���̃��Z�b�g�̎���

    public bool isAttack;
    public bool isDashAttack;

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
    }

    void Attack()
    {
        ////�{�^������������90�x��]�����ĐU�艺�낷
        if (Input.GetKeyDown(KeyCode.Z) && !isAttack)
        {
            transform.Rotate(0, 0, 90);
            weponPos = transform.position;
            isAttack = true;
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
        if (Input.GetKeyDown(KeyCode.C) && !isDashAttack)
        {
            weponPos = transform.position;
            isDashAttack = true;
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
}

