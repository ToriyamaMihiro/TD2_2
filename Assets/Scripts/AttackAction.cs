using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class AttackAction : MonoBehaviour
{

    private Rigidbody2D rb;
    public bool isAttack;

    Vector3 weponPos;//振り下ろす際の位置を取得する
    Vector3 startPos = new Vector3(-1.3f, 0.45f, 0);//初期化用の位置を取得

    int attackTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        ////ボタンを押したら90度回転させて振り下ろす
        if (Input.GetKeyDown(KeyCode.Z) && !isAttack)
        {
            transform.Rotate(0, 0, 90);
            weponPos = transform.position;
            isAttack = true;
        }
        if (isAttack)
        {
            //振り下ろしている時間をカウント
            attackTime += 1;

            transform.position = Vector3.MoveTowards(transform.position, new Vector3(weponPos.x, weponPos.y - 0.8f, weponPos.z), 2 * Time.deltaTime);

            //振り下ろし終わったらリセットする
            if (attackTime >= 201)
            {
                isAttack = false;
                transform.Rotate(0, 0, -90);
                transform.localPosition = startPos;
                attackTime = 0;
            }
        }
    }
}

