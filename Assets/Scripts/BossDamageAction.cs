using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossDamageAction : MonoBehaviour
{
    SpriteRenderer bossRenderer;

    int hitTime;
    float level;

    // Start is called before the first frame update
    void Start()
    {
        bossRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Damage();
    }
    void Damage()
    {
        if (SceneManager.GetActiveScene().name == "Tyutorial")
        {
            TyutorialManager tyutorial;
            GameObject objT = GameObject.Find("TyutorialManager");
            tyutorial = objT.GetComponent<TyutorialManager>();
            if (tyutorial.isJump)
            {

                BossAction boss;
                GameObject obj1 = GameObject.Find("PracticeBoss(Clone)");
                boss = obj1.GetComponent<BossAction>();

                if (boss.isDamageHit)
                {
                    //プレイヤーの色を点滅させて無敵時間だと分かりやすくする
                    gameObject.GetComponent<SpriteRenderer>().color = new UnityEngine.Color(1, 1, 1, 0.5f);

                    //if (level <= 0.5)
                    //{
                    //    level += 0.05f;
                    //}

                    //毎フレーム呼び出させないため

                }
                else
                {
                    bossRenderer.color = new UnityEngine.Color(1f, 1f, 1f, 0f);
                    hitTime = 0;
                }

            }
        }

        if (SceneManager.GetActiveScene().name == "Game")
        {
            BossAction boss;
            GameObject obj = GameObject.Find("Boss");
            boss = obj.GetComponent<BossAction>();

            if (boss.isDamageHit)
            {
                //プレイヤーの色を点滅させて無敵時間だと分かりやすくする
                gameObject.GetComponent<SpriteRenderer>().color = new UnityEngine.Color(1, 1, 1, 0.5f);

                //if (level <= 0.5)
                //{
                //    level += 0.05f;
                //}

                //毎フレーム呼び出させないため

            }
            else
            {
                bossRenderer.color = new UnityEngine.Color(1f, 1f, 1f, 0f);
                hitTime = 0;
            }
        }
    }

    void WaitFor()
    {
        //ボスの色を元に戻す
        bossRenderer.color = new UnityEngine.Color(1f, 1f, 1f, 0f);
        hitTime = 0;
    }
}
