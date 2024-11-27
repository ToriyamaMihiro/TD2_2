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
    //Sliderを入れる
    public Slider skipSlider;

    public GameObject Boss;

    public Sprite[] uiSprite = new Sprite[3];
    //UIまわり
    public GameObject tutorial_UI;
    public GameObject ok_UI;
    public Ease ease;
    public Vector3 toSize = new Vector3(2.5f, 2.5f, 1);
    private SpriteRenderer uiSpriteRenderer;
    bool isFinishUI;
    bool isOkUI;

    int jumpCount;
    int jumpMaxCount = 6;

    int XCount;
    int XMaxCount = 6;

    int YCount;
    int YMaxCount = 5;

    int skipTimer;

    public bool isJump;
    public bool isXAttack;
    public bool isYAttack;
    bool isBossCall;
    public bool isAttack;//攻撃しているかしていないかの判定
    public bool isSkip;

    // Start is called before the first frame update
    void Start()
    {
        //コントローラーを使うためのもの
        inputAcution = new TD2_2();
        inputAcution.Enable();
        //開始UI
        uiSpriteRenderer = tutorial_UI.GetComponent<SpriteRenderer>();
        tutorial_UI.transform.DOScale(toSize, 1f).SetEase(ease).SetLink(gameObject);
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
        Count();
        Skip();
    }
    void Count()
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
        if (isJump && !isXAttack && !isBossCall)
        {
            Instantiate(Boss, new Vector2(0, -2.5f), Quaternion.identity);
            tutorial_UI.transform.DOScale(new Vector3(0, 0, 0), 0.5f).SetEase(ease).SetLoops(2, LoopType.Yoyo).SetLink(gameObject);
            ok_UI.transform.DOScale(new Vector3(4, 4, 1), 1f).SetLink(gameObject);
            Invoke("OkUI", 1.5f);
            isBossCall = true;
        }

        //カウントするためのリセット
        if (!weapon.isAttack && !weapon.isDashAttack)
        {
            isAttack = false;
        }

        //押す攻撃しているかつジャンプのチュートリアルが終わっていてXのカウントがマックス以下ならカウントする
        if (XCount < XMaxCount)
        {
            XCount = weapon.tyutorialXCount;
            ////攻撃中に一回だけカウントするようにする
            //isAttack = true;
        }
        if (XCount >= XMaxCount && !isXAttack)//たたく終了
        {
            isXAttack = true;
            isBossCall = false;
        }
        //もしジャンプのチュートリアルが終わっていて押す攻撃のチュートリアルが終わっていなかったら
        if (isJump && !isXAttack)
        {
            slider.value = (float)XCount / (float)XMaxCount;
        }
        if (isJump && !isXAttack && !isYAttack)//たたく中
        {
            Invoke("ChangeSpriteHit", 1f);
        }
        if (isJump && isXAttack && !isYAttack)
        {
            if (!isFinishUI)
            {
                tutorial_UI.transform.DOScale(new Vector3(0, 0, 0), 1.5f).SetEase(ease).SetLoops(2, LoopType.Yoyo).SetLink(gameObject);
                ok_UI.transform.DOScale(new Vector3(4, 4, 1), 1f).SetLink(gameObject);
                Invoke("OkUI", 1.5f);
                isFinishUI = true;
            }
        }

        //潰す攻撃
        if (YCount < YMaxCount)
        {
            YCount = weapon.tyutorialYCount;
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
        if (isJump && isXAttack && isYAttack)
        {
            if (!isOkUI)
            {
                //OK出す
                ok_UI.transform.DOScale(new Vector3(4, 4, 1), 1f).SetLink(gameObject);
                Invoke("OkUI", 1.5f);
                isOkUI = true;
            }
        }

    }

    void OkUI()
    {
        ok_UI.transform.DOScale(new Vector3(0, 0, 1), 1f).SetLink(gameObject);
    }

    void Skip()
    {

        skipSlider.value = (float)skipTimer / (float)150;
        //押している間ゲージが溜まる
        if (inputAcution.Player.Skip.IsPressed())
        {
            skipTimer += 1;
            if (skipTimer >= 150)
            {
                isSkip = true;
            }
        }
        //溜まるまでに離すとリセット
        if (inputAcution.Player.Skip.WasReleasedThisFrame() && !isSkip)
        {
            skipTimer = 0;
        }
    }
}
