using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_BossHPShake : MonoBehaviour
{

    private Tweener shakeTweener;
    private Vector3 initPosition;
    [SerializeField] private Vector3 positionStrength = new(0, 0, 0);
    [SerializeField] private Vector3 rotationStrength = new(2, 2, 2);
    private float shakeDuration = 0.6f;

    // Start is called before the first frame update
    void Start()
    {
        initPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //ここにシェイクのタイミング描く
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Shaker();
        }

        BossAction boss;
        GameObject obj = GameObject.Find("Boss");
        boss = obj.GetComponent<BossAction>();
        if (boss.isDamage)
        {
            Shaker();
            boss.isDamage = false;
        }
    }

    private void Shaker()
    {

        gameObject.transform.DOComplete();
        gameObject.transform.DOShakePosition(shakeDuration, positionStrength).SetLink(gameObject);
        gameObject.transform.DOShakeRotation(shakeDuration, rotationStrength).SetLink(gameObject);
    }


    void StartShake(float duration, float strength, int vibrato, float randomness, bool fadeOut)
    {
        // 前回の処理が残っていれば停止して初期位置に戻す
        if (shakeTweener != null)
        {
            shakeTweener.Kill();
            gameObject.transform.position = initPosition;
        }
        // 揺れ開始
        shakeTweener = gameObject.transform.DOShakePosition(duration, strength, vibrato, randomness, fadeOut);
    }
}
