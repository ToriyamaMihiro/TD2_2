using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class AttackAction : MonoBehaviour
{

    private Rigidbody2D rb;
    public bool isAttack;

    Vector3 weponPos;//�U�艺�낷�ۂ̈ʒu���擾����
    Vector3 startPos = new Vector3(-1.3f, 0.45f, 0);//�������p�̈ʒu���擾

    int attackTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
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

            transform.position = Vector3.MoveTowards(transform.position, new Vector3(weponPos.x, weponPos.y - 0.8f, weponPos.z), 2 * Time.deltaTime);

            //�U�艺�낵�I������烊�Z�b�g����
            if (attackTime >= 201)
            {
                isAttack = false;
                transform.Rotate(0, 0, -90);
                transform.localPosition = startPos;
                attackTime = 0;
            }
        }
    }
}

