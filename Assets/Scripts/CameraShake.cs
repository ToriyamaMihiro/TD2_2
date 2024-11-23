using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraShake : MonoBehaviour
{

    [SerializeField] private Transform cam;
    [SerializeField] private Vector3 positionStrength = new(0, 0, 0);
    [SerializeField] private Vector3 rotationStrength = new(2, 2, 2);
    private float shakeDuration = 0.3f;



    // Start is called before the first frame update
    void Start()
    {
        // FPSを設定(1秒 = 60fps)
        Application.targetFrameRate = 60;
    }
    private void CameraShaker()
    {

        cam.DOComplete();
        cam.DOShakePosition(shakeDuration, positionStrength);
        cam.DOShakeRotation(shakeDuration, rotationStrength);
    }

    // Update is called once per frame
    void Update()
    {

        //床に手が当たったらシェイク
        AttackAction playerscript;
        GameObject obj = GameObject.Find("Weapon");
        playerscript = obj.GetComponent<AttackAction>();

        if (playerscript.isAttackShake)
        {
            CameraShaker();
            playerscript.isAttackShake = false;
        }

        //ボスが上から攻撃したとき
        BossAttackAction boss;
        GameObject objB = GameObject.Find("Boss");
        boss = objB.GetComponent<BossAttackAction>();

        if (boss.isShake)
        {
            CameraShaker();
            boss.isShake = false;
        }

        //ここにシェイクのタイミング描く
        if (Input.GetKeyDown(KeyCode.Q))
        {
            CameraShaker();
        }
    }
}
