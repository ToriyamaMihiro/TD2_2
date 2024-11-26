using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TyutorialManager : MonoBehaviour
{
    private TD2_2 inputAcution;
    //Sliderを入れる
    public Slider slider;

    public GameObject Boss;

    public Sprite[] uiSprite = new Sprite[3];
    //UIまわり
    public GameObject tutorial_UI;
    public Ease ease;
    public Vector3 toSize = new Vector3(2.5f, 2.5f, 1);
    private SpriteRenderer uiSpriteRenderer;
    bool isFinishUI;
    bool isStartUI;

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
        //開始UI
        uiSpriteRenderer = tutorial_UI.GetComponent<SpriteRenderer>();
        tutorial_UI.transform.DOScale(toSize, 1f).SetEase(ease);
    }

    void ChangeSpriteHit()
    {
        //UIの画像
        uiSpriteRenderer.sprite = uiSprite[2];
    }
    void ChangeSpriteFlutter()
    {
        //UIの画像
        uiSpriteRenderer.sprite = uiSprite[1];
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
            //UIの画像をジャンプのにする
            uiSpriteRenderer.sprite = uiSprite[0];
        }
       

        //練習用のボスをだす
        if (isJump && !isBossCall)
        {
            Instantiate(Boss, new Vector2(0, -2.5f), Quaternion.identity);
            tutorial_UI.transform.DOScale(new Vector3(0,0,0), 0.5f).SetEase(ease).SetLoops(2,LoopType.Yoyo);

            isBossCall = true;
        }

        //押す攻撃
        if (inputAcution.Player.DashAttack.WasPressedThisFrame() && !weapon.isAttack && weapon.isDashAttack && isJump && XCount <= XMaxCount)
        {
            XCount += 1;
        }
        if (XCount >= XMaxCount)//たたく終了
        {
            isXAttack = true;
            
        }
        if (isJump && !isXAttack)
        {
            slider.value = (float)XCount / (float)XMaxCount;
        }
        if(isJump && !isXAttack && !isYAttack)//たたく中
        {
            Invoke("ChangeSpriteHit", 1f);
        }
        if (isJump && isXAttack && !isYAttack)
        {
            if (!isFinishUI)
            {
                tutorial_UI.transform.DOScale(new Vector3(0, 0, 0), 1.5f).SetEase(ease).SetLoops(2, LoopType.Yoyo);
                isFinishUI = true;
            }
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
        if (YCount >= YMaxCount)//はたく終了
        {
            isYAttack = true;
            
        }
        if (isJump && isXAttack && !isYAttack)//はたく中
        {
            Invoke("ChangeSpriteFlutter", 1f);
        }
    }

    
}
