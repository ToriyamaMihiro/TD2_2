using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionAction : MonoBehaviour
{
    private TD2_2 inputAcution;

    public GameObject Retry;
    public GameObject Title;

    public bool isRight = false;//右を選択しているか

    // Start is called before the first frame update
    void Start()
    {
        inputAcution = new TD2_2();
        inputAcution.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if (inputAcution.Player.Left.IsPressed())
        {
            isRight = false;
        }

        if (inputAcution.Player.Right.IsPressed())
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
