using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceanManaer : MonoBehaviour
{
    public GameObject Clear;
    public GameObject GameOver;

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
        if (SceneManager.GetActiveScene().name == "Title")
        {
            //タイトルからゲームへ
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(++nowSceneIndexNumber);
                audioSource.PlayOneShot(desitionAudio);//音
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
            if (player.isDead || boss.isDead)
            {
                SceneManager.LoadScene(++nowSceneIndexNumber);
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

            if (Input.GetKeyDown(KeyCode.Space))
            {
                audioSource.PlayOneShot(desitionAudio);//音
                isPlayerDead = false;
                isBossDead = false;

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
    }
}
