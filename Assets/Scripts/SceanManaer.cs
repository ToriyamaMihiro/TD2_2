using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SceanManaer : MonoBehaviour
{
    public GameObject Clear;
    public GameObject GameOver;
    public GameObject SceanChange;

    private TD2_2 inputAcution;

    //シーンを跨いだ変数を取得するため
    public static bool isPlayerDead = false;
    public static bool isBossDead = false;

    //音
    private AudioSource audioSource;
    public AudioClip desitionAudio;
    public AudioClip gameOverAudio;
    public AudioClip gameClearAudio;
    public AudioClip selectAudio;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;//frameレートの固定
        Clear.SetActive(false);
        GameOver.SetActive(false);
        audioSource = GetComponent<AudioSource>();
        //コントローラーを使うためのもの
        inputAcution = new TD2_2();
        inputAcution.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        int nowSceneIndexNumber = SceneManager.GetActiveScene().buildIndex;

        //リセット
        if (Input.GetKeyDown(KeyCode.R))
        {
            nowSceneIndexNumber = SceneManager.GetActiveScene().buildIndex;

            SceneManager.LoadScene(nowSceneIndexNumber);
        }
        if (inputAcution.Player.Title.WasPressedThisFrame())
        {
            SceneManager.LoadScene("Title");
        }

        if (SceneManager.GetActiveScene().name == "Title")
        {
            //タイトルからゲームへ
            if (inputAcution.Player.Jump.WasPressedThisFrame())
            {
                Instantiate(SceanChange, new Vector2(0, 0), Quaternion.identity);
                audioSource.PlayOneShot(desitionAudio);//音
                Invoke("LoadScean", 0.7f);
            }
        }
        if (SceneManager.GetActiveScene().name == "Tyutorial")
        {
            TyutorialManager tyutorial;
            GameObject objT = GameObject.Find("TyutorialManager");
            tyutorial = objT.GetComponent<TyutorialManager>();



            if (tyutorial.isYAttack||tyutorial.isSkip)
            {
                Invoke("Call", 1.5f);
                Invoke("LoadScean", 2.2f);
            }

        }

        if (SceneManager.GetActiveScene().name == "Game")
        {
            PlayerAction player;
            GameObject obj = GameObject.Find("Player");
            player = obj.GetComponent<PlayerAction>();

            BossAction boss;
            GameObject bossObj = GameObject.Find("Boss");
            boss = bossObj.GetComponent<BossAction>();


            if (boss.isDead)
            {
                isBossDead = boss.isDead;
            }

            if (player.isDead)
            {
                isPlayerDead = player.isDead;
            }

            //プレイヤーかボスが死んだらリザルト画面へ
            if (player.isDead)
            {
                Instantiate(SceanChange, new Vector2(0, 0), Quaternion.identity);
                Invoke("LoadScean", 0.7f);
            }
            if (boss.isDead)
            {
                Invoke("SceneChangeON", 4f);
                Invoke("LoadScean", 5f);
            }
        }

        if (SceneManager.GetActiveScene().name == "Result")
        {
            //リザルトで結果を表示する
            if (isBossDead)
            {
                Clear.SetActive(true);
                GameOver.SetActive(false);
            }

            if (isPlayerDead)
            {
                Clear.SetActive(false);
                GameOver.SetActive(true);
            }

            if (inputAcution.Player.Jump.WasPressedThisFrame())
            {
                audioSource.PlayOneShot(desitionAudio);//音
                isPlayerDead = false;
                isBossDead = false;

                Instantiate(SceanChange, new Vector2(0, 0), Quaternion.identity);
                Invoke("LoadSceanResult", 0.7f);
            }
        }
    }

    void SceneChangeON()
    {
        Instantiate(SceanChange, new Vector2(0, 0), Quaternion.identity);
    }

    void LoadScean()
    {
        int nowSceneIndexNumber = SceneManager.GetActiveScene().buildIndex;

        SceneManager.LoadScene(++nowSceneIndexNumber);
    }

    void Call()
    {
        Instantiate(SceanChange, new Vector2(0, 0), Quaternion.identity);
    }
    void LoadSceanResult()
    {
        OptionAction option;
        GameObject objOp = GameObject.FindWithTag("Option");
        option = objOp.GetComponent<OptionAction>();

        if (option.isRight)
        {

            SceneManager.LoadScene("Title");
        }
        else
        {
            SceneManager.LoadScene("Game");
        }
    }
}
