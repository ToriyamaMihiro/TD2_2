using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SizeEase : MonoBehaviour
{
    public Vector3 toSize =new Vector3(1,1,1);
    public Ease ease;
    public float finishTime = 4;
    private float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        transform.DOScale(toSize,1f).SetEase(ease);
    }

    // Update is called once per frame
    void Update()
    {
        timer += 1 * Time.deltaTime;
        if (timer > finishTime)
        {
            transform.DOScale(new Vector3(0,0,0), 1f).SetEase(ease);
            timer = 0;
        }
    }
}
