using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;

public class BossCounterParticle : MonoBehaviour
{
    //transformで毎フレーム取得すると負荷が掛かる為、別に参照を保持。
    Transform tf;

    //-1.0fで時計回り、1.0fで反時計回り。
    float direction = -1.0f;

    //移動速度というか移動角度。
    float moveSpeed;

    //プレイヤーを追尾する速度のレート(大きい程高速)。
    float followRate = 0.1f;

    //追尾するポイントのプレイヤーからの距離(つまり小さい程、近付く)。
    float followTargetDistance;

    //カウンターになったときどれだけ元の位置から離すか
    float distancePlus;

    float distance;

    int destroyTime;

    // Start is called before the first frame update
    void Start()
    {
        tf = transform;

        followTargetDistance = Random.Range(0.1f, 0.8f);
        moveSpeed = Random.Range(2f, 4.5f);
        distancePlus = Random.Range(1f, 3f);
        distance = followTargetDistance;
    }

    // Update is called once per frame
    void Update()
    {

        BossAttackAction boss;
        GameObject obj = GameObject.Find("Boss");
        boss = obj.GetComponent<BossAttackAction>();

        //カウンターになったらかつ元の距離と足した数になるまでだんだん距離を遠くする
        if (boss.isCounter && followTargetDistance <= distance + distancePlus)
        {
            followTargetDistance = followTargetDistance + 0.1f;
        }

        //プレイヤーを一定の距離で追尾。
        tf.position = Vector3.Lerp(tf.position, boss.transform.position + (tf.position - boss.transform.position).normalized * followTargetDistance, followRate);
        //プレイヤーを中心に円運動。
        tf.RotateAround(boss.transform.position, Vector3.forward, direction * moveSpeed);

        destroyTime += 1;
        if (destroyTime >= boss.counterMaxTime + 40)
        {
            boss.isCounter = false;
            Destroy(this.gameObject);
        }

    }
}
