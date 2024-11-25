using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TyutorialManager : MonoBehaviour
{
    private TD2_2 inputAcution;
    //Slider������
    public Slider slider;

    public GameObject Boss;

    int jumpCount;
    int jumpMaxCount = 6;

    int XCount;
    int XMaxCount = 4;

    int YCount;
    int YMaxCount = 4;

    bool isJump;
    bool isXAttack;
    public bool isYAttack;
    bool isBossCall;

    // Start is called before the first frame update
    void Start()
    {
        //�R���g���[���[���g�����߂̂���
        inputAcution = new TD2_2();
        inputAcution.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerAction player;
        GameObject obj = GameObject.Find("Player");
        player = obj.GetComponent<PlayerAction>();

        AttackAction weapon;
        GameObject objW = GameObject.Find("Weapon");
        weapon = objW.GetComponent<AttackAction>();

        if (jumpCount <= jumpMaxCount)
        {
            jumpCount = player.tyutorialJumpCount;
        }
        //�w��񐔈ȏ㉟������W�����v�̃`���[�g���A���I��
        if (jumpCount >= jumpMaxCount)
        {
            isJump = true;
        }
        //�X���C�_�[�̐������W�����v�̐����ɂ���
        if (!isJump)
        {
            //int�Ő؂�̂Ă�ꂿ�Ⴄ�̂�float�^�ɒ����Ă�
            slider.value = (float)jumpCount / (float)jumpMaxCount;
        }

        //���K�p�̃{�X������
        if (isJump && !isBossCall)
        {
            Instantiate(Boss, new Vector2(0, -2.5f), Quaternion.identity);
            isBossCall = true;
        }

        //�����U��
        if (inputAcution.Player.DashAttack.WasPressedThisFrame() && !weapon.isAttack && weapon.isDashAttack && isJump && XCount <= XMaxCount)
        {
            XCount += 1;
        }
        if (XCount >= XMaxCount)
        {
            isXAttack = true;
        }
        if (isJump && !isXAttack)
        {
            slider.value = (float)XCount / (float)XMaxCount;
        }

        //�ׂ��U��
        if (inputAcution.Player.Attack.WasPressedThisFrame() && weapon.isAttack && !weapon.isDashAttack && isJump && isXAttack && YCount <= YMaxCount)
        {
            YCount += 1;
        }
        //�����ό`�����牺�Ɉړ����ĕ����Ă��Ȃ����ɂ���

        if (isJump && isXAttack && !isYAttack)
        {
            slider.value = (float)YCount / (float)YMaxCount;
        }
        if (YCount >= YMaxCount)
        {
            isYAttack = true;
        }
    }
}
