using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BulletAction : MonoBehaviour
{
    Vector3 offset;
    //Vector3 target;
    float deg;
    float speed = 0.2f;
    GameObject target;

    Rigidbody2D m_rigidbody;

    int time;//このオブジェクトが呼び出されてからの時間

    float power = 45;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        //プレイヤーの方向に60度の角度で放出
        m_rigidbody = this.GetComponent<Rigidbody2D>();
        // SetTarget(player.transform.position, 60);
        float angle = 40;

        float distance = Vector3.Distance(transform.position, target.transform.position);
        float gravity = Mathf.Abs(Physics.gravity.y) * m_rigidbody.gravityScale;
        float direction = angle * Mathf.Deg2Rad;
        float sin = Mathf.Sin(direction);
        float cos = Mathf.Cos(direction);
        float speed = Mathf.Sqrt((gravity * distance) / (2.0f * sin * cos)) * power; // forceは係数
        float speedX = speed;
        if (target.transform.position.x <= transform.position.x)
        {
            speedX *= -1;
        }
      
        m_rigidbody.AddForce(new Vector2(Mathf.Cos(direction) * speedX, Mathf.Sin(direction) * speed));
    }

    private void Update()
    {
        
    }

    // Update is called once per frame
    //IEnumerator ThrowBall()
    //{
    //    float b = Mathf.Tan(deg * Mathf.Deg2Rad);
    //    float a = (target.y - b * target.x) / (target.x * target.x);

    //    for (float x = 0; x <= this.target.x + 5; x += speed)
    //    {
    //        float y = a * x * x + b * x;
    //        transform.position = new Vector3(x, y, 0) + offset;
    //        yield return null;
    //    }
    //}

    //public void SetTarget(Vector3 target, float deg)
    //{
    //    this.offset = transform.position;
    //    this.target = target - this.offset;
    //    this.deg = deg;

    //    StartCoroutine("ThrowBall");
    //}
    void OnTriggerEnter2D(Collider2D collision)
    {
        //何かに当たったら消す
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Floor")
        {
            Destroy(this.gameObject);

        }
    }
}
