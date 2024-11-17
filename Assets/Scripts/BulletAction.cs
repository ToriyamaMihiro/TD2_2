using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BulletAction : MonoBehaviour
{
    Vector3 offset;
    Vector3 target;
    float deg;
    float speed = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        //プレイヤーの方向に60度の角度で放出
        SetTarget(player.transform.position, 60);
    }

    // Update is called once per frame
    IEnumerator ThrowBall()
    {
        float b = Mathf.Tan(deg * Mathf.Deg2Rad);
        float a = (target.y - b * target.x) / (target.x * target.x);

        for (float x = 0; x <= this.target.x + 5; x += speed)
        {
            float y = a * x * x + b * x;
            transform.position = new Vector3(x, y, 0) + offset;
            yield return null;
        }
    }

    public void SetTarget(Vector3 target, float deg)
    {
        this.offset = transform.position;
        this.target = target - this.offset;
        this.deg = deg;

        StartCoroutine("ThrowBall");
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        //何かに当たったら消す
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Floor")
        {
            Destroy(this.gameObject);

        }
    }
}
