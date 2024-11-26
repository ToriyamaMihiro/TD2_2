using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.Mathematics;

public class PracticeBossAction : MonoBehaviour
{
    public GameObject Boss2;

    int time;

    int moveTime;

    // Start is called before the first frame update
    void Start()
    {
        //登場イージング
        //  transform.DOScale(new Vector3(3, 3, 1), 3f).SetEase(Ease.InOutCubic).SetLink(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

        //初めの練習用の敵は消す
        if (gameObject.name == "PracticeBoss(Clone)")
        {
            TyutorialManager tyutorial;
            GameObject objT = GameObject.Find("TyutorialManager");
            tyutorial = objT.GetComponent<TyutorialManager>();
            if (tyutorial.isXAttack)
            {
                time += 1;
            }

            if (time == 50)
            {
                //退場イージング
                //変形すると0まで行ってくれない
                transform.DOScale(new Vector3(0, 0, 1), 1.5f).SetEase(Ease.InOutCubic).SetLink(this.gameObject);
            }

        }

        //消したら次のボスを呼び出す

        if (transform.localScale.x <= 0)
        {
            Instantiate(Boss2, new Vector2(0, -2.5f), Quaternion.identity);
            Destroy(gameObject);
        }

        if (gameObject.name == "PracticeBoss2(Clone)")
        {
            BossAction boss;
            GameObject obj = GameObject.Find("PracticeBoss2(Clone)");
            boss = obj.GetComponent<BossAction>();

            if (boss.isYDeformation)
            {
                moveTime += 1;
                if (moveTime == 1)
                {
                    //横長になったら床に着くように移動
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z), 100 * Time.deltaTime);
                }
            }
        }
    }
}
