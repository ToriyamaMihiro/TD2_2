using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TyutorialManager : MonoBehaviour
{
    private TD2_2 inputAcution;
    //Sliderを入れる
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
        //コントローラーを使うためのもの
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
        //指定回数以上押したらジャンプのチュートリアル終了
        if (jumpCount >= jumpMaxCount)
        {
            isJump = true;
        }
        //スライダーの数字をジャンプの数字にする
        if (!isJump)
        {
            //intで切り捨てられちゃうのでfloat型に直してる
            slider.value = (float)jumpCount / (float)jumpMaxCount;
        }

        //練習用のボスをだす
        if (isJump && !isBossCall)
        {
            Instantiate(Boss, new Vector2(0, -2.5f), Quaternion.identity);
            isBossCall = true;
        }

        //押す攻撃
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

        //潰す攻撃
        if (inputAcution.Player.Attack.WasPressedThisFrame() && weapon.isAttack && !weapon.isDashAttack && isJump && isXAttack && YCount <= YMaxCount)
        {
            YCount += 1;
        }
        //もし変形したら下に移動して浮いていない風にする

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
