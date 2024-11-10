using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionAction : MonoBehaviour
{
    public GameObject Retry;
    public GameObject Title;

    public bool isRight = false;//右を選択しているか

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            isRight = false;
        }

        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            isRight = true;
        }
        if (isRight)
        {
            //拡大拡縮アニメーション
            Title.transform.localScale = new Vector3(1f, 1f, 1f);
            Retry.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
        }
        if (!isRight)
        {
            //拡大拡縮アニメーション
            Title.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
            Retry.transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }
}
