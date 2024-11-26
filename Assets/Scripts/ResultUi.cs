using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultUi : MonoBehaviour
{
    [SerializeField]
    Vector3 size = new Vector3(1.5f, 1.5f, 1);
    [SerializeField]
    float toTime = 2f;

    // Start is called before the first frame update
    void Start()
    {
        
        //ウィンドウサイズ
        Screen.SetResolution(1280, 720, false);

        transform.DOScale(size, toTime).SetLoops(-1, LoopType.Yoyo).SetLink(gameObject);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
