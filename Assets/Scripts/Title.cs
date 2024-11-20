using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Title : MonoBehaviour
{
    [SerializeField]
    Vector3 size = new Vector3(1.5f, 1.5f, 1);

    // Start is called before the first frame update
    void Start()
    {
        // FPSを設定(1秒 = 60fps)
        Application.targetFrameRate = 60;
        //ウィンドウサイズ
        Screen.SetResolution(1280, 720, false);

        transform.DOScale(size, 0.5f).SetLoops(-1, LoopType.Yoyo);
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
