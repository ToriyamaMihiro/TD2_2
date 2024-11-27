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

<<<<<<< HEAD
                    //if (level <= 0.5)
                    //{
                    //    level += 0.05f;
                    //}
=======
            //if (boss.isDamageHit)
            //{
            //    //繝励Ξ繧､繝､繝ｼ縺ｮ濶ｲ繧堤せ貊・＆縺帙※辟｡謨ｵ譎る俣縺縺ｨ蛻・°繧翫ｄ縺吶￥縺吶ｋ
            //    gameObject.GetComponent<SpriteRenderer>().color = new UnityEngine.Color(1, 1, 1, 0.5f);
>>>>>>> dead繝懊せ繧｢繝九Γ螟峨∴縺・

                    //毎フレーム呼び出させないため

<<<<<<< HEAD
                }
                else
                {
                    bossRenderer.color = new UnityEngine.Color(1f, 1f, 1f, 0f);
                    hitTime = 0;
                }
=======
            //    //豈弱ヵ繝ｬ繝ｼ繝蜻ｼ縺ｳ蜃ｺ縺輔○縺ｪ縺・◆繧・
>>>>>>> dead繝懊せ繧｢繝九Γ螟峨∴縺・

            }
        }

        if (SceneManager.GetActiveScene().name == "Game")
        {
            BossAction boss;
            GameObject obj = GameObject.Find("Boss");
            boss = obj.GetComponent<BossAction>();

            if (boss.isDamageHit)
            {
                //繝励Ξ繧､繝､繝ｼ縺ｮ濶ｲ繧堤せ貊・＆縺帙※辟｡謨ｵ譎る俣縺縺ｨ蛻・°繧翫ｄ縺吶￥縺吶ｋ
                gameObject.GetComponent<SpriteRenderer>().color = new UnityEngine.Color(1, 1, 1, 0.5f);

                //if (level <= 0.5)
                //{
                //    level += 0.05f;
                //}

                //豈弱ヵ繝ｬ繝ｼ繝蜻ｼ縺ｳ蜃ｺ縺輔○縺ｪ縺・◆繧・

            }
            else
            {
                bossRenderer.color = new UnityEngine.Color(1f, 1f, 1f, 0f);
                hitTime = 0;
            }

            //繝懊せ豁ｻ繧薙□繧蛾乗・縺ｫ縺ｪ繧・
            if (boss.isDead)
            {
                gameObject.GetComponent<SpriteRenderer>().color = new UnityEngine.Color(0, 0, 0, 0f);
            }
        }
    }

    void WaitFor()
    {
        //繝懊せ縺ｮ濶ｲ繧貞・縺ｫ謌ｻ縺・
        bossRenderer.color = new UnityEngine.Color(1f, 1f, 1f, 0f);
        hitTime = 0;
    }
}
