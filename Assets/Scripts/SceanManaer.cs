using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceanManaer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;//frameレートの固定
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
            }
        }
        if (SceneManager.GetActiveScene().name == "Game")
        {
            PlayerAction player;
            GameObject obj = GameObject.Find("Player");
            player = obj.GetComponent<PlayerAction>();

            //プレイヤーかボスが死んだらリザルト画面へ
            if (player.isDead)
            {
                SceneManager.LoadScene(++nowSceneIndexNumber);
            }
        }
        if (SceneManager.GetActiveScene().name == "Result")
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene("Title");
            }
        }
    }
}
